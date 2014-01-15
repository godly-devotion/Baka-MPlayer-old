-- libquvi-scripts
-- Copyright (C) 2012,2013  Toni Gundogdu <legatvs@gmail.com>
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

-- Parse scan properties.
function parse(qargs)
  qargs.media_url = {}

  local paths = {'/embed/([-_%w]+)', '/%w/([-_%w]+)', '/([-_%w]+)'}
  local hosts = {'youtu%.be', 'youtube%.com'}

  local Y = require 'quvi/youtube'
  local U = require 'socket.url'

  local scheme = U.parse(qargs.input_url).scheme

  for _,h in pairs(hosts) do
    for _,p in pairs(paths) do
      local r = table.concat({h,p})
      for v in qargs.content:gmatch(r) do
        if #v ==11 then
          local u = table.concat({scheme, '://youtube.com/watch?v=', v})
          Y.append_if_unique(qargs, u)
        end
      end
    end
  end
  return qargs
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
