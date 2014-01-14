
-- libquvi-scripts
-- Copyright (C) 2010-2013  Toni Gundogdu <legatvs@gmail.com>
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
-- NOTE: Vimeo is picky about the user-agent string.
--

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
    local c = Vimeo.get_config(self)
    local s = Vimeo.iter_streams(c)

    local t = {}
    for _,v in pairs(s) do
        table.insert(t, Vimeo.to_id(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = "vimeo"

    local c = Vimeo.get_config(self)
    local U = require 'quvi/util'

    local s = c:match('"title":"(.-)",') or error('no match: media title')
    self.title = U.slash_unescape(s)

    self.duration = U.json_get(c, 'duration', true) * 1000

    local t = c:match('"thumbs":({.-})')
    if t and #t >0 then
        local r = {}
        for u in t:gmatch('"%d+":"(.-)"') do
            table.insert(r,u)
        end
        self.thumbnail_url = r[#r] or ''
    end

    local c = U.choose_format(self, Vimeo.iter_streams(c),
                              Vimeo.choose_best, Vimeo.choose_default,
                              Vimeo.to_id)

    self.url = {c.url or error("no match: media stream URL")}
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

    local s = self.page_url:match('^(%w+)://') or 'http'
    local t = {s, '://player.vimeo.com/video/', self.id}

    local c = quvi.fetch(table.concat(t), {fetch_type='config'})

    if c:match('<error>') then
        local s = c:match('<message>(.-)[\n<]')
        error( (not s) and "no match: error message" or s )
    end

    return c:match('b=(.-);') or error('no match: b')
end

function Vimeo.iter_streams(c)
      local p =
          '"(%w+)":{"profile".-"url":"(.-)","height":(%d+).-"bitrate":(%d+)'
      local r = {}
      for q,url,h,br in c:gmatch(p) do
          local t = {
              bitrate = tonumber(br) or 0,
              height  = tonumber(h) or 0,
              quality = q,
              url = url
          }
          t.id = Vimeo.to_id(t)
          table.insert(r,t)
      end
      return r
end

function Vimeo.to_id(t)
    return string.format("%s_%dk_%dp", t.quality, t.height, t.bitrate)
end

function Vimeo.choose_best(t)
    local r = t[1]
    for _,v in pairs(t) do
        if v.height > r.height then
            r = v
        end
    end
    return r
end

function Vimeo.choose_default(t)
    return Vimeo.choose_best(t)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
