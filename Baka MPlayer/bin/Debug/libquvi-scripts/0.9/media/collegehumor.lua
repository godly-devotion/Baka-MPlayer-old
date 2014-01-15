-- libquvi-scripts
-- Copyright (C) 2012,2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2010-2011  Lionel Elie Mamane <lionel@mamane.lu>
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

local CollegeHumor = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = CollegeHumor.can_parse_url(qargs),
    domains = table.concat({'collegehumor.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  local u = p:match('source type=.- src="(.-)"')
              or error('no match: media stream URL')

  if u:match('%.%w+$') then -- Stream URL ends to a file extension?
    local S = require 'quvi/stream'
    qargs.streams = {S.stream_new(u)}
  else
    qargs.goto_url = u -- Affiliate content.
    return qargs       -- Pass the new page URL back to the library.
  end

  qargs.duration_ms =
    tonumber(p:match('"video:duration" content="(%d+)"') or 0) *1000

  qargs.thumb_url = p:match('"og:image" content="(.-)"') or ''

  qargs.title = p:match('"og:title" content="(.-)"') or ''

  qargs.id = qargs.input_url:match('/video/(%d+)/')
                or qargs.input_url:match('/embed/(%d+)/') or ''

  return qargs
end

--
-- Utility functions
--

function CollegeHumor.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('collegehumor%.com$')
       and t.path   and (t.path:lower():match('^/video/%d+/')
                          or t.path:lower():match('^/embed/%d+/'))
  then
    return true
  else
    return false
  end
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
