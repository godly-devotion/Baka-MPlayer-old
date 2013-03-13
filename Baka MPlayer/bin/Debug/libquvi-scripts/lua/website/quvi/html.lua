
-- libquvi-scripts v0.4.10
-- Copyright (C) 2011  Toni Gundogdu <legatvs@gmail.com>
--
-- This file is part of libquvi-scripts <http://quvi.sourceforge.net/>.
--
-- This library is free software; you can redistribute it and/or
-- modify it under the terms of the GNU Lesser General Public
-- License as published by the Free Software Foundation; either
-- version 2.1 of the License, or (at your option) any later version.
--
-- This library is distributed in the hope that it will be useful,
-- but WITHOUT ANY WARRANTY; without even the implied warranty of
-- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
-- Lesser General Public License for more details.
--
-- You should have received a copy of the GNU Lesser General Public
-- License along with this library; if not, write to the Free Software
-- Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
-- 02110-1301  USA
--

--
-- A modularized revision of <http://www.hpelbers.org/lua/utf8>
--
-- convert numeric html entities to utf8
-- example:  &#8364;    ->   €
--

local M    = {}
local char = string.char
 
function M.tail(n, k)
    local u, r=''
    for i=1,k do
        n,r = math.floor(n/0x40), n%0x40
        u = char(r+0x80) .. u
    end
    return u, n
end
 
function M.to_utf8(a)
    local n, r, u = tonumber(a)
    if n<0x80 then                        -- 1 byte
        return char(n)
    elseif n<0x800 then                   -- 2 byte
        u, n = M.tail(n, 1)
        return char(n+0xc0) .. u
    elseif n<0x10000 then                 -- 3 byte
        u, n = M.tail(n, 2)
        return char(n+0xe0) .. u
    elseif n<0x200000 then                -- 4 byte
        u, n = M.tail(n, 3)
        return char(n+0xf0) .. u
    elseif n<0x4000000 then               -- 5 byte
        u, n = M.tail(n, 4)
        return char(n+0xf8) .. u
    else                                  -- 6 byte
        u, n = M.tail(n, 5)
        return char(n+0xfc) .. u
    end
end
 
--
--for line in io.lines() do
--    out =  string.gsub(line, '&#(%d+);', to_utf8)
--    print(out)
--end
--

return M

-- vim: set ts=4 sw=4 tw=72 expandtab:
