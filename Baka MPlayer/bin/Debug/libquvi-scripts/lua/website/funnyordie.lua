
-- libquvi-scripts
-- Copyright (C) 2011,2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010 quvi project
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

local FunnyOrDie = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "funnyordie%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/videos/"})
    return r
end

-- Query available formats.
function query_formats(self)
    local page    = quvi.fetch(self.page_url)
    local formats = FunnyOrDie.iter_formats(page)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, FunnyOrDie.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "funnyordie"
    local page   = quvi.fetch(self.page_url)

    self.title = page:match('"og:title" content="(.-)">')
                    or error ("no match: media title")

    self.id = page:match('key:%s+"(.-)"')
                or error ("no match: media ID")

    self.thumbnail_url = page:match('"og:image" content="(.-)"') or ''

    local formats = FunnyOrDie.iter_formats(page)
    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                     FunnyOrDie.choose_best,
                                     FunnyOrDie.choose_default,
                                     FunnyOrDie.to_s)
                        or error("unable to choose format")
    self.url      = {format.url or error('no match: media stream URL')}
    return self
end

--
-- Utility functions
--

function FunnyOrDie.iter_formats(page)
    local t = {}
    for u in page:gmatch('type: "video/mp4", src: "(.-)"') do
        table.insert(t, u)
    end
    for u in page:gmatch('source src="(.-)"') do table.insert(t,u) end
    if #t ==0 then error('no match: media stream URL') end
    local r = {}
    for _,u in pairs(t) do
        local q,c = u:match('/(%w+)%.(%w+)$')
        if q and c then
          table.insert(r, {url=u, quality=q, container=c})
        end
    end
    return r
end

function FunnyOrDie.choose_best(formats)
    return FunnyOrDie.choose_default(formats)
end

function FunnyOrDie.choose_default(formats)
    return formats[1]
end

function FunnyOrDie.to_s(t)
    return string.format("%s_%s", t.container, t.quality)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
