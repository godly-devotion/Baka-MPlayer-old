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

local a = {
-- 5 standard entities:
  {from='&amp;', to='&'},
  {from='&lt;',  to='<'},
  {from='&gt;',  to='>'},
  {from='&quot;',to='"'},
  {from='&apos;',to="'"},
}

-- Standard HTML entities to ASCII.
function M.convert_html(s)
  for _,t in pairs(a) do
    s = s:gsub(t.from, t.to)
  end
  -- Numeric HTML entities to UTF8.
  local H = require 'quvi/html'
  return (s:gsub('&#(%d+);', H.to_utf8))
end

--[[
function M.foo()
  local s = 'foo &amp;&quot;bar &#220;&apos;'
  print(M.convert_html(s))
end
package.path = package.path .. ';../?.lua'
M.foo()
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
