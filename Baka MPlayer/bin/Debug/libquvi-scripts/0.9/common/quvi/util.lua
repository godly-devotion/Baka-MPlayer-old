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

local M = {}

--[[
Check whether a string A ends with string B.
Parameters:
  a .. String A
  b .. String B
Returns:
  true if string A ends with string B.
]]--
function M.ends_with(a, b) -- http://lua-users.org/wiki/StringRecipes
  return a:sub(-#b) == b
end

--[[
Compare quality properties of two media entities. Compares the height, then
the width, followed by the bitrate property comparison (if it is set).
Parameters:
  a .. Media entity A
  b .. Media entity B
Returns:
  true if entity A is the higher quality, otherwise false.
]]--
function M.is_higher_quality(a, b)
  if a.height > b.height then
    if a.width > b.width then
      if a['bitrate'] then -- Optional
        if a.bitrate > b.bitrate then return true end
      else
        return true
      end
    end
  end
  return false
end

--[[
Compare quality properties of two media entities. Compares the height, then
the width, followed by the bitrate property comparison (if it is set).
Parameters:
  a .. Media entity A
  b .. Media entity B
Returns:
  true if entity A is the lower quality, otherwise false.
]]--
function M.is_lower_quality(a, b)
  if a.height < b.height then
    if a.width < b.width then
      if a['bitrate'] then -- Optional
        if a.bitrate < b.bitrate then return true end
      else
        return true
      end
    end
  end
  return false
end

--[[
Decode a string.
Parameters:
  s .. String to decode
Returns:
  Decoded string.
]]--
function M.decode(s) -- http://www.lua.org/pil/20.3.html
  r = {}
  for n,v in s:gmatch("([^&=]+)=([^&=]+)") do
    n = M.unescape(n)
    r[n] = v
  end
  return r
end

--[[
Unescape a string.
Parameters:
  s .. String to unescape
Returns:
  Unescaped string
]]--
function M.unescape(s) -- http://www.lua.org/pil/20.3.html
  s = s:gsub('+', ' ')
  return (s:gsub('%%(%x%x)',
            function(h)
              return string.char(tonumber(h, 16))
            end))
end

--[[
Unescape slashed string.
Parameters:
  s .. String to unescape
Returns:
  Unescaped string
]]--
function M.slash_unescape(s)
  return (s:gsub('\\(.)', '%1'))
end

--[[
Trim a string removing leading and trailing whitespace.
Parameters:
  s .. String to trim
Returns:
  The trimmed string.
]]--
function M.trim(s)
  s = s:gsub('^%s+(.)', '%1')
  return s:gsub('(.)%s+$', '%1')
end

--[[
Tokenize a string.
Parameters:
  s   .. String to tokenize
  sep .. Separator
Returns:
  Tokenized items in a table.
]]--
function M.tokenize(s,sep) -- Based on http://lua-users.org/wiki/SplitJoin
  local sep, fields = sep or ':', {}
  local pattern = string.format('([^%s]+)', sep)
                    s:gsub(pattern, function(c) fields[#fields+1] = c
                  end)
  return fields
end

--[[
Escape Lua magic characters.
Parameters:
  s .. String to escape
Returns
  The escaped string.
]]--
function M.escape_magic(s)
  return (s:gsub('([()%%.%+%-*?[%]^$])', '%%%1'))
end

-- Uncomment to test
--[[
package.path = package.path .. ';../?.lua'
for _,v in pairs(M.tokenize('a,b,c,d,e',',')) do print(v) end
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
