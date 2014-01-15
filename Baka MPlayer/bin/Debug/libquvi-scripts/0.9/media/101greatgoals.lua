-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  quvi project
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

-- Hundred and One Great Goals (aggregator)
local HaOgg = {} -- Utility functions specific to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = HaOgg.can_parse_url(qargs),
    domains = table.concat({'101greatgoals.com'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  qargs.goto_url = HaOgg.chk_self_hosted(p) or HaOgg.chk_embedded(p)
                    or error('unable to determine media source')

  return qargs
end

--
-- Utility functions
--

function HaOgg.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('101greatgoals%.com$')
       and t.path   and t.path:lower():match('^/gvideos/.+/$')
  then
    return true
  else
    return false
  end
end

function HaOgg.chk_self_hosted(p)
  --
  -- Previously referred to as the "self-hosted" media, although according
  -- to the old notes, these were typically hosted by YouTube.
  --    http://is.gd/EKKPy2
  --
  -- 2013-05-05: The contents of the URL no longer seems to contain the
  --             "file" value, see chk_embedded for notes; keep this
  --             function around for now
  --
  local d = p:match('%.setup%((.-)%)')
  if d then
    local s = d:match('"file":"(.-)"') or error('no match: file')
    if #s ==0 then
      error('empty media URL ("file")')
    end
    local U = require 'quvi/util'
    return (U.slash_unescape(U.unescape(s)))
  end
end

function HaOgg.chk_embedded(p)
  --
  -- 2013-05-05: Most of the content appears to be embedded from elsewhere
  --
  -- Instead of trying to check for each, parse the likely embedded source
  -- and pass it back to libquvi to find a media script that accepts the
  -- parsed (embedded) media URL.
  --
  local s = p:match('class="post%-type%-gvideos">(.-)</')
              or p:match('id="jwplayer%-1">(.-)</>')
                or error('unable to determine embedded source')

  s = s:match('value="(.-)"') or s:match('src="(.-)"')

  local U = require 'socket.url'
  local t = U.parse(s)
  if not t.scheme then -- Make sure the URL has a scheme.
    t.scheme = 'http'
  end
  return U.build(t)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
