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

local M = {}

--[[
Convert a string to a timestamp.
Parameters:
  s .. String to convert
Returns:
  Converted string.
]]--
function M.to_timestamp(s) -- Based on <http://is.gd/ee9ZTD>
  local p = "%a+, (%d+) (%a+) (%d+) (%d+):(%d+):(%d+)"

  local d,m,y,hh,mm,ss = s:match(p)
  if not d then error('no match: date') end

  local MON = {Jan=1, Feb=2, Mar=3, Apr=4, May=5, Jun=6, Jul=7, Aug=8,
               Sep=9, Oct=10, Nov=11, Dec=12}

  local m = MON[m]
  local offset = os.time() - os.time(os.date("!*t"))

  return os.time({day=d,month=m,year=y,
                  hour=hh,min=mm,sec=ss}) + offset
end

--[[
Convert seconds to a timecode string.
Parameters:
  s           .. Seconds
  hours_only  .. true=if seconds >=24h then return hours only
Returns:
  Timecode string.
]]--
function M.to_timecode_str(s, hours_only)
  if hours_only and s >= 86400 then -- 24h
    return string.format('%d hours', (s/3600)%60)
  else
    return string.format('%02d:%02d:%02d', (s/3600)%60, (s/60)%60, s%60)
  end
end

--[[
Convert seconds to a timecode dictionary.
Parameters:
  s .. Seconds
Returns:
  A dictionary containing the hours (hh), the minutes (mm) and the
  seconds (ss).
]]--
function M.to_timecode(s)
  return {hh=(s/3600)%60, mm=(s/60)%60, ss=s%60}
end

--[[
Convert a timecode string to a timecode dictionary.
Parameters:
  s .. Timecode string in format "%02d:%02d:%02d"
Returns:
  A dictionarary (see `to_timecode' above).
]]--
function M.from_timecode_str(s)
  local hh,mm,ss = s:match('(%d+)%:(%d+)%:(%d+)')
  return {
    hh = tonumber(hh or 0)*3600,
    mm = tonumber(mm or 0)*60,
    ss = tonumber(ss or 0)
  }
end

--[[
Convert a timecode string to seconds.
Parameters:
  s .. Timecode string in format "%02d:%02d:%02d"
Returns:
  Number of seconds.
]]--
function M.timecode_str_to_s(s)
  local tc = M.from_timecode_str(s)
  return tc.hh + tc.mm + tc.ss
end

-- Uncomment to test.
--[[
package.path = package.path .. ';../?.lua'

local t = {
  {tc='00:03:25', s=205},
  {tc='12:03:25', s=43405},
  {tc='25:03:25', s=90205}
}

local n = 0
for k,v in pairs(t) do
  local s = M.timecode_str_to_s(v.tc)
  if s ~= v.s then
    local r = {
      string.format('\n   input: %s (#%d)', v.tc, n),
      string.format('expected: %d, got %d', v.s, s)
    }
    print(table.concat(r,'\n'))
    n = n+1
  end
  local tc = M.to_timecode_str(s)
  if tc ~= v.tc then
    local r = {
      string.format('\n   input: %s (#%d)', s, n),
      string.format('expected: %d, got %d', v.tc, tc)
    }
    print(table.concat(r,'\n'))
    n = n+1
  end
end
print((n == 0) and 'All tests OK' or ('\nerrors: '..n))
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
