
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Ross Burton <ross@burtonini.com>
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
    r.domain     = "city%.lego%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/.+/movies/.+$"})
      -- http://city.lego.com/en-gb/movies/mini-movies/gold-run/
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats  = "default"
    return self
end

-- Parse video URL.
function parse(self)
    self.host_id = "lego"

    local p = quvi.fetch(self.page_url)

    local d = p:match('FirstVideoData = {(.-)};')
                or error('no match: FirstVideoData')

    self.title = d:match('"Name":"(.-)"')
                  or error('no match: media title')

    self.id = d:match('"LikeObjectGuid":"(.-)"') -- Lack of a better.
                  or error('no match: media ID')

    self.url = {d:match('"VideoFlash":%{"Url":"(.-)"')
                  or error('no match: media stream URL')}

    -- TODO: return self.thumbnail_url

    return self
end
