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

local YouTube = {} -- Utility functions unique to this script

-- Identify the script.
function ident(qargs)
  local Y = require 'quvi/youtube'
  return Y.ident(qargs)
end

-- Parse subtitle properties.
function parse(qargs)
  local Y = require 'quvi/youtube'
  local C = require 'quvi/const'

  local u = Y.normalize(qargs.input_url)
  local v = u:match('v=([%w-_]+)') or error('no match: media ID')

  qargs.subtitles = {}

  YouTube.tts_get(qargs, v, u, C)
  YouTube.cc_get(qargs, v, C)

  return qargs
end

--
-- Utility functions
--

-- Extract the TTS (text-to-speech, or transcript).
function YouTube.tts_get(qargs, v, u, C)
  local u_fmt = "%s&tlang=%s&type=trackformat=1,&lang=en&kind=asr"
  local p = quvi.http.fetch(u).data

  local tts_url = p:match('[\'"]TTS_URL[\'"]:%s+[\'"](.-)[\'"]')
  if not tts_url then return end

  p = nil

  tts_url = tts_url:gsub('\\u0026','&')
  local U = require 'quvi/util'
  tts_url = U.slash_unescape(tts_url)
  tts_url = U.unescape(tts_url)

  local langs = tts_url:match('asr_langs=(.-)&')
  if not langs then return end

  local r = {}
  for c in langs:gmatch('(%w+)') do
    table.insert(r, {
      url = string.format(u_fmt, tts_url, c),
      translated = '',
      id = 'tts_'..c,
      original = '',
      code = c
    })
  end

  table.insert(qargs.subtitles, {format=C.sif_tt, type=C.st_tts, lang=r})
end

-- Return a new timed-text track URL.
function YouTube.tt_track_new(scheme, v, name, lang)
  local t = {
    scheme, '://youtube.com/api/timedtext?hl=en&type=track',
    '&v=', v, '&name=', name, '&lang=', lang
  }
  return table.concat(t)
end

-- Return a new timed-text list URL.
function YouTube.tt_list_new(scheme, v)
  local t = {scheme, '://video.google.com/timedtext?hl=en&type=list&v=', v}
  return table.concat(t)
end

-- Extract the CC (closed-captions) data.
function YouTube.cc_get(qargs, v, C)
  local U = require 'socket.url'
  local u = U.parse(qargs.input_url)

  local l = YouTube.tt_list_new(u.scheme, v)
  local x = quvi.http.fetch(l).data

  local L = require 'lxp.lom'
  local t = L.parse(x)

  local r = {}
  for i=1, #t do
    if t[i].tag == 'track' then
      local lang = t[i].attr['lang_code']
      if lang then
        local name = t[i].attr['name'] or ''
        table.insert(r, {
          translated = t[i].attr['lang_translated'] or '',
          url = YouTube.tt_track_new(u.scheme, v, name, lang),
          original = t[i].attr['lang_original'] or '',
          id = table.concat({'cc_', lang}),
          code = lang
        })
      end
    end
  end
  table.insert(qargs.subtitles, {format=C.sif_tt, type=C.st_cc, lang=r})
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
