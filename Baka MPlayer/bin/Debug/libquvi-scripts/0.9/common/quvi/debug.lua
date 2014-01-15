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

function M.line()  return debug.getinfo(2, 'l').currentline end
function M.short() return debug.getinfo(2, 'S').short_src end
function M.file()  return debug.getinfo(2, 'S').source end
function M.func()  return debug.getinfo(2, 'n').name end

function M.say(...)
  local s = os.getenv("LIBQUVI_SCRIPTS_VERBOSE")
  if s and #s >0 then
    io.stderr:write(string.format('%s\n', string.format(...)))
  end
end

--[[
function M.foo()
  M.say('%s:%s: foo=%s', M.file(), M.line(), 1)
  M.say('bar')
end
M.foo()
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
