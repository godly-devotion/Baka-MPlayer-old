
-- libquvi-scripts v0.4.10
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

-- Hundred and One Great Goals
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
    r.handles    = U.handles(self.page_url, {r.domain}, {"/gvideos/.+"})
    return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return HaOgg.check_external_content(self)
end

-- Parse media URL.
function parse(self)
    self.host_id = "101greatgoals"
    return HaOgg.check_external_content(self)
end

--
-- Utility functions
--

function HaOgg.check_external_content(self)
    local p = quvi.fetch(self.page_url)

    local m = '<div .- id="space4para" class="post%-type%-gvideos">'
              ..'.-<script (.-)</script>'
    local a = p:match(m) or error("no match: article")

    -- Self-hosted, and they use YouTube
    -- http://www.101greatgoals.com/gvideos/golazo-wanchope-abila-sarmiento-junin-v-merlo-2/
    if a:match('id="jwplayer%-1%-div"') then -- get the javascript chunk for jwplayer
        local U = require 'quvi/util'
        local s = p:match('"file":"(.-)"') or error('no match: file location')
        a = U.unescape(s):gsub("\\/", "/")
    end

    -- e.g. http://www.101greatgoals.com/gvideos/ea-sports-uefa-euro-2012-launch-trailer/
    -- or
    -- http://www.101greatgoals.com/gvideos/golazo-wanchope-abila-sarmiento-junin-v-merlo-2/
    local s = a:match('http://.*youtube.com/embed/([^/"]+)')
                or a:match('http://.*youtube.com/v/([^/"]+)')
                or a:match('http://.*youtube.com/watch%?v=([^/"]+)')
                or a:match('http://.*youtu%.be/([^/"]+)')
    if s then
        self.redirect_url = 'http://youtube.com/watch?v=' .. s
        return self
    end

    -- e.g. http://www.101greatgoals.com/gvideos/leicester-1-west-ham-2/
    -- or
    -- http://www.101greatgoals.com/gvideos/golazo-alvaro-negredo-overhead-kick-puts-sevilla-1-0-up-at-getafe/
    local s = a:match('http://.*dailymotion.com/embed/video/([^?"]+)')
              or a:match('http://.*dailymotion.com/swf/video/([^?"]+)')
    if s then
        self.redirect_url = 'http://dailymotion.com/video/' .. s
        return self
    end

    -- e.g. http://www.101greatgoals.com/gvideos/2-0-juventus-arturo-vidal-2-v-roma/
    local s = a:match('http://.*videa.hu/flvplayer.swf%?v=([^?"]+)')
    if s then
        self.redirect_url = 'http://videa.hu/flvplayer.swf?v=' .. s
        return self
    end

    -- e.g. http://www.101greatgoals.com/gvideos/golazo-hulk-porto-v-benfica/
    local s = a:match('http://.*sapo.pt/([^?"/]+)')
    if s then
        self.redirect_url = 'http://videos.sapo.pt/' .. s
        return self
    end

    -- FIXME rutube support missing
    -- e.g. http://www.101greatgoals.com/gvideos/allesandro-diamanti-bologna-1-0-golazo-v-cagliari-2/
    local s = a:match('http://video.rutube.ru/([^?"]+)')
    if s then
        self.redirect_url = 'http://video.rutube.ru/' .. s
        return self
    end

    -- FIXME svt.se support missing
    -- e.g. http://www.101greatgoals.com/gvideos/gais-2-norrkoping-0/
    local s = a:match('http://svt%.se/embededflash/(%d+)/play%.swf')
    if s then
        self.redirect_url = 'http://svt.se/embededflash/' .. s .. '/play.swf'
        return self
    end

    -- FIXME lamalla.tv support missing
    -- e.g. http://www.101greatgoals.com/gvideos/golazo-bakary-espanyol-b-vs-montanesa/

    -- FIXME indavideo.hu support missing
    -- e.g. http://www.101greatgoals.com/gvideos/golazo-michel-bastos-lyon-v-psg-3/

    -- FIXME xtsream.dk support missing
    -- e.g. http://www.101greatgoals.com/gvideos/golazo-the-ball-doesnt-hit-the-floor-viktor-claesson-elfsborg-v-fc-copenhagen-1-22-mins-in/

    -- FIXME mslsoccer.com support missing
    -- e.g. http://www.101greatgoals.com/gvideos/thierry-henry-back-heel-assist-mehdi-ballouchy-v-montreal-impact/

    error("FIXME: no support: Unable to determine the media host")
end

-- vim: set ts=4 sw=4 tw=72 expandtab:
