
-- libquvi-scripts v0.4.10
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
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "canalplus%.fr"
    r.formats    = "default|best"
    r.categories = C.proto_rtmp
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/pid%d",
                    -- presidentielle2012.canalplus.fr
                    "/emissions", "/candidats", "/debats",
                    -- canalstreet.canalplus.fr
                    "/musique", "/actu", "/humour", "/tendances", "/sport", "/arts", "/danse"})

    return r
end

-- Query available formats.
function query_formats(self)
    local config  = CanalPlus.get_config(self)

    if #self.redirect_url >0 then
        return self
    end

    local U       = require 'quvi/util'
    local formats = CanalPlus.iter_formats(self, config, U)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, CanalPlus.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = 'canalplus'

    local config  = CanalPlus.get_config(self)

    if #self.redirect_url >0 then
        return self
    end

    local U       = require 'quvi/util'
    local formats = CanalPlus.iter_formats(self, config, U)
    local format  = U.choose_format(self, formats,
                                    CanalPlus.choose_best,
                                    CanalPlus.choose_default,
                                    CanalPlus.to_s)
                        or error("unable to choose format")
    self.url           = {format.url or error("no match: media url")}

    return self
end

--
-- Utility functions
--

function CanalPlus.get_config(self)
    local t    = {}
    local page = quvi.fetch(self.page_url)

    local u = page:match('"og:video" content="(.-)"')
    if u and not u:match('canalplus%.fr') then
        self.redirect_url = u -- Media is hosted elsewhere, e.g. YouTube.
        return
    end

    -- canalplus.fr
    self.title = page:match('videoTitre%s-=%s-"(.-)"')
    if not self.title then
      -- presidentielle2012.canalplus.fr
      self.title = page:match('property="og:title"%s+content="(.-)"')
                    or error('no match: media title')
    end

    self.id = page:match('videoId=(%d+)')
                or page:match('videoId%s+=%s+"(%d+)"')
                or error('no match: media ID')

    local u = "http://service.canal-plus.com/video/rest/getVideosLiees/cplus/"
                .. self.id

    return quvi.fetch(u, {fetch_type = 'config'})
end

function CanalPlus.iter_formats(self, config, U)

    local id = config:match('<ID>(.-)</ID>')
    if id and id == '-1' then
        error('Media is no longer available (expired)')
    end

    local p = '<ID>' .. self.id .. '</ID>'
           .. '.-<INFOS>'
           .. '.-<TITRAGE>'
           .. '.-<MEDIA>'
           .. '.-<IMAGES>'
           .. '.-<PETIT>(.-)<'
           .. '.-<VIDEOS>'
           .. '.-<BAS_DEBIT>(.-)<'
           .. '.-<HAUT_DEBIT>(.-)<'
           .. '.-<HD>(.-)<'

    -- sd = low definition flv
    -- hd = high definition flv
    -- hq = high definition mp4

    local thumb,sd_url,hd_url,hq_url = config:match(p)
    if not sd_url then error("no match: media url") end

    self.thumbnail_url = thumb or ''

    local t = {}
    table.insert(t, {url=sd_url, quality="sd"})
    table.insert(t, {url=hd_url, quality="hd"})
    table.insert(t, {url=hq_url, quality="hq"})
    return t
end

function CanalPlus.choose_default(t)
    return t[1] -- Presume the first to be the 'default'.
end

function CanalPlus.choose_best(t)
    return t[#t] -- Presume the last to be the 'best'.
end

function CanalPlus.to_s(t)
    return t.quality
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
