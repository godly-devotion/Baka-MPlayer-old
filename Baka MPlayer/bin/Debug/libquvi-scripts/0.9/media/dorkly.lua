-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
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

local Dorkly = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Dorkly.can_parse_url(qargs),
    domains = table.concat({'dorkly.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  if Dorkly.is_affiliate(qargs) then
    return qargs
  end

  -- Make mandatory: the ID is required to fetch the stream data
  qargs.id = qargs.input_url:match('/video/(%d+)/')
                or qargs.input_url:match('/embed/(%d+)/')
                  or error('no match: media ID')

  local t = {'http://www.dorkly.com/moogaloop/video/', qargs.id}
  local d = quvi.http.fetch(table.concat(t)).data

  local L = require 'quvi/lxph'
  local P = require 'lxp.lom'

  local x = P.parse(d)
  local v = L.find_first_tag(x, 'video')

  qargs.duration_ms = tonumber(L.find_first_tag(v, 'duration')[1])

  qargs.thumb_url = L.find_first_tag(v, 'thumbnail')[1]

  qargs.title = L.find_first_tag(v, 'caption')[1]

  qargs.streams = Dorkly.iter_streams(L, v)

  return qargs
end

--
-- Utility functions
--

function Dorkly.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('dorkly%.com$')
       and t.path   and (t.path:lower():match('^/video/%d+/')
                          or t.path:lower():match('^/embed/%d+/'))
  then
    return true
  else
    return false
  end
end

function Dorkly.is_affiliate(qargs)
  if not qargs.input_url:match('/embed/') then
    return false
  end

  local p = quvi.http.fetch(qargs.input_url).data

  local u = p:match('iframe.-src="(.-)"')
              or error('no match: iframe: src')

  if not u:match('^http:') then
    u = table.concat({'http:',u}) -- Fix URL if the scheme is missing.
  end

  qargs.goto_url = u
  return true
end

function Dorkly.iter_streams(L, v)
  local u = L.find_first_tag(v, 'file')[1]
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
