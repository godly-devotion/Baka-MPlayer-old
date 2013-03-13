
-- quvi
-- Copyright (C) 2011, 2012  quvi project
--
-- This file is part of quvi <http://quvi.sourceforge.net/>.
--
-- This library is free software; you can redistribute it and/or
-- modify it under the terms of the GNU Lesser General Public
-- License as published by the Free Software Foundation; either
-- version 2.1 of the License, or (at your option) any later version.
--
-- This library is distributed in the hope that it will be useful,
-- but WITHOUT ANY WARRANTY; without even the implied warranty of
-- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
-- Lesser General Public License for more details.
--
-- You should have received a copy of the GNU Lesser General Public
-- License along with this library; if not, write to the Free Software
-- Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
-- 02110-1301  USA


-- TODO:
-- - Add support for Radio programmes
-- - Add support for live streaming
-- - Better error messages for geolocation errors
-- - Offer the subtitles for download somehow

-- Obtained with grep -oP '(?<=service=")[^"]+(?=")' on config
local fmt_id_lookup = {
  high     = 'iplayer_streaming_h264_flv_high',
  standard = 'iplayer_streaming_h264_flv',
  low      = 'iplayer_streaming_h264_flv_lo',
  vlow     = 'iplayer_streaming_h264_flv_vlo'
  -- iplayer_streaming_n95_3g
  -- iplayer_streaming_n95_wifi
}

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "www.bbc.co.uk"
    r.formats    = "default|best"
    for k,_ in pairs(fmt_id_lookup) do
        r.formats = r.formats .."|".. k
    end
    r.categories = C.proto_rtmp
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/iplayer/"})
    return r
end

