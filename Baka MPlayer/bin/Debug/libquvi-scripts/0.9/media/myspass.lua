-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Guido Leisker <guido@guido-leisker.de>
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

local MySpass = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = MySpass.can_parse_url(qargs),
    domains = table.concat({'myspass.de'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  -- Make mandatory: the media ID is needed to fetch the data XML.
  qargs.id = qargs.input_url:match("(%d+)/?$") or error("no match: media ID")

  local t = {
    'http://www.myspass.de/myspass/includes/apps/video',
    '/getvideometadataxml.php?id=', qargs.id
  }

  local c = quvi.http.fetch(table.concat(t)).data
  local P = require 'lxp.lom'

  local L = require 'quvi/lxph'
  local x = P.parse(c)

  qargs.thumb_url = L.find_first_tag(x, 'imagePreview')[1]

  qargs.duration_ms = MySpass.to_duration_ms(L, x)

  qargs.streams = MySpass.iter_streams(L, x)

  qargs.title = MySpass.to_title(L, x)

  return qargs
end

--
-- Utility functions
--

function MySpass.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('myspass%.de$')
       -- Expect all URLs ending with digits to be videos.
       and t.path   and t.path:lower():match('^/myspass/.-/%d+/?$')
  then
    return true
  else
    return false
  end
end

function MySpass.iter_streams(L, x)
  local u = L.find_first_tag(x, 'url_flv')[1]
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

function MySpass.to_duration_ms(L, x)
  local m,s = L.find_first_tag(x, 'duration')[1]:match('(%d+)%:(%d+)')
  m = tonumber(((m or ''):gsub('%a',''))) or 0
  m = tonumber(((s or ''):gsub('%a',''))) or 0
  return (m*60000) + (s*1000)
end

function MySpass.to_title(L, x)
  local t = {
    L.find_first_tag(x, 'format')[1],
    string.format('s%02de%02d -', L.find_first_tag(x, 'season')[1],
                                  L.find_first_tag(x, 'episode')[1]),
    L.find_first_tag(x, 'title')[1]
  }
  return table.concat(t, ' ')
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
