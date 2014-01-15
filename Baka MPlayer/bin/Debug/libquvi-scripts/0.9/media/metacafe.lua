-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Lionel Elie Mamane <lionel@mamane.lu>
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

local Metacafe = {} -- Utility functions unique to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Metacafe.can_parse_url(qargs),
    domains = table.concat({'metacafe.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  if Metacafe.is_affiliate(qargs) then
    return qargs
  end

  local p = quvi.http.fetch(qargs.input_url).data

  local v = p:match('name="flashvars" value="(.-)"')
              or error('no match: flashvars')

  qargs.thumb_url = p:match('"og:image"%s+content="(.-)"') or ''

  local U = require 'socket.url'
  local T = require 'quvi/util'

  local r = T.decode(U.unescape(v))

  qargs.streams = Metacafe.iter_streams(U, r)

  qargs.duration_ms = tonumber(r['duration'] or 0) * 1000

  qargs.title = r['title'] or ''

  qargs.id = r['itemID'] or ''

  return qargs
end

--
-- Utility functions
--

function Metacafe.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('metacafe%.com$')
       and t.path   and (t.path:lower():match('^/watch/%d+/')
                          or t.path:lower():match('^/watch/yt-[^/]+/'))
  then
    return true
  else
    return false
  end
end

function Metacafe.is_affiliate(qargs)
  local id = qargs.input_url:match('/watch/yt%-([^/]+)/') -- YouTube.
  if id then
    qargs.goto_url = table.concat({'http://youtu.be/',id})
  end
  return qargs.goto_url ~= nil
end

function Metacafe.iter_streams(U, r)
  local S = require 'quvi/stream'
  local J = require 'json'

  local j = J.decode(r['mediaData'])
  local t = {}

  for k,_ in pairs(j) do
    local u = U.parse(j[k]['mediaURL'] or error('no match: media stream URL'))

    local q = {} -- Construct the URL query from the given args.
    for _,v in pairs(j[k]['access'][1]) do
      table.insert(q,v)
    end
    u.query = table.concat(q, '=')

    local s = S.stream_new(U.build(u)) -- Build stream URL from these elems.
    Metacafe.to_id(s,k)

    table.insert(t,s)
  end

  local r = {} -- Reverse the stream order, SD should be first ('default').
  for i = #t,1,-1 do
    table.insert(r, t[i])
  end

  if #r >1 then
    Metacafe.ch_best(S, r)
  end

  return r
end

function Metacafe.ch_best(S, t)
  t[#t].flags.best = true -- Make the last stream the best one.
end

function Metacafe.to_id(s,k)
  -- Lack of a better scheme: reuse the internal IDs with minor tweaks.
  s.id = k:gsub('(%l)(%u)','%1_%2'):lower()
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
