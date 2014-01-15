-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Tzafrir Cohen <tzafrir@cohens.org.il>
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

local Tapuz = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Tapuz.can_parse_url(qargs),
    domains = table.concat({'flix.tapuz.co.il'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  -- Make mandatory: the ID required to fetch the XML data
  qargs.id = qargs.input_url:match('/v/watch%-(%d+)%-.*%.html')
  if not qargs.id then
    qargs.id = qargs.input_url:match('/showVideo%.asp%?m=(%d+)')
                or error("no match: media ID")
  end

  local t = {
    'http://flix.tapuz.co.il/v/Handlers/XmlForPlayer.ashx?mediaid=',
    qargs.id, '&playerOptions=0|1|grey|large|0&mako=0'
  }

  local d = quvi.http.fetch(table.concat(t)).data
  local L = require 'quvi/lxph'

  local X = require 'lxp.lom'
  local x = X.parse(d)

  qargs.duration_ms = tonumber(L.find_first_tag(x, 'duration')[1] or 0)*1000

  qargs.thumb_url = L.find_first_tag(x, 'thumbUrl')[1]

  qargs.title = L.find_first_tag(x, 'title')[1]

  qargs.streams = Tapuz.iter_streams(L, x)

  return qargs
end

--
-- Utility functions.
--

function Tapuz.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('flix%.tapuz%.co%.il$')
       and t.path   and (t.path:lower():match('^/v/watch%-.-%.html$')
                          or t.path:lower():match('/showVideo%.asp%?m=%d+'))
  then
    return true
  else
    return false
  end
end

function Tapuz.iter_streams(L, x)
  local u = L.find_first_tag(x, 'videoUrl')[1]
              or error('no match: media stream URL')
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
