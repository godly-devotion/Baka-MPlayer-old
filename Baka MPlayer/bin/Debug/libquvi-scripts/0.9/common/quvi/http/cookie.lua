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

function M.session(s)
  local C = require 'quvi/const'
  local o = { [C.qoo_http_cookie_mode] = C.qohco_mode_session }
  return quvi.http.cookie(s, o)
end

function M.file(s)
  local C = require 'quvi/const'
  local o = { [C.qoo_http_cookie_mode] = C.qohco_mode_file }
  return quvi.http.cookie(s, o)
end

function M.list(s)
  local C = require 'quvi/const'
  local o = { [C.qoo_http_cookie_mode] = C.qohco_mode_list }
  return quvi.http.cookie(s, o)
end

function M.jar(s)
  local C = require 'quvi/const'
  local o = { [C.qoo_http_cookie_mode] = C.qohco_mode_jar }
  return quvi.http.cookie(s, o)
end

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
