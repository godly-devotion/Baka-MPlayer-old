
-- libquvi-scripts v0.4.10
-- Copyright (C) 2012  Tzafrir Cohen <tzafrir@cohens.org.il>
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
    --- http://flix.tapuz.co.il/v/watch-4158845-.html
    r.domain     = "flix%.tapuz%.co%.il"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain},
                       {"/v/watch-.*.html", "/showVideo%.asp"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = 'tapuz-flix'

    self.id = self.page_url:match('/v/watch%-(%d+)%-.*%.html')
    if not self.id then
        self.id = self.page_url:match('/showVideo%.asp%?m=(%d+)')
                    or error("no match: media ID")
    end

    local xml_url_base = 'v/Handlers/XmlForPlayer.ashx' -- Variable?
    local mako = 0 -- Does it matter?
    local playerOptions = '0|1|grey|large|0' -- Does it matter? Format?

    local p = quvi.fetch(self.page_url)
    self.title = p:match('<meta name="item%-title" content="([^"]*)" />')

    local s_fmt =
      'http://flix.tapuz.co.il/%s?mediaid=%d&autoplay=0&mako=%d'
      .. '&playerOptions=%s'

    local xml_url =
      string.format(s_fmt, xml_url_base, self.id, mako, playerOptions)

    local xml_page = quvi.fetch(xml_url)
    self.url = { xml_page:match('<videoUrl>.*(http://.*%.flv).*</videoUrl>') }

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
