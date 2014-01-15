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

--
-- Shorthand functions for the common hash functions.
--  NOTE: None of these functions unset the 'croak_if_error' flag
--

local M = {}

function M._init(s)
  local C = require 'quvi/const'
  local H = require 'quvi/hex'
  local s = H.to_hex(s)
  return C,H,s
end

-- A shorthand for calling quvi.crypto.hash for a MD5 hash.
function M.md5sum(s)
  local C,H,s = M._init(s)
  return quvi.crypto.hash(s, { [C.qoo_crypto_algorithm] = 'md5' }).digest
end

-- A shorthand for calling quvi.crypto.hash for a MD5 hash.
function M.sha1sum(s)
  local C,H,s = M._init(s)
  return quvi.crypto.hash(s, { [C.qoo_crypto_algorithm] = 'sha1' }).digest
end

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
