
-- libquvi-scripts
-- Copyright (C) 2010-2012,2013  Toni Gundogdu <legatvs@gmail.com>
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

local CBSNews = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local U      = require 'quvi/util'
    local B      = require 'quvi/bit'
    local d      = 'cbsnews%.com'
    local r      = {
        handles    = U.handles(self.page_url, {d}, {"/videos/.-/?"}),
        categories = B.bit_or(C.proto_http, C.proto_rtmp),
        formats   = 'default',
        domain    = d
    }
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "cbsnews"

    local p = quvi.fetch(self.page_url)

    local d = p:match("data%-cbsvideoui%-options='(.-)'")
                  or error('no match: player options')

    local U = require 'quvi/util'

    self.thumbnail_url =
        U.slash_unescape(d:match('"image":{"path":"(.-)"') or '')

    self.title = d:match('"title":"(.-)"')
                    or error('no match: media ID')

    self.id = d:match('"id":"(.-)"')
                  or error('no match: media ID')

    local u = d:match('"desktop":.-"uri":"(.-)"')
                  or error('no match: media stream URL')

    self.url = {U.slash_unescape(u)}

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
