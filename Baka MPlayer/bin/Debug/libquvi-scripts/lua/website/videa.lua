
-- libquvi-scripts v0.4.10
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

local Videa = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "videa%.hu"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url,
                    {r.domain}, {"/videok/.+/.+-.+$", "/flvplayer.swf"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "videa"
    Videa.normalize(self)

    local p = quvi.fetch(self.page_url)

    self.id = p:match("v=(%w+)")
                or error("no match: media id")

    self.title = p:match('"og:title"%s+content="(.-)"')
                    or error("no match: media title")

    local s  = p:match("%?f=(.-)&") or error("no match: f param")
    self.url = {'http://videa.hu/static/video/' .. s:gsub("%.%d+$","")}

    self.thumbnail_url = p:match('"og:image"%s+content="(.-)"') or ''

    return self
end

--
-- Utility functions
--

function Videa.normalize(self) -- "Normalize" an embedded URL
    local id = self.page_url:match('/flvplayer%.swf%?v=(.-)$')
    if not id then return end

    self.page_url = 'http://videa.hu/videok/' .. id
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
