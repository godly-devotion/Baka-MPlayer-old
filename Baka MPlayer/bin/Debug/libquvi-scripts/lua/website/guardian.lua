
-- libquvi-scripts v0.4.10
-- Copyright (C) 2011  Toni Gundogdu <legatvs@gmail.com>
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
-- NOTE: Ignores the m3u8 format. Patches welcome.
--
--   libquvi allows specifying multiple media stream URLs in
--   "self.url" (referred sometimes as "media or video segments"),
--   e.g.
--      self.url = {"http://foo", "http://bar"}
--
--   Whether the applications using libquvi make any use of this,
--   is a whole different matter.
--

local Guardian = {} -- Utility functions unique to this script

-- Identify the script.
function ident (self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "guardian%.co%.uk"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url,
                    {r.domain}, {"/video/","/audio"})
    return r
end

-- Query available formats.
function query_formats(self)
    local c = Guardian.get_config(self)
    local fmts = Guardian.iter_formats(c)

    local t = {}
    for _,v in pairs(fmts) do
        table.insert(t, Guardian.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "guardian"

    local c = Guardian.get_config(self)

    local formats = Guardian.iter_formats(c)
    local U = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                    Guardian.choose_best,
                                    Guardian.choose_default,
                                    Guardian.to_s)
                        or error("unable to choose format")
    self.url = {format.url or error("no match: media url")}

    self.title = c:match('"headline":%s+"(.-)%s+-%s+video"')
                    or error("no match: media title")

    self.id = c:match('"video%-id":%s+"(.-)"')
                    or error ("no match: media id")

    self.thumbnail_url = c:match('"thumbnail%-image%-url":%s+"(.-)"') or ''

    local d = c:match('"duration":%s+(%d+)') or 0
    self.duration = tonumber(d)*1000 -- to msec

    return self
end

--
-- Utility functions
--

function Guardian.get_config(self)
    return quvi.fetch(self.page_url .. "/json", {fetch_type='config'})
end

function Guardian.iter_formats(config)
    local p = '"format":%s+"(.-)".-"video%-file%-url":%s+"(.-)"'
    local t = {}
    for c,u in config:gmatch(p) do
--        print(f,u)
        c = c:gsub("video/", "")
        c = c:gsub(":", "_")
        if c ~= "m3u8" then -- http://en.wikipedia.org/wiki/M3U
            table.insert(t, {container=c, url=u})
        end
    end
    return t
end

function Guardian.choose_best(t) -- Expect the first to be the 'best'
    return t[1]
end

function Guardian.choose_default(t) -- Use the first
  return t[1]
end

function Guardian.to_s(t)
    return t.container
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
