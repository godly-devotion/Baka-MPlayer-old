-- libquvi-scripts
-- Copyright (C) 2010-2013  Toni Gundogdu <legatvs@gmail.com>
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

local ClipFish = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = ClipFish.can_parse_url(qargs),
    domains = table.concat({'clipfish.de'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  -- Make mandatory: the ID is required to fetch the media info
  qargs.id = qargs.input_url:match('/video/(%d+)/.-/$')
                or error("no match: media ID")

  local t = {'http://www.clipfish.de/devxml/videoinfo/', qargs.id}
  local c = quvi.http.fetch(table.concat(t)).data

  local L = require 'quvi/lxph'
  local P = require 'lxp.lom'

  x = P.parse(c)

  qargs.thumb_url = L.find_first_tag(x, 'imageurl')[1]

  qargs.title = L.find_first_tag(x, 'title')[1]

  local d = L.find_first_tag(x, 'duration')[1]
  local T = require 'quvi/time'

  qargs.duration_ms = T.timecode_str_to_s(d)*1000

  qargs.streams = ClipFish.iter_streams(L, x)

  return qargs
end

--
-- Utility functions
--

function ClipFish.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('clipfish%.de$')
       and t.path   and t.path:lower():match('/video/%d+/.-/$')
  then
    return true
  else
    return false
  end
end

function ClipFish.iter_streams(L, x)
  local u = L.find_first_tag(x, 'filename')[1]
              or error('no match: media stream URL')
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
