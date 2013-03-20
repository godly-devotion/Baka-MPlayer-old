
-- libquvi-scripts
-- Copyright (C) 2012  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010-2011  Lionel Elie Mamane <lionel@mamane.lu>
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

local CollegeHumor = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    local domains= {"collegehumor%.com", "dorkly%.com"}
    r.domain     = table.concat(domains, "|")
    r.formats    = "default|best"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, domains,
                    {"/video[:/]%d+/?", "/embed/%d+"})
    return r
end

-- Query formats.
function query_formats(self)
    if CollegeHumor.redirect_if_embed(self) then
        return self
    end

    local config  = CollegeHumor.get_config(self)
    local formats = CollegeHumor.iter_formats(config)

    local t = {}
    for k,v in pairs(formats) do
        table.insert(t, CollegeHumor.to_s(v))
    end

    table.sort(t)
    self.formats = table.concat(t, "|")

    return self
end

-- Parse media URL.
function parse(self)
    self.host_id  = "collegehumor"

    if CollegeHumor.redirect_if_embed(self) then
        return self
    end

    local c = CollegeHumor.get_config(self)

    self.title = c:match('<caption>.-%[.-%[(.-)%]%]')
                  or error("no match: media title")

    self.thumbnail_url = c:match('<thumbnail><!%[.-%[(.-)%]') or ''

    local formats = CollegeHumor.iter_formats(c)
    local U       = require 'quvi/util'
    local format  = U.choose_format(self, formats,
                                     CollegeHumor.choose_best,
                                     CollegeHumor.choose_default,
                                     CollegeHumor.to_s)
                        or error("unable to choose format")
    self.url      = {format.url or error("no match: media URL")}
    return self
end

--
-- Utility functions
--

function CollegeHumor.redirect_if_embed(self) -- dorkly embeds YouTube videos
    if self.page_url:match('/embed/%d+') then
        local p = quvi.fetch(self.page_url)
        local s = p:match('youtube.com/embed/([%w-_]+)')
        if s then
            -- Hand over to youtube.lua
            self.redirect_url = 'http://youtube.com/watch?v=' .. s
            return true
        end
    end
    return false
end

function CollegeHumor.get_media_id(self)
    local U = require 'quvi/url'
    local domain = U.parse(self.page_url).host:gsub('^www%.', '', 1)

    self.host_id = domain:match('^(.+)%.[^.]+$') or error("no match: domain")

    self.id = self.page_url:match('/video[/:](%d+)')
                or error("no match: media ID")

    return domain
end

function CollegeHumor.get_config(self)
    local domain = CollegeHumor.get_media_id(self)

    -- quvi normally checks the page URL for a redirection to another
    -- URL. Disabling this check (QUVIOPT_NORESOLVE) breaks the support
    -- which is why we do this manually here.
    local r = quvi.resolve(self.page_url)

    -- Make a note of the use of the quvi.resolve returned string.
    local u = string.format("http://www.%s/moogaloop/video%s%s",
                              domain, (#r > 0) and ':' or '/', self.id)

    return quvi.fetch(u, {fetch_type='config'})
end

function CollegeHumor.iter_formats(config)
    local sd_url = config:match('<file><!%[.-%[(.-)%]')
    local hq_url = config:match('<hq><!%[.-%[(.-)%]')
    local hq_avail = (hq_url and #hq_url > 0) and 1 or 0

    local t = {}

    local s = sd_url:match('%.(%w+)$')
    table.insert(t, {quality='sd', url=sd_url, container=s})

    if hq_avail == 1 and hq_url then
        local s = hq_url:match('%.(%w+)$')
        table.insert(t, {quality='hq', url=hq_url, container=s})
    end

    return t
end

function CollegeHumor.choose_best(formats) -- Assume last is best.
    local r
    for _,v in pairs(formats) do r = v end
    return r
end

function CollegeHumor.choose_default(formats) -- Whatever is found first.
    for _,v in pairs(formats) do return v end
end

function CollegeHumor.to_s(t)
    return string.format('%s_%s', t.container, t.quality)
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
