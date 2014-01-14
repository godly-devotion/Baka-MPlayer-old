
-- libquvi-scripts
-- Copyright (C) 2012,2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Raphaël Droz <raphael.droz+floss@gmail.com>
--
-- This file is part of libquvi-scripts <http://quvi.googlecode.com/>.
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

-- NOTE: Most videos expire some (7?) days after their original broadcast

local Arte = {} -- Utility functions unique to to this script.

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local U      = require 'quvi/util'
    local B      = require 'quvi/bit'
    local d      = 'www%.arte%.tv'
    return {
        handles    = U.handles(self.page_url, {d}, {"/guide/%w+/"}),
        categories = B.bit_or(C.proto_http, C.proto_rtmp),
        formats    = 'default|best',
        domain     = d
    }
end

-- Query available formats.
function query_formats(self)
    local c,U = Arte.get_config(self)
    local s = Arte.iter_streams(U, c)

    local t = {}
    for _,v in pairs(s) do
        table.insert(t, Arte.to_id(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = 'arte'

    local c,U = Arte.get_config(self)

    self.duration = U.json_get(c, 'videoDurationSeconds', true) * 1000

    self.thumbnail_url = U.json_get(c, 'programImage')

    self.title = U.json_get(c, 'VTI')

    self.id = U.json_get(c, 'VPI')

    local c = U.choose_format(self, Arte.iter_streams(U,c), Arte.choose_best,
                              Arte.choose_default, Arte.to_id)

    self.url = {c.url or error("no match: media stream URL")}

    return self
end

--
-- Utility functions
--

function Arte.iter_streams(U, c)
    local s = c:match('"VSR":(.-)$') or error('no match: VSR')
    local r = {}
    for id,p in s:gmatch('"(.-)":{(.-)}') do
        local m = U.json_get(p, 'streamer')
        local u = U.json_get(p, 'url')
        local g = (#m >0) and table.concat({m,'mp4:',u}) or u
        local t = {
            bitrate = U.json_get(p, 'bitrate', true),
            height = U.json_get(p, 'height', true),
            width = U.json_get(p, 'width', true),
            quality = U.json_get(p, 'quality'),
            -- Refer to 0.9+ script for the description of "versionProg".
            vprog = tonumber(U.json_get(p, 'versionProg')),
            vcode = U.json_get(p, 'versionCode'),
            mtype = U.json_get(p, 'mediaType'),
            url = g
        }
        table.insert(r,t)
    end
    return r
end

function Arte.get_config(self)
    local p = quvi.fetch(self.page_url)

    local u = p:match('arte_vp_url="(.-)">')
                  or error('no match: config URL')

    local c = quvi.fetch(u, {fetch_type='config'})
    local U = require 'quvi/util'

    local e = U.json_get(c, 'VRU')
    if #e >0 and Arte.has_expired(U, e) then
        error('media no longer available (expired)')
    end

    return c,U
end

function Arte.has_expired(U, s)
    local d, mo, y, h, m, sc =
              s:match('(%d+)/(%d+)/(%d+) (%d+):(%d+):(%d+)')

    local t = os.time({ year = y, month = mo, day = d,
                        hour = h, min = m, sec = sc })

    return (t - os.time()) < 0
end

function Arte.is_best(a,b)
    -- Select the default language version rather than the alternative.
    -- refer to 0.9+ script for versionProg description.
    if b.vprog ==2 and a.vprog ~= 2 then
        return true
    end
    -- Select the default language version rather than the original.
    if a.vprog < b.vprog then
        return true
    end
    -- Otherwise, compare the resolution and the bitrate properties.
    return a.height >b.height
                or (a.height ==b.height and a.bitrate >b.bitrate)
end

function Arte.choose_best(t)
    local r = t[1]
    for _,v in pairs(t) do
        if Arte.is_best(v, r) then
            r = v
        end
    end
    return r
end

function Arte.choose_default(t)
    for _,v in pairs(t) do
        if Arte.to_id(v):match('sd') and t.mtype == 'rtmp' then
            return v  -- Anything that matches 'SD'.
        end
    end
    return t[1]  -- Or whatever was returned as the first stream.
end

function Arte.to_id(t)
    local s = (#t.mtype ==0) and 'http' or t.mtype
    return string.format('%s_%s_%s', t.quality, s, t.vcode)
                          :gsub('%s?%-%s?', '_'):lower()
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
