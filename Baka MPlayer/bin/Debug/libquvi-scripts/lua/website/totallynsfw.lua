
-- libquvi-scripts
-- Copyright (C) 2011-2012  quvi project
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

local TotallyNSFW = {} -- Utility functions specific to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "totallynsfw%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    local is_rss = U.handles(self.page_url, {r.domain},
                            {"/videos/rss_2%.0/?"})
    if is_rss then -- Don't try to handle the RSS feeds
        r.handles = false
    else
        r.handles = U.handles(self.page_url, {r.domain},
                              {"/videos/.-", "/nsfw/"})
    end
    return r
end

-- Query available formats.
function query_formats(self)
    local p = quvi.fetch(self.page_url)

    if TotallyNSFW.is_external(self, p) then
        return self
    end

    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
    self.host_id = "totallynsfw"

    local p = quvi.fetch(self.page_url)

    if TotallyNSFW.is_external(self, p) then
        return self
    end

    self.title = p:match('<div class="hdr"><h2>(.-)</h2></div>')
                  or error ("no match: media title")

    local c_url = p:match('"config","(.-)"')
                    or error ("no match: config URL")

    self.id = c_url:match("config_jwplayer_test/(.-)/")
                or error ("no match: media id")

    local c = quvi.fetch(c_url, {fetch_type='config'})

    local s = c:match('<file>(.-)</file>')
                or error ("no match: media url")

    self.url = {s}

    return self
end

--
-- Utility functions.
--

function TotallyNSFW.is_external(self, page)
    local u = page:match('options=(.-)"')
    if u then
        if TotallyNSFW.is_hosted_at_pornhub(self, u) then
            return true
        end
        -- Add more below if necessary.
    end
    return false
end

function TotallyNSFW.is_hosted_at_pornhub(self, url)
    if url:match('pornhub%.com/embed_player') then
        local c = quvi.fetch(url, {fetch_type='config'})
        self.redirect_url = c:match('<link_url>(.-)<') or ''
    end
    return #self.redirect_url >0
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
