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

local Majestyc = {} -- Utility functions specific to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Majestyc.can_parse_url(qargs),
    domains = table.concat({'tube.majestyc.net'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  local v = t.query:match('v=([%w-_]+)') or error('no match: media ID')
  qargs.goto_url = table.concat({'http://youtu.be/',v})
  return qargs
end

--
-- Utility functions
--

function Majestyc.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('tube%.majestyc%.net$')
       and t.query  and t.query:lower():match('v=[%w-_]+')
       and t.path   and t.path:lower():match('^/')
  then
    return true
  else
    return false
  end
end
