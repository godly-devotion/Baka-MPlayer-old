
-- libquvi-scripts
-- Copyright (C) 2011,2013  Toni Gundogdu <legatvs@gmail.com>
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

local Guardian = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "theguardian%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/video/","/audio/"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "guardian"

    local p = Guardian.fetch(self)

    self.title = p:match('"og:title" content="(.-)"')
                    or error('no match: media title')
    self.title = self.title:gsub('%s+%-%s+video', '')

    self.id = (p:match('prop8%s+=%s+["\'](.-)["\']') or '')
                  :match('(%d+)') or error('no match: media ID')

    self.duration = tonumber(p:match('duration%:%s+"?(%d+)"?') or 0) * 1000
    if self.duration ==0 then
        local m,s = p:match('T(%d+)M(%d+)S')
        self.duration = (tonumber(m or 0)*60 + tonumber(s or 0)) * 1000
    end

    self.thumbnail_url = p:match('"thumbnail" content="(.-)"')
                            or p:match('"og:image" content="(.-)"') or ''

    self.url = {p:match("%s+file.-:%s+'(.-)'")
                  or error('no match: media stream URL')}

    return self
end

--
-- Utility functions
--

function Guardian.fetch(self)
    local p = quvi.fetch(self.page_url)
    local e = p:match('<div class="expired">.-<p>(.-)</p>.-</div>') or ''
    if #e >0 then error(e) end
    return p
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
