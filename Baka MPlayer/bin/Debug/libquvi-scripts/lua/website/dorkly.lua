
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
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

local Dorkly = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "dorkly%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain},
                    {"/video/%d+/", "/embed/%d+/"})
    return r
end

-- Query formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = "dorkly"

    self.id = self.page_url:match('/video/(%d+)/')
                  or self.page_url:match('/embed/(%d+)/')
                      or error('no match: media ID')

    if Dorkly.is_affiliate(self) then
      return self
    end

    local t = {'http://www.dorkly.com/moogaloop/video/', self.id}
    local x = quvi.fetch(table.concat(t), {fetch_type='config'})

    local U = require 'quvi/util'

    self.duration = tonumber(U.xml_get(x, 'duration', false))

    self.thumbnail_url = U.xml_get(x, 'thumbnail', true)

    self.title = U.xml_get(x, 'caption', true)

    self.url = { U.xml_get(x, 'file', true) }

    return self
end

--
-- Utility functions
--

function Dorkly.is_affiliate(self)
    if not self.page_url:match('/embed/') then
        return false
    end
    local p = quvi.fetch(self.page_url)
    local u = p:match('iframe.-src="(.-)"') or error('no match: iframe: src')
    if not u:match('^http%:') then    -- If the URL scheme is malformed...
        u = table.concat({'http:',u}) -- ... Try to fix it.
    end
    self.redirect_url = u
    return true
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
