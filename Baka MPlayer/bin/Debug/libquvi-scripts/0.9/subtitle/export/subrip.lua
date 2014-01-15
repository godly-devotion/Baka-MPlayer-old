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

--[[
Notes
  * http://en.wikipedia.org/wiki/SubRip#SubRip_text_file_format
  * Uses comma (,) for a decimal separator
  * Uses CRLF, with LF line terminators
]]--

local SubRip = {format='srt'} -- Utility functions unique to this script

-- Identify the script.
function ident(qargs)
  return {
    can_export_data = (qargs.to_format == SubRip.format),
    export_format = SubRip.format
  }
end

-- Export data.
function export(qargs)
  local C = require 'quvi/const'
  if qargs.from_format == C.sif_tt then
    return SubRip.from_tt(qargs)
  else
    error(string.format('unsupported subtitle format: 0x%x',
            qargs.from_format))
  end
end

--
-- Utility functions
--

-- timed-text (tt) - YouTube uses this for both CCs and TTSes.
function SubRip.from_tt(qargs)

  local f = '%d\r\n%02d:%02d:%06.3f --> %02d:%02d:%06.3f\r\n%s\r\n\r\n'
  local E = require 'quvi/entity'
  local T = require 'quvi/time'
  local U = require 'quvi/util'
  local L = require 'lxp.lom'

  local x = quvi.http.fetch(qargs.input_url).data
  local t = L.parse(x)
  local r = {}

  local last_start = 0

  --
  -- NOTE: Building up a large string by concatenation will create a lot
  --       temporary strings burdening the Lua garbage collector. The
  --       Lua way is to put the strings into a table.
  --

  for i=1, #t do
    if t[i].tag == 'text' then
      local start = tonumber(t[i].attr['start'] or 0)
      local dur = tonumber(t[i].attr['dur'] or (start-last_start))
      local end_sec = tonumber(start) + dur

      local text = U.trim( E.convert_html(t[i][1]) )

      local start_tc = T.to_timecode(start)
      local end_tc = T.to_timecode(end_sec)

      local s = string.format(f, i, start_tc.hh, start_tc.mm, start_tc.ss,
                              end_tc.hh, end_tc.mm, end_tc.ss, text)

      -- Use comma for a decimal separator.
      table.insert(r, (s:gsub('(%d+)%.(%d+)', '%1,%2')))
      last_start = start
    end
  end
  qargs.data = table.concat(r)
  return qargs
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
