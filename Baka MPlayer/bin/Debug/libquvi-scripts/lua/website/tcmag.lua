
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  quvi project
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

local TCMag = {} -- Utility functions specific to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "tcmag%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/magazine/.+"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return TCMag.check_external_content(self)
end

-- Parse media URL.
function parse(self)
    self.host_id = "tcmag"
    return TCMag.check_external_content(self)
end

--
-- Utility functions
--

function TCMag.check_external_content(self)
    local p = quvi.fetch(self.page_url)

    self.url = {p:match("file: '(.-)'")
                  or error("no match: media stream URL")}

    if self.url[1]:match('%.%w+$') then
        -- Self-hosted content.
        self.title = p:match('<h1>(.-)</h1>')
                        or error ("no match: media title")
        self.id = self.url[1]:match("/%d+/%d+/(.-)/sizes/")
                        or error ("no match: media id")
        self.thumbnail_url = p:match('"og:image" content="(.-)"') or ''
    else -- Affiliate content.
        self.redirect_url = self.url[1]
        return self
    end

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
