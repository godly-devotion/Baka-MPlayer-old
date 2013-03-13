
-- libquvi-scripts v0.4.10
-- Copyright (C) 2012  quvi project <http://quvi.sourceforge.net/>
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
    package.path  = self.script_dir .. '/?.lua'
    local C       = require 'quvi/const'
    local r       = {}
    local domains = {"twitch%.tv", "justin%.tv"}
    r.domain      = table.concat(domains, "|")
    r.formats     = "default"
    r.categories  = C.proto_http
    local U       = require 'quvi/util'
    r.handles     = U.handles(self.page_url, domains, {"/[%w_]+/b/%d+"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "justintv"

    self.id = self.page_url:match("/[%w_]+/b/(%d+)")
                or error("no match: media ID")

    local c_url = "http://api.justin.tv/api/clip/show/"
                    .. self.id .. ".xml"

    local c = quvi.fetch(c_url, {fetch_type = 'config'})

    self.title = c:match('<title>(.-)</title>') or ''

    if #self.title ==0 then
        c = c:gsub("^%s*(.-)%s*$", "%1") -- 'c' may hold an error message.
        c = c:gsub("%s%s+", " ")         -- Sanitize.
        error(string.format("no match: media title (%s)",
                            (#c >0) and c or ''))
    end

    self.thumbnail_url =
      c:match('<image_url_medium>(.-)</image_url_medium>') or ''

    self.url = {c:match("<video_file_url>(.-)</video_file_url>")
                  or error("no match: media URL")}

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
