
-- libquvi-scripts
-- Copyright (C) 2013 Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012 Paul Kocialkowski <contact@paulk.fr>
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

local CanalPlus = {} -- Utility functions unique to to this script.

-- Identify the script.
function ident(self)
    local domains= {"canalplus%.fr", "d17%.tv", "d8%.tv"}
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local U      = require 'quvi/util'
    local B      = require 'quvi/bit'
    local r      = {
        handles    = U.handles(self.page_url, domains, {"/pid%d+"}),
        categories = B.bit_or(C.proto_http, C.proto_rtmp),
        domain     = table.concat(domains, '|'),
        formats    = "default|best"
    }
    return r
end

-- Query available formats.
function query_formats(self)
    local r = CanalPlus.rest_new(self)
    local U = require 'quvi/util'

    local s = CanalPlus.iter_streams(U, r)
    local r = {}

    for _,v in pairs(s) do
        table.insert(r, CanalPlus.to_s(v))
    end

    table.sort(r)
    self.formats = table.concat(r, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = 'canalplus'

    local r = CanalPlus.rest_new(self)
    local U = require 'quvi/util'

    self.thumbnail_url = U.xml_get(r, 'PETIT', false)
    self.title         = U.xml_get(r, 'TITRE', true)
    self.id            = U.xml_get(r, 'ID', false)

    local s = CanalPlus.iter_streams(U, r)

    local c = U.choose_format(self, s, CanalPlus.choose_best,
                              CanalPlus.choose_default, CanalPlus.to_s)

    self.url = {c.url or error("no match: media stream URL")}

    return self
end

--
-- Utility functions
--

function CanalPlus.rest_new(self)
    self.id = self.page_url:match('vid=(%d+)')
                  or error('no match: media ID')

    local c = self.page_url:match('http://www%.(%w+)%.%w+/')
                  or error('unable to determine the second-level domain')

    c = c:gsub('canalplus', 'cplus')

    local t = {
        'http://service.canal-plus.com/video/rest/getVideos/',
        c, '/', self.id, '/?format=xml'
    }

    return quvi.fetch(table.concat(t), {fetch_type = 'config'})
end

function CanalPlus.iter_streams(U, r)
    local m = U.xml_get(r, 'MEDIA')
    local v = U.xml_get(m, 'VIDEOS')
    local r = {}
    for id,uri in v:gmatch('<(%w+)>(.-)</%1>') do
        table.insert(r, {quality=id:lower(), url=uri})
    end
    if #r ==0 then
        error('failed to find any media stream URLs')
    end
    return r
end

function CanalPlus.choose_default(t)
    return t[1]
end

function CanalPlus.choose_best(t)
    return CanalPlus.choose_default(t)
end

function CanalPlus.to_s(t)
    return t.quality
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
