
-- libquvi-scripts
-- Copyright (C) 2011-2013  quvi project
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

local Spankwire = {}

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "spankwire%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/.-/video%d+"})
    return r
end

-- Query available formats.
function query_formats(self)
    local p = quvi.fetch(self.page_url)
    local U = require 'quvi/util'

    local r = {}
    for _,v in pairs(Spankwire.iter_streams(U,p)) do
        table.insert(r, v.id)
    end

    table.sort(r)
    self.formats = table.concat(r,'|')

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = 'spankwire'

    local p = quvi.fetch(self.page_url)

    self.thumbnail_url = p:match('image_url%s+=%s+"(.-)"') or ''

    self.title = p:match('<title>(.-)%s+%-%s+Sp')
                    or error('no match: media title')

    self.id = self.page_url:match('/video(%d+)/$')
                  or error('no match: media ID')

    local U = require 'quvi/util'

    local c = U.choose_format(self, Spankwire.iter_streams(U,p),
                              Spankwire.choose_best, Spankwire.choose_default,
                              Spankwire.to_id)

    self.url = {c.url or error('no match: media stream URL')}

    return self
end

--
-- Utility functions
--

function Spankwire.iter_streams(U, p)
    local r = {}
    for id,u in p:gmatch('flashvars%.(quality_%d+p)%s+=%s"(.-)"') do
        if #u >0 then
            table.insert(r, {url=U.unescape(u),id=id})
        end
    end
    return r
end

function Spankwire.choose_best(t)
    return t[#t]
end

function Spankwire.choose_default(t)
    return Spankwire.choose_best(t)
end

function Spankwire.to_id(t)
    return t.id
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
