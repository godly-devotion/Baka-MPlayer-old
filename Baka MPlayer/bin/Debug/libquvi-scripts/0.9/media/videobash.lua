-- libquvi-scripts
-- Copyright (C) 2012  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Thomas Preud'homme <robotux@celest.fr>
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

local Videobash = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Videobash.can_parse_url(qargs),
    domains = table.concat({'videobash.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  qargs.duration_ms = tonumber(p:match('duration=(%d+)') or 0)*1000

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  qargs.title = p:match('"og:title" content="(.-)"') or ''

  qargs.id = qargs.input_url:match('%-(%d+)$') or ''

  qargs.streams = Videobash.iter_streams(p)

  return qargs
end

--
-- Utility functions.
--

function Videobash.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('^www%.videobash%.com$')
       and t.path   and t.path:lower():match('^/video_show/.-%d+$')
  then
    return true
  else
    return false
  end
end

function Videobash.iter_streams(p)
  local S = require 'quvi/stream'
  local u = p:match("file=.-'(%w+%..-)'")
              or error('no match: media stream URL')
  u = table.concat({'http://', u})
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
