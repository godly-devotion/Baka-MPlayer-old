-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Mikhail Gusarov <dottedmag@dottedmag.net>
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

local OTvRu = {} -- Utility functions specific to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = OTvRu.can_parse_url(qargs),
    domains = table.concat({'1tv.ru'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  qargs.id = qargs.input_url:match('fi(%d+)')
              or qargs.input_url:match('fi=(%d+)')
              or qargs.input_url:match('p(%d+)')
                or ''

  local C = require 'quvi/const'
  local o = { [C.qoo_fetch_from_charset] = 'windows-1251' }
  local p = quvi.http.fetch(qargs.input_url, o).data

  --
  -- Videos and articles (?) share similar URL path structure. Look up
  -- 'og:video' in the page HTML to determine whether this is a video page.
  --
  if not p:match('og:video') then
    error('no video: page does not appear to contain a media stream')
  end

  local d = p:match('"og:video:duration" content="(%d+)"') or 0
  qargs.duration_ms = tonumber(d)*1000

  d = p:match('playlistObj.-=.-%[(.-)%]') or error('no match: playlistObj')

  local J = require 'json'
  local j = J.decode(d)

  qargs.streams = OTvRu.iter_streams(j)

  qargs.thumb_url = j['image'] or ''

  qargs.title = j['title'] or ''

  return qargs
end

--
-- Utility functions
--

function OTvRu.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('1tv%.ru$')
       and t.path   and (t.path:lower():match('^/sprojects_edition/')
                         or t.path:lower():match('^/sprojects_utro_video/'))
  then
    return true
  else
    return false
  end
end

function OTvRu.iter_streams(j)
  local u = j['file'] or error('no match: media stream URL')
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
