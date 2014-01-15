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

local Vzaar = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Vzaar.can_parse_url(qargs),
    domains = table.concat({'vzaar.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local U = require 'socket.url'
  local r = Vzaar.normalize(U, qargs.input_url)
  local u = U.parse(r)

  -- Make ID mandatory: It is required to fetch
  qargs.id = u.path:match('^/videos/(%d+)$') or error('no match: media ID')

  local t = {u.scheme, '://view.vzaar.com/', qargs.id, '/player'}
  local p = quvi.http.fetch(table.concat(t)).data

  local d = p:match('data%-setup="(.-)"') or
              error('no match: data-setup')

  local E = require 'quvi/entity'
  local J = require 'json'

  local j = J.decode(E.convert_html(d))
  local l = j['playlist'][1] or error('no data: playlist')

  qargs.streams = Vzaar.iter_streams(l)

  qargs.thumb_url = l['thumb_url']

  qargs.title = l['title']

  return qargs
end

--
-- Utility functions.
--

function Vzaar.can_parse_url(qargs)
  local U = require 'socket.url'
  local u = Vzaar.normalize(U, qargs.input_url)
  local t = U.parse(u)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('^vzaar%.com$')
       and t.path   and t.path:lower():match('^/videos/%d+$')
  then
    return true
  else
    return false
  end
end

function Vzaar.normalize(U, url)
  -- Sanity checks.
  if not url then
    return url
  end

  local u = U.parse(url)

  if not u or not u.host or not u.path then
    return url
  end

  -- Shortened URLs (http://vzaar.tv/:id:).
  u.host = u.host:gsub('vzaar%.tv', 'vzaar.com')

  -- Embedded media URLs (http://view.vzaar.com/:id:/player).
  u.host = u.host:gsub('^view%.', '')
  u.path = u.path:gsub('/player', '')

  -- Fix path.
  u.path = u.path:gsub('^/(%d+)$', '/videos/%1')

  -- Rebuild and return the media URL.
  return U.build(u)
end

function Vzaar.iter_streams(l)
  local S = require 'quvi/stream'
  return {S.stream_new(l['video_content_url'])}
end

--[[
local function test_normalize()
  local test_cases = {
    {url='http://vzaar.tv/1020181',               -- Shortened URL.
     expect='http://vzaar.com/videos/1020181'},
    {url='http://view.vzaar.com/1020181/player',  -- Embedded URL.
     expect='http://vzaar.com/videos/1020181'},
    {url='http://vzaar.com/videos/1020181',       -- Media URL.
     expect='http://vzaar.com/videos/1020181'},
  }
  local U = require 'socket.url'
  local i,e = 0,0
  for _,v in pairs(test_cases) do
    local r = Vzaar.normalize(U, v.url)
    if r ~= v.expect then
      print(string.format('input: %s (#%s)\nexpected: %s\ngot: %s',
                          v.url, i, v.expect, r))
      e = e+1
    end
    i = i+1
  end
  print((e==0) and 'tests OK' or error('failed tests: '..e))
end
test_normalize()
]]--

-- vim: set ts=2 sw=2 tw=72 expandtab:
