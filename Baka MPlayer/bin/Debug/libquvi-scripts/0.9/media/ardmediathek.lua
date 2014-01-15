-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2013  Thomas Weißschuh <thomas@t-8ch.de>
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

local ArdMediathek = {} -- Utility functions unique to to this script.

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = ArdMediathek.can_parse_url(qargs),
    domains = table.concat({'ardmediathek.de'}, ',')
  }
end

-- Parse the media properties.
function parse(qargs)

  local p = quvi.http.fetch(qargs.input_url).data

  ArdMediathek.chk_avail(p)

  qargs.thumb_url = p:match('property="og:image" content="(.-)"') or ''

  qargs.id = qargs.input_url:match('documentId=(%d+)') or ''

  qargs.title = ArdMediathek.get_title(p)

  qargs.streams = ArdMediathek.iter_streams(p)

  return qargs
end

--
-- Utility functions
--

function ArdMediathek.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('ardmediathek%.de$')
       and t.query  and t.query:match('^documentId=%d+$')
  then
    return true
  else
    return false
  end
end

function ArdMediathek.get_title(p)
  return p:match('<meta property="og:title" content="([^"]*)')
                 :gsub('%s*%- %w-$', '') -- remove name of station
                 :gsub('%s*%(FSK.*', '') -- remove FSK nonsense
                 or ''
end

function ArdMediathek.chk_avail(p)
  if p:match('<title>ARD Mediathek %- Fehlerseite</title>') then
    error('an invalid media URL -- the media is no longer available?')
  end
  -- Some videos are only scrapable at certain times
  local s = 'Der Clip ist deshalb nur von (%d%d?) bis (%d%d?) Uhr verfügbar'
  local from,to = p:match(s)
  if from and to then
    local t = {'media available from ', from, ':00 to ', to, ':00 CET only'}
    error(table.concat(t))
  end
end

function ArdMediathek.to_container(s)
  return (s:match('^(...):') or s:match('%.(...)$') or ''):lower()
end

function ArdMediathek.to_encoding(s)
  return (s:match('%.(%w+)%.%w+$') or ''):lower()
end

function ArdMediathek.to_resolution(s)
  local w,h = s:match('_(%d+)x(%d+)[_%.]')
  return tonumber(w or 0), tonumber(h or 0)
end

function ArdMediathek.to_quality(s)
  local q = (s:match('%.web(%w)%.') or s:match('%.(%w)%.')
              or s:match('[=%.]Web%-(%w)') -- ".webs", ".s" or "Web-S"
                or ''):lower()
  if #q then
    for k,v in pairs({s='ld', m='md', l='sd', xl='hd'}) do
      if q == k then
        return v
      end
    end
  end
  return q
end

function ArdMediathek.iter_streams(p)
  local S = require 'quvi/stream'
  local s = 'mediaCollection%.addMediaStream'
              .. '%(0, (%d+), "(.-)", "(.-)", "%w+"%);'
  local r = {}
  for id,pre,suf in p:gmatch(s) do
    local u = table.concat({pre,suf})
    u = u:match('^(.-)?') or u  -- remove the query string

    local t = S.stream_new(u)

    -- Available only for some videos.
    t.video.width, t.video.height = ArdMediathek.to_resolution(u)

    t.container = ArdMediathek.to_container(u)
    t.quality = ArdMediathek.to_quality(u)

    -- Ignored by libquvi.
    t.nostd = { internal_id=id }

    -- Without the 'quality' the stream ID would incomplete.
    if #t.quality ==0 then
      t.quality = ArdMediathek.to_encoding(u)
    else
      t.video.encoding = ArdMediathek.to_encoding(u)
    end

    t.id = ArdMediathek.to_id(t)

    table.insert(r,t)
  end

  if #r >1 then
    ArdMediathek.ch_best(S, r)
  end

  return r
end

function ArdMediathek.ch_best(S, t)
  local r = t[#t] -- Make the last the best quality.
  r.flags.best = true
end

function ArdMediathek.to_id(t)
  return string.format('%s_%s_i%02d%s%s',
            (#t.quality >0) and t.quality or 'sd',
            t.container, t.nostd.internal_id,
            (#t.video.encoding >0) and ('_'..t.video.encoding) or '',
            (t.video.height >0) and ('_'..t.video.height..'p') or '')
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
