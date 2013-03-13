--
-- libquvi-scripts v0.4.10
-- Copyright (C) 2011  quvi project
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

local IMDB = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = 'imdb%.com'
    r.formats    = 'default|best'
    local B      = require 'quvi/bit'
    r.categories = B.bit_or(C.proto_http, C.proto_rtmp)
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url,
            {r.domain},
            {"/video/.+"})
    return r
end

-- Query available formats.
function query_formats(self)
    local page    = quvi.fetch(self.page_url)
    local formats = IMDB.iter_formats(page)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, IMDB.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = 'imdb'

    self.id = self.page_url:match('/video/%w+/(vi%d-)/')

    local page = quvi.fetch(self.page_url)
    self.title = page:match('<title>(.-) %- IMDb</title>')

    --
    -- Format codes (for most videos):
    -- uff=1 - 480p RTMP
    -- uff=2 - 480p HTTP
    -- uff=3 - 720p HTTP
    --
    local formats = IMDB.iter_formats(page)
    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                    IMDB.choose_best,
                                    IMDB.choose_default,
                                    IMDB.to_s)
                        or error("unable to choose format")
    if not format.path then
        error("no match: media url")
    end

    local url = 'http://www.imdb.com' .. format.path
    local iframe = quvi.fetch(url, {fetch_type = 'config'})
    local file = iframe:match('so%.addVariable%("file", "(.-)"%);')
    file = U.unescape(file)

    if file:match('^http.-') then
        self.url = {file}
    else
        local path  = iframe:match('so%.addVariable%("id", "(.-)"%);')
        path = U.unescape(path)
        self.url = {file .. path}
    end

    -- Optional: we can live without these

    local thumb = iframe:match('so%.addVariable%("image", "(.-)"%);')
    self.thumbnail_url = thumb or ''

    return self
end

--
-- Utility functions
--

function IMDB.iter_formats(page)
    local p = "case '(.-)' :.-url = '(.-)';"
    local t = {}
    for c,u in page:gmatch(p) do
        table.insert(t, {path=u, container=c})
        --print(u,c)
    end

    -- First item is ad, so remove it
    table.remove(t, 1)

    return t
end

function IMDB.choose_best(t) -- Expect the last to be the 'best'
  return t[#t]
end

function IMDB.choose_default(t) -- Expect the second to be the 'default'
  return t[2]
end

function IMDB.to_s(t)
  return t.container
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
