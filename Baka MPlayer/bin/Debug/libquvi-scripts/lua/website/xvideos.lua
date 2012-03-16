
-- libquvi-scripts
-- Copyright (C) 2010  quvi project
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

local Xvideos = {} -- Utility functions unique to this script

-- Identify the script.
function ident (self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "xvideos%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    Xvideos.normalize(self)
    r.handles    = U.handles(self.page_url, {r.domain}, {"/video%d+"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse (self)
    self.host_id = "xvideos"
    Xvideos.normalize(self)
    local page   = quvi.fetch(self.page_url)

    local _,_,s = page:find("<title>(.-)%s+-%s+XVID")
    self.title  = s or error ("no match: media title")

    local _,_,s = page:find("id_video=(.-)&amp;")
    self.id     = s or error ("no match: media id")

    local _,_,s = page:find("url_bigthumb=(.-)&amp;")
    self.thumbnail_url = s or ''

    local _,_,s = page:find("flv_url=(.-)&amp;")
    s           = s or error ("no match: flv url")
    local U     = require 'quvi/util'
    self.url    = {U.unescape(s)}

    return self
end

--
-- Utility functions
--

function Xvideos.normalize(self) -- "Normalize" embedded URL
    if not self.page_url then return end
    -- http://flashservice.xvideos.com/embedframe/ID to
    -- http://www.xvideos.com/videoID/
    local url = self.page_url
    url = url:gsub("flashservice.xvideos.com", "www.xvideos.com")
    url = url:gsub("/embedframe/", "/video")
    self.page_url = url
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
