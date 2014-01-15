-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Raphaël Droz <raphael.droz+floss@gmail.com>
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

local Senat = {} -- Utility functions unique to this script.

-- Identify the script.
function ident(qargs)
  return {
    can_parse_url = Senat.can_parse_url(qargs),
    domains = table.concat({'videos.senat.fr'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local C = require 'quvi/const'
  local o = { [C.qoo_fetch_from_charset] = 'iso-8859-1' }
  local p = quvi.http.fetch(qargs.input_url, o).data

  qargs.id = qargs.input_url:match('/video(%d+)%.html$') or ''

  qargs.title = p:match('<title>(.-)</title>') or ''

  qargs.thumb_url = p:match('image=(.-)&') or ''

  qargs.streams = Senat.iter_streams(p)

  return qargs
end

--
-- Utility functions.
--

function Senat.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  local p = '^/%w+/videos/%d+/video%d+%.html$'
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('^videos%.senat%.fr$')
       and t.path   and t.path:lower():match(p)
  then
    return true
  else
    return false
  end
end

function Senat.iter_streams(p)
  local v = p:match('name="flashvars" value="(.-)"')
              or error('no match: flash vars')

  local u = v:match('file=(.-flv)')
              or error('no match: media stream URL')

  local S = require 'quvi/stream'
  local t = S.stream_new(u)

  t.video.height= v:match('height=(%d+)') or 0
  t.video.width = v:match('width=(%d+)') or 0

  return {t}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
