-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010-2012  quvi project
--
-- This file is part of libquvi-scripts <http://quvi.sourceforge.net/>.
--
-- This program is free software: you can redistribute it and/or
-- modify it under the terms of the GNU Affero General Public
-- License as published by the Free Software Foundation, either
-- version 3 of the License, or (at your option) any later version.
--
-- This program is distributed in the hope that it will be useful,
-- but WITHOUT ANY WARRANTY; without even the implied warranty of
-- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
-- GNU Affero General Public License for more details.
--
-- You should have received a copy of the GNU Affero General
-- Public License along with this program.  If not, see
-- <http://www.gnu.org/licenses/>.
--

local CharlieRose = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = CharlieRose.can_parse_url(qargs),
    domains = table.concat({'charlierose.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  qargs.title = p:match("<title>Charlie Rose%s+-%s+(.-)</title>") or ''

  qargs.thumb_url = p:match('rel="image_src" href="(.-)"') or ''

  qargs.id = p:match('view%/content%/(.-)"') or ''

  qargs.streams = CharlieRose.iter_streams(p)

  return qargs
end

--
-- Utility functions
--

function CharlieRose.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('charlierose%.com$')
       and t.path   and t.path:lower():match('^/view/.-/%d+')
  then
    return true
  else
    return false
  end
end

function CharlieRose.iter_streams(p)
  local u = p:match('url":"(.-)"') or error("no match: media stream URL")

  local h = p:match('name="video_height" content="(%d+)"') or 0
  local w = p:match('name="video_width" content="(%d+)"') or 0

  local S = require 'quvi/stream'
  local s = S.stream_new(u)

  s.video.height = h
  s.video.width = w

  return {s}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
