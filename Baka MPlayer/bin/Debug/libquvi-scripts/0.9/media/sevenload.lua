-- libquvi-scripts
-- Copyright (C) 2010-2013  Toni Gundogdu <legatvs@gmail.com>
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

local SevenLoad = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = SevenLoad.can_parse_url(qargs),
    domains = table.concat({'sevenload.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  local E = require 'quvi/entity'
  p = E.convert_html(p)

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  qargs.title = p:match('"og:title" content="(.-)"') or ''

  qargs.id = p:match('videoid":"(.-)"') or ''

  qargs.streams = SevenLoad.iter_streams(p)

  return qargs
end

--
-- Utility functions
--

function SevenLoad.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('sevenload%.com$')
       and t.path   and t.path:lower():match('^/videos/')
  then
    return true
  else
    return false
  end
end

function SevenLoad.stream_new(S, t)
  local u = t['src'] or error('no match: media stream URL')
  local s = S.stream_new(u)
  -- 'nostd' is a temporary dictionary and will be ignored by libquvi.
  s.nostd = {
    quality = u:match('%-(%w+)%.%w+$') or ''
  }
  s.video.encoding = t['video_codec'] or ''
  s.audio.encoding = t['audio_codec'] or ''
  s.container = u:match('%.(%w+)$') or ''
  s.id = SevenLoad.to_id(s)
  return s
end

function SevenLoad.iter_streams(p)
  local S = require 'quvi/stream'
  local J = require 'json'

  local d = p:match('data%-html5="(.-)" ') or error('no match: data-html5')
  local j = J.decode(d)

  local s = j['sources'] or error('"sources" not found')
  local r = {}

  for _,v in pairs(s) do
    table.insert(r, SevenLoad.stream_new(S,v))
  end

  return r
end

function SevenLoad.to_id(t)
  return string.format('%s_%s', t.nostd.quality, t.container)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
