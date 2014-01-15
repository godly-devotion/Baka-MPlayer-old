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

local LiveLeak = {} -- Utility functions specific to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = LiveLeak.can_parse_url(qargs),
    domains = table.concat({'liveleak.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  qargs.title = p:match("<title>LiveLeak.com%s+%-%s+(.-)</") or ''

  qargs.id = qargs.input_url:match('view%?i=([%w_]+)') or ''

  local d = p:match("setup%((.-)%)%.")

  if not d then  -- Try the first iframe
    qargs.goto_url = p:match('<iframe.-src="(.-)"') or ''
    if #qargs.goto_url >0 and not qargs.goto_url:match('facebook') then
      return qargs
    else
      error('no match: setup')
    end
  end

  local j = LiveLeak.sanitize_json(d)

  qargs.thumb_url = j['image'] or ''

  qargs.streams = LiveLeak.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function LiveLeak.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('liveleak%.com$')
       and t.path   and t.path:lower():match('^/view')
       and t.query  and t.query:lower():match('i=[_%w]+')
  then
    return true
  else
    return false
  end
end

function LiveLeak.stream_new(S, url)
  local v_enc, v_h, c = url:match('%.(%w+)_(%d+)p%.(%w+)%?')
  local s = S.stream_new(url)
  s.video.height = tonumber(v_h or 0)
  s.video.encoding = v_enc
  s.container = c
  return LiveLeak.to_id(s)
end

function LiveLeak.iter_streams(j)
  local S = require 'quvi/stream'
  local U = require 'socket.url'

  local t = U.parse(j['config'])
  local r = {}

  for f_url in t.query:gmatch('file_url=(.-)&') do
    local u = U.unescape(f_url)
    table.insert(r, LiveLeak.stream_new(S, u))
  end

  if #r ==0 then
    error('no match: media stream URL')
  else
    if #r >1 then -- Pick one stream as the 'best' quality.
      LiveLeak.ch_best(S, r)
    end
  end

  return r
end

function LiveLeak.ch_best(S, t)
  local r = t[1] -- Make the first one the 'best' by default.
  r.flags.best = true
  for _,v in pairs(t) do
    if v.video.height > r.video.height then
      r = S.swap_best(r, v)
    end
  end
end

function LiveLeak.sanitize_json(d)
  d = d:gsub('\r\n',''):gsub('%s','') -- Strip CRLFs and wspace
  d = d:gsub('%+?%w+%(.-%)?,', ',')   -- Remove function calls
  d = d:gsub('%w+:,','')              -- Remove empty value names
  local J = require 'json'
  return J.decode(d)
end

function LiveLeak.to_id(s)
  if s.container then
    local r = {s.container, s.video.encoding, s.video.height .. 'p'}
    s.id = table.concat(r, '_')
  end
  return s
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
