-- libquvi-scripts
-- Copyright (C) 2011,2013  Toni Gundogdu <legatvs@gmail.com>
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

local TVLux = {} -- Utility functions unique to to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = TVLux.can_parse_url(qargs),
    domains = table.concat({'tvlux.be'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local C = require 'quvi/const'
  local o = { [C.qoo_fetch_from_charset] = 'iso-8859-1' }
  local p = quvi.http.fetch(qargs.input_url, o).data

  qargs.id = qargs.input_url:match('/video/.-(%d+)%.html$') or ''

  qargs.title = p:match('<title>(.-)%s+%-%s+TV') or ''

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  qargs.streams = TVLux.iter_streams(p)

  return qargs
end

--
-- Utility functions
--

function TVLux.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('^www%.tvlux%.be$')
       and t.path   and t.path:lower():match('^/video/.-_%d+%.html$')
  then
    return true
  else
    return false
  end
end

function TVLux.iter_streams(p)
  local d = p:match('setup%((.-)%)') or error('no match: setup')

  local J = require 'json'
  local j = J.decode(d)

  local u = {'http://www.tvlux.be'}
  table.insert(u, j['file'] or error('no match: media stream URL path'))

  local S = require 'quvi/stream'
  local s = S.stream_new(table.concat(u))

  s.video.height = tonumber(j['height'] or 0)
  s.video.width = tonumber(j['width'] or 0)

  return {s}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
