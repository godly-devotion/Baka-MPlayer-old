
-- libquvi-scripts v0.4.10
-- Copyright (C) 2012  Mikhail Gusarov <dottedmag@dottedmag.net>
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
    r.domain     = "tvrain%.ru"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/articles/"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "tvrain"

    self.id = self.page_url:match('%-(%d+)/$')
                or error("no match: media ID")

    local p = quvi.fetch(self.page_url)

    -- tvrain uses <iframe> for the video player which constructs URL to load a
    -- playlist. Luckily, we can obtain URL of the playlist from the URL of
    -- thumbnail image.

    local p1,p2 = p:match('<meta property="og:image" '
                          .. 'content="http://photo.tvigle.ru/res/'
                          .. 'prt/(.-)/../../0*(.-)/pub.jpg" />')
    if not p1 then
        error("no match: thumbnail URL")
    end

    local u = string.format('http://pub.tvigle.ru/xml/index.php?'
                            .. 'prt=%s&id=%s&mode=1', p1, p2)

    local pl = quvi.fetch(u, {fetch_type='playlist'})

    self.title = pl:match('<video[^>]-name="(.-)"')
                  or error("no match: media title")

    self.url = {pl:match('<video[^>]-videoLink="(.-)"')
                  or error("no match: media stream URL")}

    self.thumbnail_url = pl:match('<video[^>]-prw="(.-)"') or ''

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
