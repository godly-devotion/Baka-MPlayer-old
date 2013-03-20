
-- libquvi-scripts
-- Copyright (C) 2011-2012  Toni Gundogdu <legatvs@gmail.com>
--
-- This file is part of libquvi-scripts <http://quvi.googlecode.com/>.
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

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "francetelevisions%.fr"
    r.formats    = "default"
    r.categories = C.proto_mms
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, nil, {"id%-video"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = 'francetelevisions'

    self.id = self.page_url:match('id%-video=([%w_]+)')
                or error('no match: media ID')

    local c_url =
      "http://info.francetelevisions.fr/video-info/player_html/blochtml.php?"
        .. "id-video="
        .. self.id

    local c = quvi.fetch(c_url, {fetch_type='config'})

    self.title = c:match('itemTitle">(.-)<')
                  or error('no match: title')

    self.url = {c:match('embed src="(.-)"')
                  or error('no match: media URL')}

    return self

end

-- vim: set ts=4 sw=4 tw=72 expandtab:
