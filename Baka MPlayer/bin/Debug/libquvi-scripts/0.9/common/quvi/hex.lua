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

-- Returns an array of the numerical codes of the hexadecimal string.
function M.to_arr(s)
  if not (#s % 2 ==0) then
    error(string.format('%s: invalid hexstring', s))
  end
  local B = require 'bit'
  local r = {}
  s:gsub('%x%x', function(n)
      table.insert(r, B.tobit(tonumber(n,16)))
    end)
  return r
end

-- Returns an array of the internal numerical codes of the characters.
function M.to_bytes(s)
  local r = {}
  s:gsub('.', function(c)
      table.insert(r, c:byte(1))
    end)
  return r
end

-- Returns a hexadecimal string created from the array of the internal
-- numberical codes of the characters.
function M.from_bytes(b)
  local B = require 'bit'
  local r = {}
  for _,v in pairs(b) do
    table.insert(r, B.tohex(v,2))
  end
  return table.concat(r)
end

-- Data to a hexadecimal string.
function M.to_hex(s)
  local t = type(s)
  if t == 'string' then
    return M.from_bytes(M.to_bytes(s))
  elseif t == 'table' then
    return M.from_bytes(s)
  else
    error(string.format('invalid type: %s', t))
  end
end

-- Returns a string of the internal numerical codes created from
-- the hexadecimal string or the array of numerical codes of the
-- hexadecimal string.
function M.to_str(s)
  local a,t = type(s)
  if type(s) == 'string' then
    a = M.to_arr(s)
  elseif type(s) == 'table' then
    a = s
  else
    error(string.format('invalid type: %s', t))
  end
  local r = {}
  for _,v in pairs(a) do
    table.insert(r, string.char(v))
  end
  return table.concat(r)
end

--[[
--
-- tests
--

function test_eq(t,r,e)
  if not (r == e) then error(string.format('%s: failed', t)) end
end

-- to_hex(string)
test_eq('test/to_hex #1', M.to_hex({0x0b,0x02,0x04,0x08,0xa}), '0b0204080a')
test_eq('test/to_hex #2', M.to_hex('foo'), '666f6f')

-- to_str
test_eq('test/to_str #1', M.to_str({0x66,0x6f,0x6f}), 'foo')
test_eq('test/to_str #2', M.to_str('666f6f'), 'foo')
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
