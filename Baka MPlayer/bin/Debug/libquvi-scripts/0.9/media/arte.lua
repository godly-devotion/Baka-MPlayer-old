-- libquvi-scripts
-- Copyright (C) 2013  Mohamed El Morabity <melmorabity@fedoraproject.org>
-- Copyright (C) 2012-2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2011  Raphaël Droz <raphael.droz+floss@gmail.com>
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

-- NOTE: Most videos expire some (7?) days after their original broadcast

local Arte = {} -- Utility functions unique to to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Arte.can_parse_url(qargs),
    domains = table.concat({'arte.tv'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local J = require 'json'

  -- Fetch the video data JSON.
  local p = quvi.http.fetch(qargs.input_url).data
  local u = p:match('arte_vp_url="(.-/ALL%.json)"')
              or error('no match: config URL')
  local c = quvi.http.fetch(u).data
  local j = J.decode(c)

  -- Check the video expiration date.
  local d = j['videoJsonPlayer']['VRU']
  if d and Arte.has_expired(d) then
    error('media no longer available (expired)')
  end

  qargs.id = j['videoJsonPlayer']['VPI']

  qargs.title = j['videoJsonPlayer']['VTI']

  qargs.thumb_url = j['videoJsonPlayer']['programImage']

  qargs.duration_ms = j['videoJsonPlayer']['videoDurationSeconds'] * 1000

  qargs.streams = Arte.iter_streams(j)

  return qargs
end

--
-- Utility functions
--

function Arte.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('www.arte%.tv$')
       and t.path   and t.path:lower():match('^/guide/%w+/')
  then
    return true
  else
    return false
  end
end

function Arte.iter_streams(j)
  local S = require 'quvi/stream'

  local d = j['videoJsonPlayer']['VSR']
  local r = {}

  for _, v in pairs(d) do

    local s = v['streamer'] -- Handle available RTMP streams.
                and table.concat({v['streamer'], 'mp4:', v['url']})
                 or v['url']

    local t = S.stream_new(s)

    t.video = {
      bitrate_kbit_s = v['bitrate'],
      width = v['width'],
      height = v['height']
    }

    -- Save the property values that may be used later, these depend
    -- on the language setting. Many of these are the so called
    -- "optional media properties".  The 'nostd' dictionary is used
    -- only by this script. libquvi ignores it completely.

    t.nostd = {
      quality = v['quality'],
      -- versionProg is an ID corresponding to the video language.
      -- versionProg == 1 -> default language version, matching the video URL
      --                     language
      -- versionProg == 2 -> alternate language version
      -- versionProg == 3 -> original version with default language subtitles
      -- versionProg == 8 -> default language version with subtitle for
      --                     hard-of-hearing
      versionProg = tonumber(v['versionProg']),
      versionCode = v['versionCode'],
      mediaType = v['mediaType']
    }
    t.id = Arte.to_id(t)

    table.insert(r, t)
  end

  if #r >1 then
    Arte.ch_best(S, r)
  end

  return r
end

function Arte.ch_best(S, t, l)
  local r = t[1]
  r.flags.best = true
  for _,v in pairs(t) do
     if Arte.is_best_stream(v, r) then
      r = S.swap_best(r, v)
    end
  end
end

function Arte.has_expired(e)
  local d, mo, y, h, m, sc = e:match('(%d+)/(%d+)/(%d+) (%d+):(%d+):(%d+)')

  local t = os.time({ year = y, month = mo, day = d,
                      hour = h, min = m, sec = sc })

  return (t - os.time()) < 0
end

-- Return an ID for a stream.
function Arte.to_id(t)
  -- Streaming protocol
  local s = (t.nostd.mediaType == '') and 'http' or t.nostd.mediaType

  return string.format("%s_%s_%s", t.nostd.quality, s, t.nostd.versionCode)
           :gsub('%s?%-%s?', '_'):lower()
end

-- Check which stream of two is the "best".
function Arte.is_best_stream(v1, v2)
  -- Select stream in default language version rather than in alternate.
  if v2.nostd.versionProg == 2
       and v1.nostd.versionProg ~= 2 then
    return true
  end

  -- Select stream in default language version rather than in original
  -- version/with subtitles for hard-of-hearing.
  if v1.nostd.versionProg < v2.nostd.versionProg then
    return true
  end

  return v1.video.height > v2.video.height
           or (v1.video.height == v2.video.height
                 and v1.video.bitrate_kbit_s > v2.video.bitrate_kbit_s)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
