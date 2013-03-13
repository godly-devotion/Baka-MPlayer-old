-- libquvi-scripts v0.4.10
-- Copyright (C) 2012  Toni Gundogdu <legatvs@gmail.com>
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

function suffix_from_contenttype(c_type)
  -- Hardcoded.
  if c_type:match("audio/mpeg") then return "mp3" end

  -- Use the media subtype as file extension whenever possible.
  -- Return 'flv' if nothing is matched.
  local mst = c_type:match("/(.-)$") or "flv"
  mst = mst:gsub("^x%-","")

  -- Some servers return the following content-types (instead
  -- of "video/x-flv") for flash videos:
  --   "application/x-shockwave-flash"
  --   "text/plain"
  for _,v in pairs({"octet", "shockwave","plain"}) do
    if mst:match(v) then return "flv" end
  end

  return mst
end

--[[
local a = {
  'video/x-flv',
  'video/flv',
  'audio/mpeg',
  'video/mpeg',
  'video/webm',
  'audio/mp4',
  'video/mp4',
  'application/octet-stream',
  'application/x-shockwave-flash',
  'text/plain',
  'invalid content-type',
}
for _,v in pairs(a) do
  print(v,to_file_ext({utilscript_dir='.'},v))
end
]]--

-- vim: set ts=2 sw=2 tw=72 expandtab:
