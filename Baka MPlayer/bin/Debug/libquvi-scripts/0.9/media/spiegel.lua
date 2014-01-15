-- libquvi-scripts
-- Copyright (C) 2010-2011,2013  Toni Gundogdu <legatvs@gmail.com>
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
-- NOTE: Some streams (e.g. 3gp) do not appear to be available (404) even
--       if they are listed in the config XML.
--

local Spiegel = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Spiegel.can_parse_url(qargs),
    domains = table.concat({'spiegel.de'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local C = require 'quvi/const'
  local o = { [C.qoo_fetch_from_charset] = 'iso-8859-1' }
  local p = quvi.http.fetch(qargs.input_url, o).data

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  qargs.title = p:match('"module%-title">(.-)</')
                  or p:match('"og:title".-content="(.-)%s+%-%s+SP')
                  or ''

  -- Make mandatory: needed to fetch the config XML
  qargs.id = qargs.input_url:match('/video/.-(%d+)%.html$')
                or error('no match: media ID')

  local t = {'http://video.spiegel.de/flash/', qargs.id, '.xml'}
  local c = quvi.http.fetch(table.concat(t)).data

  local P = require 'lxp.lom'
  local x = P.parse(c)

  qargs.streams = Spiegel.iter_streams(x)

  qargs.duration_ms = qargs.streams[1].nostd.duration_ms

  return qargs
end

--
-- Utility functions
--

function Spiegel.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('spiegel%.de$')
       and t.path   and t.path:lower():match('^/video/.-%d+%.html$')
  then
    return true
  else
    return false
  end
end

function Spiegel.iter_streams(x)
  local S = require 'quvi/stream'
  local L = require 'quvi/lxph'

  local r = {}

  for i=1, #x do
    if x[i].tag and x[i].tag:match('type%d+') then

      local n = L.find_first_tag(x[i], 'filename')[1]
      local u = 'http://video.spiegel.de/flash/'..n
      local t = S.stream_new(u)

      t.video = {
        bitrate_kbit_s = tonumber(L.find_first_tag(x[i], 'totalbitrate')[1]),
        height = tonumber(L.find_first_tag(x[i], 'height')[1]),
        encoding = L.find_first_tag(x[i], 'codec')[1]:lower(),
        width = tonumber(L.find_first_tag(x[i], 'width')[1])
      }

      -- Used by this script only. libquvi will ignore the 'nostd' values.
      t.nostd = {
        duration_ms = (tonumber(L.find_first_tag(x[i], 'duration')[1])*1000)
      }

      t.container = (n:match('%.(%w+)$') or ''):lower()
      t.id = Spiegel.to_id(t)

      table.insert(r,t)
    end
  end

  if #r >1 then
    Spiegel.ch_best(S, r)
  end

  return r
end

function Spiegel.ch_best(S, t)
  local r = t[1]
  r.flags.best = true
  for _,v in pairs(t) do
    if v.video.height > r.video.height then
      r = S.swap_best(r, v)
    end
  end
end

function Spiegel.to_id(t)
    return string.format('%s_%s_%sk_%sp',
        t.container, t.video.encoding, t.video.bitrate_kbit_s, t.video.height)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
