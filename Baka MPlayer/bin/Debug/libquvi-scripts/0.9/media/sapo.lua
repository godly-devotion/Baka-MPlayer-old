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

local SAPO = {} -- Utility functions unique to to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = SAPO.can_parse_url(qargs),
    domains = table.concat({'videos.sapo.pt'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  -- The thumbnail URL, or the duration, are not in the oembed data.
  local p = quvi.http.fetch(qargs.input_url).data

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  local d = p:match('(%d+%:%d+%:%d+)') or ''
  local T = require 'quvi/time'
  qargs.duration_ms = T.timecode_str_to_s(d) * 1000

  local t = {'http://videos.sapo.pt/oembed?url=', qargs.input_url}
  local d = quvi.http.fetch(table.concat(t)).data

  local J = require 'json'
  local j = J.decode(d)

  qargs.id = qargs.input_url:match('/(%w+)$') or ''
  qargs.title = j['title'] or ''

  qargs.streams = SAPO.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function SAPO.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('^videos%.sapo%.pt$')
       and t.path   and t.path:lower():match('^/%w+$')
  then
    return true
  else
    return false
  end
end

function SAPO.iter_streams(j)
  local m = j['html'] or error('no match: html')
  local u = m:match('file=(.-)&') or error('no match: media stream URL')

  local S = require 'quvi/stream'
  local s = S.stream_new(u)

  s.video.height = tonumber(j['height'] or 0)
  s.video.width = tonumber(j['width'] or 0)

  return {s}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
