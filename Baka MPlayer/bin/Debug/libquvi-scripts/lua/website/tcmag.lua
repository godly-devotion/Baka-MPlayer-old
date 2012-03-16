
-- libquvi-scripts
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
    local page = quvi.fetch(self.page_url)

    local article = page:match('<article id="article">(.-)</article>')
                        or error("no match: article")

    local s = article:match('http://.*youtube.com/embed/([^/]-)"')
    if s then -- Hand over to youtube.lua
        self.redirect_url = 'http://youtube.com/watch?v=' .. s
        return self
    end

    local s = article:match('http://.*vimeo.com/video/([0-9]+)')
    if s then -- Hand over to vimeo.lua
        self.redirect_url = 'http://vimeo.com/video/' .. s
        return self
    end

    local s = article:match('http://.*liveleak.com/e/([%w_]+)')
    if s then -- Hand over to liveleak.lua
        self.redirect_url = 'http://liveleak.com/e/' .. s
        return self
    end

    self.title = article:match('<h1>(.-)</h1>')
                    or error ("no match: media title")

    self.url = {article:match("'file': '(.-)',")
                or error("no match: media url")}

    self.id = self.url[1]:match("/%d+/%d+/(.-)/sizes/")
                or error ("no match: media id")

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
