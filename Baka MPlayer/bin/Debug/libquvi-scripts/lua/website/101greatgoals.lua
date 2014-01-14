
-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  quvi project
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

-- Hundred and One Great Goals (aggregator)
local HaOgg = {} -- Utility functions specific to this script

-- Identify the script.
function ident(self)
    package.path = self.script_dir .. '/?.lua'
    local C      = require 'quvi/const'
    local r      = {}
    r.domain     = "101greatgoals%.com"
    r.formats    = "default"
    r.categories = C.proto_http
    local U      = require 'quvi/util'
    r.handles    = U.handles(self.page_url, {r.domain}, {"/gvideos/.+/$"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return HaOgg.chk_ext_content(self)
end

-- Parse media URL.
function parse(self)
    self.host_id = "101greatgoals"
    return HaOgg.chk_ext_content(self)
end

--
-- Utility functions
--

function HaOgg.chk_self_hosted(p)
    --
    -- Previously referred to as the "self-hosted" media, although according
    -- to the old notes, these were typically hosted by YouTube.
    --    http://is.gd/EKKPy2
    --
    -- 2013-05-05: The contents of the URL no longer seems to contain the
    --             "file" value, see chk_embedded for notes; keep this
    --             function around for now
    --
    local d = p:match('%.setup%((.-)%)')
    if d then
        local s = d:match('"file":"(.-)"') or error('no match: file')
        if #s ==0 then
            error('empty media URL ("file")')
        end
        local U = require 'quvi/util'
        return (U.slash_unescape(U.unescape(s)))
    end
end

function HaOgg.chk_embedded(p)
    --
    -- 2013-05-05: Most of the content appears to be embedded from elsewhere
    --
    -- Instead of trying to check for each, parse the likely embedded source
    -- and pass it back to libquvi to find a media script that accepts the
    -- parsed (embedded) media URL.
    --
    -- NOTE: This means that those media scripts must unwrangle the embedded
    --       media URLs passed from this script
    --
    local s = p:match('class="post%-type%-gvideos">(.-)</')
                  or p:match('id="jwplayer%-1">(.-)</>')
                      or error('unable to determine embedded source')
    return s:match('value="(.-)"') or s:match('src="(.-)"')
end

function HaOgg.chk_ext_content(self)
    local p = quvi.fetch(self.page_url)

    local r = HaOgg.chk_self_hosted(p) or HaOgg.chk_embedded(p)
                  or error('unable to determine media source')

    self.redirect_url = (not r:match('^%w+://'))
                         and table.concat({'http:',r})
                          or r

    return self
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
