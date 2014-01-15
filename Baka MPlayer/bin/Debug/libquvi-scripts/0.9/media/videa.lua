-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Bastien Nocera <hadess@hadess.net>
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

local Videa = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  Videa.normalize(qargs)
  return {
    can_parse_url = Videa.can_parse_url(qargs),
    domains = table.concat({'videa.hu'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  Videa.normalize(qargs)

  local p = quvi.http.fetch(qargs.input_url).data
  local s = p:match('videok%((.-),player') or error('no match: videok')

  local J = require 'json'
  local j = J.decode(s)

  qargs.thumb_url = p:match('"og:image"%s+content="(.-)"') or ''

  qargs.duration_ms = tonumber(j['video']['duration'] or 0) * 1000

  qargs.title = j['video']['title'] or ''

  qargs.id = j['vcode'] or ''

  qargs.streams = Videa.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function Videa.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('^videa%.hu$')
       and t.path   and t.path:lower():match('^/videok/.+/.+%-%w+$')
  then
    return true
  else
    return false
  end
end

function Videa.normalize(qargs) -- "Normalize" an embedded URL
  local s = qargs.input_url:match('/flvplayer%.swf%?v=(.-)$')
  if s then
    qargs.input_url = table.concat({'http://videa.hu/videok/',s})
  end
end

function Videa.iter_streams(j)
  local w = j['swf_url']:match('=(.-)&') or error('no match: f parameter')
  local t = {'http://videa.hu/static/video/', (w:gsub('%.%d+$',''))}

  local S = require 'quvi/stream'
  local s = S.stream_new(table.concat(t))

  s.video.height = tonumber(j['video']['height'] or 0)
  s.video.width = tonumber(j['video']['width'] or 0)

  return {s}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
