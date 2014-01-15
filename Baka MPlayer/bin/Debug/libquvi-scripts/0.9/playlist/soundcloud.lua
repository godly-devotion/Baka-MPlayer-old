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

local Soundcloud = {} -- Utility functions unique to this script

-- Identify the playlist script.
function ident(qargs)
  return {
    domains = table.concat({'soundcloud.com'}, ','),
    can_parse_url = Soundcloud.can_parse_url(qargs)
  }
end

-- Parse playlist properties.
function parse(qargs)

  qargs.id, s = qargs.input_url:match('/([%w-_]+)/sets/([%w-_]+)/')
  if qargs.id and s then
    qargs.id = qargs.id .."_".. s
  end

  local p = quvi.http.fetch(qargs.input_url).data

  qargs.thumb_url = p:match('.+content="(.-)"%s+property="og:image"') or ''
  qargs.title = p:match('.+content="(.-)"%s+property="og:title"') or ''

  local m = 'class="info">.-href="(.-)"'
         .. '.-class="set%-track%-title".->(.-)<'
         .. '.-class="time">(.-)<'

  qargs.media = {}

  for u,t,d in p:gmatch(m) do
    local m,s = d:match('(%d+)%.(%d+)')
    local r = {
      duration_ms = ((tonumber(m or '0')*60) + tonumber(s or '0')) *1000,
      url = "http://soundcloud.com" ..u,
      title = t
    }
    table.insert(qargs.media, r)
  end

  return qargs
end

--
-- Utility functions
--

function Soundcloud.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('soundcloud%.com$')
       and t.path   and t.path:lower():match('^/.-/sets/[%w-_]+/$')
  then
    return true
  else
    return false
  end
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
