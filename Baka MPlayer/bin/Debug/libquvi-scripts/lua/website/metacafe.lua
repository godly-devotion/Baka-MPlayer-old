
-- libquvi-scripts v0.4.10
-- Copyright (C) 2011  Lionel Elie Mamane <lionel@mamane.lu>
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

local Metacafe = {} -- Utility functions unique to this script.

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "metacafe%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain},
		     {"/watch/%d+/", "/watch/yt-[^/]+/"})
    return r
end

-- Query available formats.
function query_formats(self)
    if not Metacafe.redirectp(self) then
        self.formats = 'default'
    end
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "metacafe"

    if Metacafe.redirectp(self) then
        return self
    end

    local U = require 'quvi/util'
    local p = Metacafe.fetch_page(self, U)

    self.title = p:match('"title":"(.-)"')
                  or error("no match: media title")

    self.title = U.unescape(self.title)

    self.id = p:match('"itemID":"(.-)"')
                or error('no match: media id')

    self.thumbnail_url = p:match('rel="image_src" href="(.-)"') or ''

    local d = p:match('"mediaData":"(.-)"')
                or error('no match: media data')
    d = U.unescape(d)

    local u = d:match('"mediaURL":"(.-)"')
                or error('no match: media url')
    u = U.slash_unescape(u)

    local k = d:match('"key":"(.-)"')
                or error('no match: gda key')

    self.url = {string.format("%s?__gda__=%s", u, k)}

    return self
end

--
-- Utility functions
--

function Metacafe.redirectp(self)
    local s = self.page_url:match('/watch/yt%-([^/]+)/')
    if s then -- Hand over to youtube.lua
        self.redirect_url = 'http://youtube.com/watch?v=' .. s
        return true
    end
    return false
end

function Metacafe.fetch_page(self, U)
    self.page_url = Metacafe.normalize(self.page_url)

    return quvi.fetch(self.page_url)
end

function Metacafe.normalize(page_url) -- "Normalize" embedded URLs
    return page_url
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
