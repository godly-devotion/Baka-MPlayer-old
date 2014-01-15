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

local YouTube = {} -- Utility functions unique to this script

-- <http://en.wikipedia.org/wiki/YouTube#Quality_and_codecs>

-- Identify the script.
function ident(qargs)
  local Y = require 'quvi/youtube'
  return Y.ident(qargs)
end

-- Parse media properties.
function parse(qargs)
  return YouTube.parse_properties(qargs)
end

--
-- Utility functions
--

-- Make a new get_view_info request.
function YouTube.gvi_request_new(qargs, input_url)
  local U = require 'socket.url'
  local u = U.parse(input_url)

  qargs.id = u.query:match('v=([%w-_]+)') or error('no match: media ID')

  local t = {
    u.scheme, '://www.youtube.com/get_video_info?', 'video_id=', qargs.id,
    '&el=detailpage', '&ps=default', '&gl=US', '&hl=en'
  }

  local T = require 'quvi/util'
  local r = table.concat(t)

  local c = T.decode(quvi.http.fetch(r).data)
  local r = c['reason']

  if r and #r then
    error(string.format("%s (errorcode=%s)",
                          T.unescape(r), c['errorcode']))
  end
  return c, U, T, u.scheme
end

-- Parse the video info from the server.
function YouTube.parse_properties(qargs)
  local Y = require 'quvi/youtube'
  local u = Y.normalize(qargs.input_url)

  local c,U,T,scheme = YouTube.gvi_request_new(qargs, u)

  qargs.duration_ms = (c['length_seconds'] or 0)*1000 -- to ms
  qargs.thumb_url = T.unescape(c['thumbnail_url'] or '')

  qargs.title = T.unescape(c['title'] or '')
  qargs.streams = YouTube.iter_streams(c, U, T, scheme)

  YouTube.append_begin_param(qargs, U)
  return qargs
end

-- Append the &begin parameter to the media stream URL.
function YouTube.append_begin_param(qargs, U)
  local m,s = qargs.input_url:match('t=(%d?%d?m?)(%d%d)s')
  m = tonumber(((m or ''):gsub('%a',''))) or 0
  s = tonumber(((s or ''):gsub('%a',''))) or 0
  local ms = (m*60000) + (s*1000)
  if ms >0 then -- Rebuild each stream URL with the 'begin' parameter.
    for i,v in ipairs(qargs.streams) do
      local u = U.parse(qargs.streams[i].url)
      u.query = table.concat({u.query, '&begin=', ms})
      qargs.streams[i].url = U.build(u)
    end
    qargs.start_time_ms = ms
  end
end

-- Return a new media stream URL.
function YouTube.stream_url_new(d, U, T, scheme)
  local u = U.parse(T.unescape(d['url']))
  --
  -- The service returns only HTTP media stream URLs even if the media
  -- properties were requested over HTTPS. Forcing HTTPS will only
  -- result in HTTP/403. (2013-09-11)
  --
  --u.scheme = scheme -- Uncomment to use the input URL scheme
  --
  if d['sig'] then
    local s = table.concat({'&signature=', T.unescape(d['sig'])})
    u.query = table.concat({u.query, s})
  end
  return U.build(u) -- Rebuild the stream URL.
end

-- Iterate the available streams.
function YouTube.iter_streams(config, U, T, scheme)

  -- stream_map: holds many of the essential properties.
  local v = 'url_encoded_fmt_stream_map'
  local stream_map = T.unescape(config[v]
                      or error(string.format('no match: %s', v)))
                        .. ','

  local smr = {}
  for d in stream_map:gmatch('([^,]*),') do
    local d = T.decode(d)
    if d['url'] then -- Found media stream URL.
      local ct = T.unescape(d['type'])
      local v_enc, a_enc = ct:match('codecs="([%w.]+),%s+([%w.]+)"')
      local t = {
        container = (ct:match('/([%w-]+)')):gsub('x%-', ''),
        url = YouTube.stream_url_new(d, U, T, scheme),
        quality = d['quality'],
        v_enc = v_enc,
        a_enc = a_enc
      }
      local itag = d['itag']
      smr[itag] = t
    end
  end

  -- fmt_list: stores the video resolutions.
  local fmtl = T.unescape(config['fmt_list'] or error('no match: fmt_list'))

  local S = require 'quvi/stream'
  local r = {}

  for itag,w,h in fmtl:gmatch('(%d+)/(%d+)x(%d+)') do
    local smri = smr[itag]
    local t = S.stream_new(smri.url)

    t.video.encoding = smri.v_enc or ''
    t.audio.encoding = smri.a_enc or ''

    t.container = smri.container or ''

    t.video.height = tonumber(h)
    t.video.width = tonumber(w)

    t.id = YouTube.to_id(t, itag, smri)
    table.insert(r, t)
  end

  if #r >1 then -- Pick one stream as the 'best' quality.
    YouTube.ch_best(S, r)
  end
  return r
end

-- Choose the stream with the highest video height property as the best.
function YouTube.ch_best(S, t)
  local r = t[1] -- Make the first one the 'best' by default.
  r.flags.best = true
  for _,v in pairs(t) do
    if v.video.height > r.video.height then
      r = S.swap_best(r, v)
    end
  end
end

-- Return an ID for a stream.
function YouTube.to_id(t, itag, smri)
  return string.format("%s_%s_i%02d_%sp",
          smri.quality, t.container, itag, t.video.height)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
