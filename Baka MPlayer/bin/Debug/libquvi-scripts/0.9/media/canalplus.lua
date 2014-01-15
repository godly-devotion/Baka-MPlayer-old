-- libquvi-scripts
-- Copyright (C) 2013  Mohamed El Morabity <melmorabity@fedoraproject.org>
-- Copyright (C) 2012  Paul Kocialkowski <contact@paulk.fr>

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

local CanalPlus = {} -- Utility functions unique to to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = CanalPlus.can_parse_url(qargs),
    domains = table.concat({'canalplus.fr', 'd8.tv', 'd17.tv'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local J = require 'json'

  local p = quvi.http.fetch(qargs.input_url).data

  -- Make mandatory: needed to fetch the video data JSON.
  qargs.id = p:match('videoId%s*=%s*"?(%d+)"?')
               or error('no match: media ID')

  -- Get the channel ID.
  local channel = p:match('src="http://player%.canalplus%.fr/common/js/'
                           .. 'canalPlayer%.js%?param=(.-)"')
                                 or error('no match: channel ID')

  -- Fetch the video data JSON.
  local t = {
     'http://service.canal-plus.com/video/rest/getVideos/', channel, '/',
     qargs.id, '?format=json'
  }

  local c = quvi.http.fetch(table.concat(t)).data
  j = J.decode(c)

  qargs.thumb_url = j['MEDIA']['IMAGES']['GRAND']

  qargs.title = j['INFOS']['TITRAGE']['TITRE']

  qargs.streams = CanalPlus.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function CanalPlus.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and (t.host:lower():match('canalplus%.fr$')
                         or t.host:lower():match('d8%.tv$')
                         or t.host:lower():match('d17%.tv$'))
       and t.path   and t.path:lower():match('/pid%d+') then
    return true
  else
    return false
  end
end

function CanalPlus.iter_streams(j)
  local S = require 'quvi/stream'

  local d = j['MEDIA']['VIDEOS']
  local r = {}

  for q, v in pairs(d) do
    if v ~= ''
      -- Some streams have denied accessed, just ignore them.
      and q ~= 'HDS' and q ~= 'HLS' and q ~= 'IPHONE' and q ~= 'IPAD' then
      local t = S.stream_new(v)
      t.nostd = { quality = q }  -- Ignored by libquvi.
      t.id = CanalPlus.to_id(t)
      table.insert(r, t)
    end
  end

  if #r >1 then
    CanalPlus.ch_best(S, r)
  end

  return r
end

function CanalPlus.ch_best(S, t)
  -- Available video qualities.
  -- HD         -> High definition
  -- HAUT_DEBIT -> Broadband access
  -- BAS_DEBIT  -> Dial-up access
  -- MOBILE     -> Mobile access
  local q = { HD = 4, HAUT_DEBIT = 3, BAS_DEBIT = 2, MOBILE = 1 }
  local r = t[1]
  r.flags.best = true
  for _,v in pairs(t) do
    if q[v.nostd.quality] > q[r.nostd.quality] then
      r = S.swap_best(r, v)
    end
  end
end

function CanalPlus.to_id(t)
  return t.nostd.quality:lower()
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
