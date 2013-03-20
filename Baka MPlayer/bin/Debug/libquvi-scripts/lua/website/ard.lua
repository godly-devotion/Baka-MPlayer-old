-- libquvi-scripts
-- Copyright (C) 2013  Thomas Weißschuh
--
-- This file is part of libquvi-scripts <http://quvi.sourceforge.net/>.
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

local Ard = {}

function ident(self)
    local C      = require 'quvi/const'
    local U      = require 'quvi/util'
    local B      = require 'quvi/bit'
    local r      = {}
    r.domain     = 'www%.ardmediathek%.de'
    r.formats    = 'default|best'
    r.categories = B.bit_or(C.proto_http, C.proto_rtmp)
    r.handles    = U.handles(self.page_url, {r.domain},
                               nil, {"documentId=%d+$"})
    return r
end

function query_formats(self)
    local config = Ard.get_config(self)
    local formats = Ard.iter_formats(config)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, Ard.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

function parse(self)

    local config = Ard.get_config(self)
    local Util  = require 'quvi/util'

    self.host_id = 'ard'
    self.title = config:match(
                     '<meta property="og:title" content="([^"]*)'
                 ):gsub(
                    '%s*%- %w-$', '' -- remove name of station
                 ):gsub(
                    '%s*%(FSK.*', '' -- remove FSK nonsense
                 )
                 or error('no match: media title')
    self.thumbnail_url = config:match(
                             '<meta property="og:image" content="([^"]*)'
                         ) or ''

    local formats = Ard.iter_formats(config)
    local format  = Util.choose_format(self,
                                       formats,
                                       Ard.choose_best,
                                       Ard.choose_default,
                                       Ard.to_s)
                    or error('unable to choose format')

    if not format.url then error('no match: media url') end
    self.url = { format.url }

    return self
end

function Ard.test_availability(page)
    -- some videos are only scrapable at certain times
    local fsk_pattern =
        'Der Clip ist deshalb nur von (%d%d?) bis (%d%d?) Uhr verfügbar'
    local from, to = page:match(fsk_pattern)
    if from and to then
        error('video only available from ' ..from.. ':00 to '
              ..to.. ':00 CET')
    end
end

function Ard.get_config(self)
    local c = quvi.fetch(self.page_url)
    self.id = self.page_url:match('documentId=(%d*)')
              or error('no match: media id')
    if c:match('<title>ARD Mediathek %- Fehlerseite</title>') then
        error('invalid URL, maybe the media is no longer available')
    end

    return c
end

function Ard.choose_best(t)
    return t[#t] -- return the last from the array
end

function Ard.choose_default(t)
    return t[1] -- return the first from the array
end

function Ard.to_s(t)
    return string.format("%s_%s_i%02d%s%s",
              (t.quality) and t.quality or 'sd',
              t.container, t.stream_id,
              (t.encoding) and '_'..t.encoding or '',
              (t.height) and '_'..t.height or '')
end

function Ard.quality_from(suffix)
    local q = suffix:match('%.web(%w)%.') or suffix:match('%.(%w)%.')
                or suffix:match('[=%.]Web%-(%w)') -- .webs. or Web-S or .s
    if q then
        q = q:lower()
        local t = {s='ld', m='md', l='sd', xl='hd'}
        for k,v in pairs(t) do
            if q == k then return v end
        end
    end
    return q
end

function Ard.height_from(suffix)
    local h = suffix:match('_%d+x(%d+)[_%.]')
    if h then return h..'p' end
end

function Ard.container_from(suffix)
    return suffix:match('^(...):') or suffix:match('%.(...)$') or 'mp4'
end

function Ard.iter_formats(page)
    local r = {}
    local s = 'mediaCollection%.addMediaStream'
                .. '%(0, (%d+), "(.-)", "(.-)", "%w+"%);'

    Ard.test_availability(page)

    for s_id, prefix, suffix in  page:gmatch(s) do
        local u = prefix .. suffix
        u = u:match('^(.-)?') or u  -- remove querystring
        local t = {
            container = Ard.container_from(suffix),
            encoding = suffix:match('%.(h264)%.'),
            quality = Ard.quality_from(suffix),
            height = Ard.height_from(suffix),
            stream_id = s_id, -- internally (by service) used stream ID
            url = u
        }
        table.insert(r,t)
    end
    if #r == 0 then error('no media urls found') end
    return r
end

-- vim: set ts=4 sw=4 sts=4 tw=72 expandtab:
