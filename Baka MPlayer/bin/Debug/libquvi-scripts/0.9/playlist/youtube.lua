-- libquvi-scripts
-- Copyright (C) 2012-2013  Toni Gundogdu <legatvs@gmail.com>
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

local YouTube = {} -- Utility functions unique to this script

-- Identify the playlist script.
function ident(qargs)
  return {
    domains = table.concat({'youtube.com'}, ','),
    can_parse_url = YouTube.can_parse_url(qargs)
  }
end

-- Parse playlist properties.
function parse(qargs)

  qargs.id = qargs.input_url:match('list=([%w_-]+)')
  if #qargs.id <16 then
    error('no match: playlist ID')
  end

  local Y = require 'quvi/youtube'
  local U = require 'socket.url'
  local L = require 'quvi/lxph'
  local P = require 'lxp.lom'

  local max_results = 25
  local start_index = 1

  qargs.media = {}
  local r = {}

  repeat -- Get the entire playlist.
    local u = YouTube.config_url(qargs, U, start_index, max_results)
    local c = quvi.http.fetch(u).data

    local x = P.parse(c)
    YouTube.chk_error_resp(x)

    YouTube.parse_thumb_url(qargs, L, x)
    YouTube.parse_title(qargs, L, x)

    local n = YouTube.parse_media_urls(qargs, L, x)
    start_index = start_index + n
  until n == 0

  return qargs
end

--
-- Utility functions
--

function YouTube.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('youtube%.com$')
       and t.path   and t.path:lower():match('^/playlist$')
       and t.query  and t.query:lower():match('^list=[%w_-]+')
  then
    return true
  else
    return false
  end
end

function YouTube.config_url(qargs, U, start_index, max_results)
  local u = U.parse(qargs.input_url)
  local t = { -- Refer to http://is.gd/0msY8X
    u.scheme, '://gdata.youtube.com/feeds/api/playlists/',
    qargs.id, '?v=2', '&start-index=', start_index,
    '&max-results=', max_results, '&strict=true'
  }
  return table.concat(t)
end

function YouTube.entry_avail(x)
  --[[
  -- app:control may contain, for example:
  --  yt:state name='restricted' reasonCode='private'
  ]]--
  for i=1, #x do
    if x[i].tag == 'app:control' then return false end
  end
  return true
end

function YouTube.parse_entry(qargs, L, x, i, r)
  for j=1, #x[i] do
    if x[i][j].tag == 'link' then
      if x[i][j].attr['rel'] == 'alternate' then
        local t = {
          title = L.find_first_tag(x[i], 'title')[1],
          duration_ms = YouTube.parse_duration(L, x[i]),
          url = x[i][j].attr['href']
        }
        table.insert(qargs.media, t)
      end
    end
  end
end

function YouTube.parse_media_urls(qargs, L, x)
  if not x then return 0 end
  local n = 0
  for i=1, #x do
    if x[i].tag == 'entry' then
      if YouTube.entry_avail(x[i]) then
        YouTube.parse_entry(qargs, L, x, i, r)
      end
      n = n+1
    end
  end
  return n
end

function YouTube.chk_error_resp(t)
  if not t then return end
  local r = {}
  for i=1, #t do
    if t[i].tag == 'error' then
      for j=1, #t[i] do
        if t[i][j].tag == 'domain' then
          r.domain = t[i][j][1]
        end
        if t[i][j].tag == 'code' then
          r.code = t[i][j][1]
        end
        if t[i][j].tag == 'internalReason' then
          r.reason = t[i][j][1]
        end
        if t[i][j].tag == 'location' then -- Ignores 'type' attribute.
          r.location = t[i][j][1]
        end
      end
    end
  end
  if #r >0 then
    local m
    for k,v in pairs(r) do
      m = m .. string.format("%s=%s ", k,v)
    end
    error(m)
  end
end

function YouTube.parse_title(qargs, L, x)
  if not qargs.title then
    qargs.title = L.find_first_tag(x, 'title')[1]
  end
end

function YouTube.parse_thumb_url(qargs, L, x)
  if qargs.thumb_url then return end
  local g = L.find_first_tag(x, 'media:group')
  for i=1, #g do
    if g[i].tag == 'media:thumbnail' then
      if g[i].attr['yt:name'] == 'hqdefault' then
        qargs.thumb_url = g[i].attr['url']
        break
      end
    end
  end
end

function YouTube.parse_duration(L, x)
  local g = L.find_first_tag(x, 'media:group')
  local d = L.find_first_tag(g, 'yt:duration')
  return tonumber(d.attr['seconds']) * 1000
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
