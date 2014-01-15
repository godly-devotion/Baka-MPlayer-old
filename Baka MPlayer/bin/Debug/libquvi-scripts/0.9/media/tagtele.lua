-- libquvi-scripts
-- Copyright (C) 2012,2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010  Paul Kocialkowski <contact@paulk.fr>
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

local Tagtele = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Tagtele.can_parse_url(qargs),
    domains = table.concat({'tagtele.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data
  local s = p:match('setup%((.-)%)') or error('no match: setup')

  local J = require 'json'
  local j = J.decode(s)

  qargs.thumb_url = j['image'] or ''

  if #qargs.thumb_url >0 then
    --
    -- Make sure the thumb URL uses the same scheme as the input URL.
    -- e.g. HTTPS. For some reason this isn't done by default.
    --
    local U = require 'socket.url'
    local t = U.parse(qargs.thumb_url)
    t.scheme = U.parse(qargs.input_url).scheme
    qargs.thumb_url = U.build(t)
  end

  qargs.title = p:match('"og:title" content="(.-)"') or ''

  qargs.id = qargs.input_url:match('/voir/(%d+)/') or ''

  qargs.streams = Tagtele.iter_streams(j)

  return qargs
end

--
-- Utility functions.
--

function Tagtele.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('^www%.tagtele%.com$')
       and t.path   and t.path:lower():match('^/videos/voir/%d+/$')
  then
    return true
  else
    return false
  end
end

function Tagtele.iter_streams(j)
  local m = 'no match: media stream URL'
  local S = require 'quvi/stream'
  local r = {}
  for _,v in pairs(j['sources']) do
    for _,vv in pairs(v) do
      --
      -- Forcing HTTPs here as we do with the thumb URLs would only
      -- result in errors. The streams are available via HTTP only.
      --
      local t = S.stream_new(v.file or error(m))
      t.id = table.concat({v.type,v.label}, '_')
      table.insert(r,t)
    end
  end
  if #r ==0 then
    error(m)
  end
  return r
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
