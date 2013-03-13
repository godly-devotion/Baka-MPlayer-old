
-- libquvi-scripts v0.4.10
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

    local s = c:match('"title":(.-),') or error("no match: media title")
    local U = require 'quvi/util'
    self.title = U.slash_unescape(s):gsub('^"',''):gsub('"$','')

    self.duration = (tonumber(c:match('"duration":(%d+)')) or 0) * 1000

    local s = c:match('"thumbnail":"(.-)"') or ''
    if #s >0 then
      self.thumbnail_url = U.slash_unescape(s)
    end

    local formats = Vimeo.iter_formats(self, c)
    local format  = U.choose_format(self, formats,
                                     Vimeo.choose_best,
                                     Vimeo.choose_default,
                                     Vimeo.to_s)
                        or error("unable to choose format")
    self.url      = {format.url or error("no match: media stream URL")}
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

    local c_url = "http://vimeo.com/" .. self.id
    local c = quvi.fetch(c_url, {fetch_type='config'})

    if c:match('<error>') then
        local s = c:match('<message>(.-)[\n<]')
        error( (not s) and "no match: error message" or s )
    end

    return c
end

function Vimeo.iter_formats(self, config)
    local t = {}
    local qualities = config:match('"qualities":%[(.-)%]')
                        or error('no match: qualities')
    for q in qualities:gmatch('"(.-)"') do
        Vimeo.add_format(self, config, t, q)
    end
    return t
end

function Vimeo.add_format(self, config, t, quality)
    table.insert(t, {quality=quality,
                     url=Vimeo.to_url(self, config, quality)})
end

function Vimeo.choose_best(t) -- First 'hd', then 'sd' and 'mobile' last.
    for _,v in pairs(t) do
        local f = Vimeo.to_s(v)
        for _,q in pairs({'hd','sd','mobile'}) do
            if f == q then return v end
        end
    end
    return Vimeo.choose_default(t)
end

function Vimeo.choose_default(t)
  for _,v in pairs(t) do
      if Vimeo.to_s(v) == 'sd' then return v end -- Default to 'sd'.
  end
  return t[1] -- Or whatever is the first.
end

function Vimeo.to_url(self, config, quality)
    local sign = config:match('"signature":"(.-)"')
                  or error("no match: request signature")

    local exp = config:match('"timestamp":(%d+)')
                  or error("no match: request timestamp")

    local s = "http://player.vimeo.com/play_redirect?clip_id=%s"
              .. "&sig=%s&time=%s&quality=%s&type=moogaloop_local"

    return string.format(s, self.id, sign, exp, quality)
end

function Vimeo.to_s(t)
    return string.format("%s", t.quality)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
