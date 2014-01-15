-- libquvi-scripts
-- Copyright (C) 2012  Toni Gundogdu <legatvs@gmail.com>
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
Construct a dictionary containing the stream properties.
These, except the URL, are set to empty (or 0) values.
Parameters:
  url .. Media stream URL
Returns:
  A dictionary with the default values
]]--
function M.stream_new(url)
  local r = {
    url = url or '',
    fmt_id = '',
    video = {
      bitrate_kbit_s = '',
      encoding = '',
      height = 0,
      width = 0
    },
    audio = {
      bitrate_kbit_s = '',
      encoding = ''
    },
    flags = {
      best = false
    }
  }
  return r
end

function M.swap_best(a,b)
  a.flags.best, b.flags.best = b.flags.best, a.flags.best
  return b
end

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
