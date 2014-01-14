
-- libquvi-scripts
-- Copyright (C) 2012,2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010-2011  Lionel Elie Mamane <lionel@mamane.lu>
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

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "collegehumor%.com"
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
    self.host_id  = "collegehumor"

    local p = quvi.fetch(self.page_url)

    local u = p:match('source type=.- src="(.-)"')
                  or error('no match: media stream URL')

    if u:match('%.%w+$') then
        self.url = {u}
    else
        self.redirect_url = u -- Affiliate content.
        return self           -- Pass the new page URL back to the library.
    end

    self.duration =
      tonumber(p:match('"video:duration" content="(%d+)"') or 0) *1000

    self.thumbnail_url = p:match('"og:image" content="(.-)"') or ''

    self.title = p:match('"og:title" content="(.-)"')
                    or error('no match: media title')

    self.id = self.page_url:match('/video/(%d+)/')
                  or self.page_url:match('/embed/(%d+)/')
                      or error('no match: media ID')
    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
