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

local CBSNews = {} -- Utility functions unique to this script

-- Identify the script.
function ident(qargs)
  return {
    can_parse_url = CBSNews.can_parse_url(qargs),
    domains = table.concat({'cbsnews.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  local d = p:match("data%-cbsvideoui%-options='(.-)'")
              or error('no match: player options')

  local J = require 'json'
  local v = J.decode(d).state.video

  qargs.duration_ms = (v.duration or 0) *1000

  qargs.streams = CBSNews.iter_streams(v)

  qargs.thumb_url = v.image.path or ''

  qargs.title = v.title or ''

  qargs.id = v.id or ''

  return qargs
end

--
-- Utility functions
--

function CBSNews.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('^www%.cbsnews%.com$')
       and t.path   and t.path:lower():match('^/videos/.-/?$')
  then
    return true
  else
    return false
  end
end

function CBSNews.iter_streams(v)
  local S = require 'quvi/stream'

  local m = v.medias
  local r = {}

  for k,_ in pairs(m) do
    local a = m[k]
    if type(a) == 'table' then
      local t = S.stream_new(a.uri)
      CBSNews.optional_stream_props(t, a)
      t.id = CBSNews.to_id(t, k)
      -- For lack of a better solution:
      --  Set the 'desktop' stream as the 'best'
      if k == 'desktop' then
        t.flags.best = true
      end
      table.insert(r, t)
    end
  end
  return r
end

function CBSNews.optional_stream_props(t, a)
  t.container = t.url:match('%.(%w+)$') or ''
  local b = tonumber(a.bitrate or 0)
  if b >1000 then
    b = b/1000
  end
  t.video.bitrate_kbit_s = b
end

function CBSNews.to_id(t, k)
  return string.format('%s_%s_%dk', t.container, k, t.video.bitrate_kbit_s)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
