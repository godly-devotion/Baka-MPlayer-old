
-- libquvi-scripts
-- Copyright (C) 2010-2011,2013  Toni Gundogdu <legatvs@gmail.com>
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
--

--
-- NOTE: mp4s do not appear to be available (404) even if listed.
--

local Spiegel = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "spiegel%.de"
    r.formats    = "default|best"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/video/"})
    return r
end

-- Query available formats.
function query_formats(self)
    Spiegel.get_media_id(self)

    local config  = Spiegel.get_config(self)
    local formats = Spiegel.iter_formats(config)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, Spiegel.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "¦")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "spiegel"

    local p = quvi.fetch(self.page_url)

    self.title = p:match('"module%-title">(.-)</')
                    or p:match('"og:title".-content="(.-)%s+%-%s+SP')
                      or error('no match: media title')

    self.thumbnail_url = p:match('"og:image" content="(.-)"') or ''

    Spiegel.get_media_id(self)

    local config  = Spiegel.get_config(self)
    local formats = Spiegel.iter_formats(config)

    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                    Spiegel.choose_best,
                                    Spiegel.choose_default,
                                    Spiegel.to_s)
                        or error("unable to choose format")
    self.duration = (format.duration or 0) * 1000 -- to msec
    self.url      = {format.url or error("no match: media url")}

    return self
end

--
-- Utility functions
--

function Spiegel.get_media_id(self)
    self.id = self.page_url:match("/video/.-video%-(.-)%.")
                or error ("no match: media id")
end

function Spiegel.get_config(self)
    local fmt_s      = "http://video.spiegel.de/flash/%s.xml"
    local config_url = string.format(fmt_s, self.id)
    return quvi.fetch(config_url, {fetch_type = 'config'})
end

function Spiegel.iter_formats(config)
    local p = '<filename>(.-)<'
           .. '.-<codec>(.-)<'
           .. '.-<totalbitrate>(%d+)'
           .. '.-<width>(%d+)'
           .. '.-<height>(%d+)'
           .. '.-<duration>(%d+)'
    local t = {}
    for fn,c,b,w,h,d in config:gmatch(p) do
        local cn = fn:match('%.(%w+)$') or error('no match: container')
        local u = 'http://video.spiegel.de/flash/' .. fn
--        print(u,c,b,w,h,cn,d)
        table.insert(t, {codec=string.lower(c), url=u,
                         width=tonumber(w),     height=tonumber(h),
                         bitrate=tonumber(b),   duration=tonumber(d),
                         container=cn})
    end
    return t
end

function Spiegel.choose_best(formats) -- Highest quality available
    local r = {width=0, height=0, bitrate=0, url=nil}
    local U = require 'quvi/util'
    for _,v in pairs(formats) do
        if U.is_higher_quality(v,r) then
            r = v
        end
    end
    return r
end

function Spiegel.choose_default(formats)
    return formats[1]
end

function Spiegel.to_s(t)
    return string.format('%s_%s_%sk_%sp',
        t.container, t.codec, t.bitrate, t.height)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
