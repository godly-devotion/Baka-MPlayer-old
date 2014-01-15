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

--
-- NOTE: Vimeo is picky about the user-agent string.
--

local Vimeo = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Vimeo.can_parse_url(qargs),
    domains = table.concat({'vimeo.com'}, ',')
  }
end

-- Parse media stream URL.
function parse(qargs)
  Vimeo.normalize(qargs)

  qargs.id = qargs.input_url:match('/(%d+)$') or error('no match: media ID')
  local c = Vimeo.config_new(qargs)

  local J = require 'json'
  local j = J.decode(c)

  qargs.duration_ms = tonumber(j.video.duration or 0) * 1000
  qargs.streams = Vimeo.iter_streams(qargs, j)

  qargs.thumb_url = Vimeo.thumb_new(j)
  qargs.title = j.video.title or ''

  return qargs
end

--
-- Utility functions
--

function Vimeo.can_parse_url(qargs)
  Vimeo.normalize(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('vimeo%.com$')
       and t.path   and t.path:lower():match('^/%d+$')
  then
    return true
  else
    return false
  end
end

function Vimeo.config_new(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  t.host = 'player.vimeo.com'
  t.path = table.concat({'/video/', qargs.id})
  local p = quvi.http.fetch(U.build(t)).data
  return p:match('b=(.-);') or error('no match: b')
end

function Vimeo.thumb_new(j)
  local r = {}
  for _,u in pairs(j.video.thumbs) do
    table.insert(r,u)
  end
  return r[#r] or ''
end

function Vimeo.normalize(qargs)
  qargs_input_url = qargs.input_url:gsub("player%.", "")
  qargs.input_url = qargs.input_url:gsub("/video/", "/")
end

function Vimeo.iter_streams(qargs, j)
  local S = require 'quvi/stream'
  local h = j.request.files.h264

  local r = {}
  for k,v in pairs(h) do
    local s = S.stream_new(v.url)
    s.container = v.url:match('%.(%w+)%?') or ''
    s.video.bitrate_kbit_s = v.bitrate
    s.video.height = v.height
    s.video.width = v.width
    s.id = Vimeo.to_id(s,k)
    table.insert(r,s)
  end

  if #r >1 then
    Vimeo.ch_best(S, r)
  end

  return r
end

function Vimeo.ch_best(S, t)
  local r = t[1]
  r.flags.best = true
  for _,v in pairs(t) do
    if v.video.height > r.video.height then
      r = S.swap_best(r, v)
    end
  end
end

function Vimeo.to_id(t, quality)
  return string.format("%s_%s_%dk_%dp",
            quality, t.container, t.video.bitrate_kbit_s, t.video.height)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
