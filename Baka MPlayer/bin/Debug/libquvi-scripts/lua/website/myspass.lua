
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Guido Leisker <guido@guido-leisker.de>
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

-- About
--  Each video url ends with an id composed of digits.
--  This id leads us to a metadata xml file (see function
--  MySpass.getMetadataValue) containing all necessary information
--  including download link.

local MySpass = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "myspass%.de"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    -- expect all urls ending with digits to be videos
    r.handles    = U.handles(self.page_url, {r.domain},{"/myspass/.-/%d+/?$"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "myspass"

    self.id = self.page_url:match("(%d+)/?$") or error("no match: media ID")

    local u = {
        'http://www.myspass.de/myspass/',
        'includes/apps/video/getvideometadataxml.php?id=',
        self.id
    }
    local m = quvi.fetch(table.concat(u,''))

    local format  = MySpass.getMetadataValue(m, 'format')
    MySpass.chk_expired(format)

    local season  = MySpass.getMetadataValue(m, 'season')
    local episode = MySpass.getMetadataValue(m, 'episode')
    local title   = MySpass.getMetadataValue(m, 'title')

    self.title = string.format("%s %03d %03d %s",
                    format,
                    tonumber(season),
                    tonumber(episode),
                    title)

    self.thumbnail_url = MySpass.getMetadataValue(m, 'imagePreview') or ''
    self.url = {MySpass.getMetadataValue(m, 'url_flv')}

    return self
end

--
-- Utility functions
--

function MySpass.getMetadataValue(m, k)
    local p = string.format("<%s>(.-)</%s>", k, k)
    local s = m:match(p) or error(string.format('no match: %s', k))
    return s:match('<!%[CDATA%[(.+)]]>') or ''
end

function MySpass.chk_expired(s)
    if #s ==0 then
        error('no match: metadata: "format": video no longer available?')
    end
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
