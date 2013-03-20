
-- libquvi-scripts
-- Copyright (C) 2012  quvi project
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

local Deviantclip = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "deviantclip%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/watch/.+"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "deviantclip"

    self.id = self.page_url:match('/watch/(.+)')
                or error("no match: media ID")

    local p = quvi.fetch(self.page_url)

    self.title = p:match('DC%.title" content="(.-)" ')
                  or error("no match: media title")
    self.title = self.title:gsub('&#x(%d+);', Deviantclip.to_utf8)

    local U = require 'quvi/util'
    self.url = {U.unescape (p:match('%[{"file":"(.-)",'))
                  or error("no match: media stream URL")}

    return self
end

--
-- Utility functions
--

function Deviantclip.to_utf8(a) -- Unescape &#x00; to UTF-8
    local H = require 'quvi/html'
    return H.to_utf8("0x" .. a)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
