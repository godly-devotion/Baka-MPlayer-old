
-- libquvi-scripts v0.4.10
-- Copyright (C) 2012  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Bastien Nocera <hadess@hadess.net>
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

local Ted = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "ted%.com"
    r.formats    = "default|best"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/talks/.+$"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats  = "default"
    Ted.is_external(self, quvi.fetch(self.page_url))
    return self
end

-- Parse video URL.
function parse(self)
    self.host_id = "ted"
    local p = quvi.fetch(self.page_url)

    if Ted.is_external(self, p) then
        return self
    end

    self.id = p:match('ti:"(%d+)"')
                or error("no match: media ID")

    self.title = p:match('<title>(.-)%s+|')
                    or error("no match: media title")

    self.thumbnail_url = p:match('rel="image_src" href="(.-)"') or ''

    self.url = {p:match('(http://download.-)"')
                  or error("no match: media stream URL")}
    return self
end

--
-- Utility functions
--

function Ted.is_external(self, page)
    -- Some of the videos are hosted elsewhere.
    self.redirect_url = page:match('name="movie"%s+value="(.-)"') or ''
    return #self.redirect_url > 0
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