-- Parse video URL.
function parse(self)

    function needs_new_authString(params)
        if not params['authString'] then
            return false
        end
        local found = false
        for _,kind in pairs{'limelight', 'akamai', 'level3', 'sis', 'iplayertok'} do
            if kind == params['kind'] then
                found = true
                break
            end
        end
        if not found then return false end
        -- We don't need to check for the mode, we already know it's what we want
        return true
    end

    function create_uri_for_limelight_level3_iplayertok(params)
        params.uri = params.tcurl .. '/' .. params.playpath
    end

    function process_akamai(params)
        params.playpath = params.identifier
        params.application = params.application or 'ondemand'
        params.application = params.application .. '?_fcs_vhost=' .. params.server .. '&undefined'
        params.uri = 'rtmp://' .. params.server .. ':80/' .. params.application
        if not params.authString:find("&aifp=") then
            params.authString = params.authString .. '&aifp=v001'
        end
        if not params.authString:find("&slist=") then
            params.identifier = params.identifier:gsub('^mp[34]:', '')
            params.authString = params.authString .. '&slist=' .. params.identifier
        end
        params.playpath = params.playpath .. '?' .. params.authString
        params.uri = params.uri .. '&' .. params.authString
        params.application = params.application .. '&' .. params.authString
        params.tcurl = 'rtmp://' .. params.server .. ':80/' .. params.application
    end

    function process_limelight_level3(params)
        params.application = params.application .. '?' .. params.authString
        params.tcurl = 'rtmp://' .. params.server .. ':1935/' .. params.application
        params.playpath = params.identifier
        create_uri_for_limelight_level3_iplayertok(params)
    end

    function process_iplayertok(params)
        params.identifier = params.identifier .. '?' .. params.authString
        params.playpath = params.identifier:gsub('^mp[34]:', '')
        params.tcurl = 'rtmp://' .. params.server .. ':1935/' .. params.application
        create_uri_for_limelight_level3_iplayertok(params)
    end

    self.host_id = 'bbc'

    local _,_,s = self.page_url:find('episode/(.-)/')
    local episode_id = s or error('no match: episode id')
    self.id = episode_id

    local playlist_uri =
        'http://www.bbc.co.uk/iplayer/playlist/' .. episode_id
    local playlist = quvi.fetch(playlist_uri, {fetch_type = 'playlist'})

    local pl_item_p,_,s = playlist:find('<item kind="programme".-identifier="(.-)"')
    if not s then
        pl_item_p,_,s = playlist:find('<item kind="radioProgramme".-identifier="(.-)"')
        -- TODO: Implement radio support
        if s then
            error('No support for radio yet')
        end
    end
    local media_id = s or error('no match: media id')

    local _,_,s = playlist:find('duration="(%d+)"', pl_item_p)
    self.duration = tonumber(s) or 0

    local _,_,s = playlist:find('<title>(.-)</title>')
    self.title = s or error('no match: video title')

    local _,_,s = playlist:find('<link rel="holding" href="(.-)"')
    self.media_thumbnail_url = s or ""

    -- stolen from http://lua-users.org/wiki/MathLibraryTutorial
    math.randomseed(os.time()) math.random() math.random() math.random()
    local config_uri =
        'http://www.bbc.co.uk/mediaselector/4/mtis/stream/' ..
        media_id .. "?cb=" .. math.random(10000)

    local config = quvi.fetch(config_uri, {fetch_type = 'config'})

    available_formats = {}
    for fmt_id in config:gmatch("iplayer_streaming_[%w_]+") do
        available_formats[fmt_id] = true
    end
    -- Create the list of acceptable formats, ordered by preference
    local r = self.requested_format
    local preferred = ((r == 'best') and {'high', 'standard', 'low', 'vlow'})
                   or ((r == 'default') and {'standard', 'low', 'vlow', 'high'})
                   or {r}

    -- Pick the first acceptable format available
    local format
    for _, cur_format in ipairs(preferred) do
        if available_formats[fmt_id_lookup[cur_format]] then
            format = cur_format
            break
        end
    end
    if not format then error('format not available') end

    -- Iterate over <media/>s
    local media
    for section in config:gmatch('<media .-</media>') do
        if section:find('service="' .. fmt_id_lookup[format] .. '"') then
            media = section
            break
        end
    end
    if not media then error("Couldn't parse the config") end

    self.url = {}

    -- Initialise with the default values from the media
    local mparams = {}
    for _,mparam in pairs{'kind', 'service'} do
        _,_,mparams[mparam] = media:find(mparam .. '="(.-)"')
        -- print ("MEDIA: mparams[" .. mparam .. "] = " .. mparams[mparam])
    end

    for connection in media:gmatch('<connection .-/>') do
        local params, complete_uri = {}, ''

        for _,param in pairs{'supplier', 'server', 'application', 'identifier', 'authString', 'kind'} do
            _,_,params[param] = connection:find(param .. '="(.-)"')
            -- print ("CONNECTION: params[" .. param .. "] = " .. (params[param] or "(null)"))
        end

        -- Get authstring from more specific mediaselector if
        -- this mode is specified - fails sometimes otherwise
	if needs_new_authString(params) then
	    local xml_url
	    xml_uri =
	        'http://www.bbc.co.uk/mediaselector/4/mtis/stream/' ..
		media_id .. '/' .. mparams['service'] .. '/' .. params['kind'] ..
		"?cb=" .. math.random(10000)
	    xml = quvi.fetch(xml_uri, {fetch_type = 'config'})
	    local _,_,new_authString = xml:find('authString="(.-)"')
	    if new_authString then
	        params['authString'] = new_authString:gsub('&amp;', '&')
	    end
        else
            -- Unescape the authString
            if params['authString'] then
                params['authString'] = params['authString']:gsub('&amp;', '&')
            end
        end

        -- in 'application', mp has a value containing one or more entries separated by strings.
        -- We only keep the first entry.
        if params.application then
            params.application = params.application:gsub("&mp=([^,&]+),?.-&", "&mp=%1&")
        end

        if params.supplier == 'akamai' then
            process_akamai(params)
        end

        if (params.supplier == 'limelight' or params.supplier == 'level3') then
            process_limelight_level3(params)
        end

        params.uri = params.uri or error('Could not create RTMP URL')

        complete_uri = params.uri
            .. ' app=' .. params.application
            .. ' playpath=' .. params.playpath
            .. ' swfUrl=http://www.bbc.co.uk/emp/revisions/18269_21576_10player.swf?revision=18269_21576 swfVfy=1'
            .. ' tcUrl=' .. params.tcurl
            .. ' pageurl=' .. self.page_url

        self.url[#(self.url) + 1] = complete_uri
    end

    return self
end
