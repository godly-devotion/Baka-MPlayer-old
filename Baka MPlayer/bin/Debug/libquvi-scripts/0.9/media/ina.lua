-- libquvi-scripts
-- Copyright (C) 2013  Mohamed El Morabity <melmorabity@fedoraproject.org>
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

local Ina = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Ina.can_parse_url(qargs),
    domains = table.concat({'ina.fr'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  -- Make mandatory: needed to fetch the video data XML.
  qargs.id = qargs.input_url:match('/video/(%w+)')
               or qargs.input_url:match('/audio/(%w+)')
               or error('no match: media ID')

  -- Fetch the video data XML
  local u = string.format('http://player.ina.fr/notices/%s.mrss?site=visio',
              qargs.id)
  local c = quvi.http.fetch(u).data
  local P = require 'lxp.lom'
  local x = P.parse(c)

  qargs.streams = Ina.iter_streams(x)

  qargs.title = qargs.streams[1].nostd.title

  qargs.thumb_url = qargs.streams[1].nostd.thumb_url

  qargs.duration_ms = qargs.streams[1].nostd.duration_ms

  return qargs
end

--
-- Utility functions
--

function Ina.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('ina%.fr$')
       and t.path   and (t.path:lower():match('^/video/%w+')
                         or t.path:lower():match('^/audio/%w+'))
  then
    return true
  else
    return false
  end
end

function Ina.iter_streams(x)
  local S = require 'quvi/stream'
  local L = require 'quvi/lxph'

  local c = L.find_first_tag(x, 'channel')
  local m = L.find_first_tag(L.find_first_tag(c, 'item'),
              'media:content')
  local t = L.find_first_tag(m, 'media:thumbnail')

  local u = m.attr.url or error('no match: media stream URL')

  local s = S.stream_new(u)
  s.nostd = {
    title = L.find_first_tag(c, 'title')[1] or '',
    thumb_url = t.attr.url,
    duration_ms = tonumber(m.attr.duration) * 1000
  }

  return {s}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
