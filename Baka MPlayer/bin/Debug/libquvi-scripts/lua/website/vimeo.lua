
-- libquvi-scripts
-- Copyright (C) 2010-2012  Toni Gundogdu <legatvs@gmail.com>
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

-- w/ HD: <http://vimeo.com/1485507>
-- no HD: <http://vimeo.com/10772672>

local Vimeo = {} -- Utility functions unique to this script.

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "vimeo%.com"
    r.formats    = "default|best"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/%d+$"})
    return r
end

-- Query available formats.
function query_formats(self)
    local config  = Vimeo.get_config(self)
    local formats = Vimeo.iter_formats(self, config)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, Vimeo.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = "vimeo"

    local c = Vimeo.get_config(self)

    self.title = c:match("<caption>(.-)</")
                  or error("no match: media title")

    self.duration = (tonumber(c:match('<duration>(%d+)')) or 0) * 1000

    self.thumbnail_url = c:match('<thumbnail>(.-)<') or ''

    local formats = Vimeo.iter_formats(self, c)
    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                     Vimeo.choose_best,
                                     Vimeo.choose_default,
                                     Vimeo.to_s)
                        or error("unable to choose format")
    self.url      = {format.url or error("no match: media URL")}
    return self
end

--
-- Utility functions
--

function Vimeo.normalize(url)
    url = url:gsub("player.", "") -- player.vimeo.com
    url = url:gsub("/video/", "/") -- player.vimeo.com
    return url
end

function Vimeo.get_config(self)
    self.page_url = Vimeo.normalize(self.page_url)

    self.id = self.page_url:match('vimeo.com/(%d+)')
                or error("no match: media ID")

    local c_url = "http://vimeo.com/moogaloop/load/clip:" .. self.id
    local c = quvi.fetch(c_url, {fetch_type='config'})

    if c:match('<error>') then
        local s = c:match('<message>(.-)[\n<]')
        error( (not s) and "no match: error message" or s )
    end

    return c
end

function Vimeo.iter_formats(self, config)
    local isHD  = tonumber(config:match('<isHD>(%d+)')) or 0

    local t = {}
    Vimeo.add_format(self, config, t, 'sd')
    if isHD == 1 then
        Vimeo.add_format(self, config, t, 'hd')
    end

    return t
end

function Vimeo.add_format(self, config, t, quality)
    table.insert(t,
        {quality=quality,
         url=Vimeo.to_url(self, config, quality)})
end

function Vimeo.choose_best(formats) -- Last is 'best'
    local r
    for _,v in pairs(formats) do r = v end
    return r
end

function Vimeo.choose_default(formats) -- First is 'default'
    for _,v in pairs(formats) do return v end
end

function Vimeo.to_url(self, config, quality)
    local sign = config:match("<request_signature>(.-)</")
                  or error("no match: request signature")

    local exp = config:match("<request_signature_expires>(.-)</")
                  or error("no match: request signature expires")

    local fmt_s = "http://vimeo.com/moogaloop/play/clip:%s/%s/%s/?q=%s"

    return string.format(fmt_s, self.id, sign, exp, quality)
end

function Vimeo.to_s(t)
    return string.format("%s", t.quality)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
