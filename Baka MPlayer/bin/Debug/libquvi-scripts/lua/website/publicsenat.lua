
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010,2012 Raphaël Droz.
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
    r.domain     = "publicsenat%.fr"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/vod/.-/%d+$"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "publicsenat"

    self.id = self.page_url:match("/vod/.-/(%d+)$")
                  or error('no match: media ID')

    local p = quvi.fetch(self.page_url)

    self.title = p:match('<title>(.-)%s+%|')
                    or error("no match: media title")

    local u = p:match('id="dmcloudUrlEmissionSelect" value="(.-)"')
                  or error('no match: emission URL')

    local e = quvi.fetch(u, {fetch_type='config'})

    local d = e:match('info = (.-);')
                  or error('no match: info')

    self.thumbnail_url = d:match('"thumbnail_url": "(.-)"') or ''

    self.url = {d:match('"mp4_url": "(.-)"')
                    or error('no match: media stream URL')}

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
