-- libquvi-scripts
-- Copyright (C) 2012-2013  Toni Gundogdu <legatvs@gmail.com>
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

local Soundcloud = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Soundcloud.can_parse_url(qargs),
    domains = table.concat({'soundcloud.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  Soundcloud.normalize(qargs)

  local p = quvi.http.fetch(qargs.input_url).data

  qargs.thumb_url = p:match('.+content="(.-)"%s+property="og:image"') or ''
  qargs.title = p:match('.+content="(.-)"%s+property="og:title"') or ''

  local m = p:match("window%.SC%.bufferTracks%.push(%(.-%);)")
              or error("no match: metadata")

  qargs.duration_ms = tonumber(m:match('"duration":(%d-),')) or 0
  qargs.id = m:match('"uid":"(%w-)"') or ''

  qargs.streams = Soundcloud.iter_streams(m);

  return qargs
end

--
-- Utility functions
--

function Soundcloud.can_parse_url(qargs)
  Soundcloud.normalize(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http?$')
       and t.host   and t.host:lower():match('soundcloud%.com$')
       and t.path   and (
            -- Not a playlist URL.
           not t.path:lower():match('^/.-/sets/[%w-_]+/$')
            -- But has the following path properties.
           and t.path:lower():match('^/.+/.+$')
       )
  then
    return true
  else
    return false
  end
end

function Soundcloud.normalize(qargs) -- "Normalize" an embedded URL
  local url = qargs.input_url:match('swf%?url=(.-)$')
  if not url then return end

  local U = require 'quvi/util'
  local u = string.format('http://soundcloud.com/oembed?url=%s&format=json',
                            U.unescape(url))

  qargs.input_url = quvi.http.fetch(u).data:match('href=\\"(.-)\\"')
                      or error('no match: media URL')
end

function Soundcloud.iter_streams(p)
  local u = p:match('"streamUrl":"(.-)"')
              or error("no match: media stream URL")
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
