-- libquvi-scripts
-- Copyright (C) 2012-2013  Toni Gundogdu <legatvs@gmail.com>
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

local TheOnion = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = TheOnion.can_parse_url(qargs),
    domains = table.concat({'theonion.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  -- Make mandatory: the ID is required for the json URL.
  qargs.id = qargs.input_url:match(',(%d+)/') or error('no match: media ID')

  local u = string.format('http://theonion.com/videos/embed/%s.json',qargs.id)
  local d = quvi.http.fetch(u).data
  local J = require 'json'
  local j = J.decode(d)

  qargs.thumb_url = j['thumbnail'][1] or ''

  qargs.title = j['title'] or ''

  qargs.streams = TheOnion.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function TheOnion.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('theonion%.com$')
       and t.path   and t.path:lower():match('^/video/.-,%d+/')
  then
    return true
  else
    return false
  end
end

function TheOnion.iter_streams(j)
  local u = j['video_url'] or error('no match: media stream URL')
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
