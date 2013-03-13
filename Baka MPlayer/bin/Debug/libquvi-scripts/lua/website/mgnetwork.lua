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

--
-- NOTE: Media General is a company that owns many local news stations
-- and newspapers in the southeast US. They all use the same system for
-- videos.
--  <http://sourceforge.net/apps/trac/quvi/ticket/78>
--

local MGNetwork = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    local domains= {'alabamas13%.com', 'counton2%.com',
        'dailyprogress%.com', 'dothaneagle%.com', 'eprisenow%.com',
        'eufaulatribune%.com', 'godanriver%.com', 'hickoryrecord%.com',
        'independenttribune%.com', 'insidenova%.com', 'jcfloridan%.com',
        'journalnow%.com', 'oanow%.com', 'morganton%.com',
        'nbc4i%.com', 'nbc17%.com', 'newsadvance%.com',
        'newsvirginian%.com', 'richmond%.com', 'sceneon7%.com',
        'scnow%.com', 'starexponent%.com', 'statesville%.com',
        'tbo%.com', 'timesdispatch%.com', 'tricities%.com',
        'turnto10%.com', 'wnct%.com', 'whlt%.com', 'wjbf%.com',
        'wjtv%.com', 'wkrg%.com', 'wrbl%.com', 'wsav%.com', 'wsls%.com',
        'wspa%.com', 'vteffect%.com'}
    r.domain     = 'mgnetwork%.com'
    r.formats    = 'default|best'
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url,
            domains,
            {"/%w+/.+%-%d+"})
    return r
end

-- Query available formats.
function query_formats(self)
    local config  = MGNetwork.get_config(self)
    local formats = MGNetwork.iter_formats(config)

    local t = {}
    for _,v in pairs(formats) do
        table.insert(t, MGNetwork.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = 'mgnetwork'

    local xml = MGNetwork.get_config(self)

    self.title = xml:match('<item>.-<title>(.-)</title>')
                    or error("no match: media title")

    local formats = MGNetwork.iter_formats(xml)
    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                    MGNetwork.choose_best,
                                    MGNetwork.choose_default,
                                    MGNetwork.to_s)
                        or error("unable to choose format")
    self.url = {format.url or error("no match: media url")}

    -- Optional: we can live without these

    self.thumbnail_url = xml:match('<media:thumbnail url="(.-)"') or ''

    local d = xml:match('duration="(.-)"') or 0
    self.duration = math.ceil(d) * 1000 -- to msec

    return self
end

--
-- Utility functions
--

function MGNetwork.get_config(self)
    -- local config_url = string.format(
    --    'http://%s/video/get/media_response_related_' ..
    --    'for_content/%s/%d/', d, v, self.id)
    local d,v,s = self.page_url:match("http://([%w%.]+)/.+%-(%w+)%-(%d+)/")
    self.id = s or error("no match: media id")

    -- Ideally, we'd skip this step and just get the config.
    -- Do this so that we can check whether the video exists.
    local d = quvi.fetch(self.page_url)
    local p = "if %(!mrss_link%){ mrss_link = '(.-)'; }"
    local s = d:match(p) or error('no match: no video found on page')

    return quvi.fetch(s, {fetch_type = 'config'})
end

function MGNetwork.iter_formats(config)
    local p = 'content url="(.-)" type="%w+/(.-)"'
    local t = {}
    for u,c in config:gmatch(p) do
        table.insert(t, {url=u, container=c})
        --print(u,c)
    end
    return t
end

function MGNetwork.choose_best(t) -- Expect the first to be the 'best'
  return t[1]
end

function MGNetwork.choose_default(t) -- Expect the last to be the 'default'
  return t[#t]
end

function MGNetwork.to_s(t)
  return t.container
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
