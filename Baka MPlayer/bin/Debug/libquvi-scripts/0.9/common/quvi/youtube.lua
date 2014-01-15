-- libquvi-scripts
-- Copyright (C) 2012-2013  Toni Gundogdu <legatvs@gmail.com>
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
Return the `ident' data for the {media,subtitle} scripts.
Parameters:
  qargs .. quvi args
Returns:
  A table containing the values expected by the library.
]]--
function M.ident(qargs)
  local u = M.normalize(qargs.input_url)
  return {
    domains = table.concat({'youtube.com'}, ','),
    can_parse_url = M.can_parse_url(u)
  }
end

--[[
Check if script can parse the URL.
Parameters:
  url .. URL to check
Returns:
  A boolean value.
]]--
function M.can_parse_url(url)
  local U = require 'socket.url'
  local t = U.parse(url)
  if t and t.scheme and t.scheme:lower():match('^https?$')
       and t.host   and t.host:lower():match('youtube%.com$')
       and t.query  and t.query:lower():match('v=[%w-_]+')
       and t.path   and t.path:lower():match('^/watch$')
  then
    return true
  else
    return false
  end
end

--[[
Validate media/video ID and update URL 'query' and 'path' elements.
Parameters:
  u    .. Dictionary returned by socket.url.parse
  v_id .. Media/video ID to validate
Returns:
  Boolean value (true if ID looks valid). Modifies the 'query' and the
  'path' URL elements. These will then be used to rebuild the
  media/video URL.
]]--
function M.normalize_found_id(u, v_id, other_query_params)
  if v_id and #v_id ==11 then
    u.query = table.concat({'v=', v_id})
    u.path = '/watch'
    return true
  end
  return false
end

--[[
Try to match media/video ID in the URL 'path' element.
Parameters:
  u .. Dictionary returned by socket.url.parse
Returns:
  Boolean value (true if matched).
]]--
function M.normalize_path_match(u)
  for _,p in pairs({'/embed/([-_%w]+)', '/%w/([-_%w]+)', '/([-_%w]+)'}) do
    local v_id = u.path:match(p)
    if M.normalize_found_id(u, v_id) then
      return true
    end
  end
  return false
end

--[[
"Normalize" the given YouTube media/video URL.
Parameters:
  s  .. URL to normalize
Returns:
  Normalized URL
See also:
  Test cases at the bottom of this file.
]]--
function M.normalize(url)
  if not url then
    return url
  end

  local U = require 'socket.url'
  local u = U.parse(url)

  if not u.host or not u.path then
    return url
  end

  -- Unmangle "youtu.be" -> "youtube.com".
  u.host = u.host:gsub('youtu%.be', 'youtube.com')

  -- Process URLs of the youtube domain only.
  if not u.host:match('youtube%.com$') then
    return url
  end

  -- Normalize embedded media/video URLs.
  if M.normalize_path_match(u) then
    return U.build(u)
  end

  -- Rebuild and return the media URL.
  return U.build(u)
end

--[[
Append URL to qargs.media_url if it is unique by comparing video IDs.
Parameters:
  url .. URL to append
]]--
function M.append_if_unique(qargs, url)
  if not url then return end

  url = M.normalize(url)

  local U = require 'socket.url'
  local t = U.parse(url)

  if not t.host or not t.query then return end

  local p = 'v=([%w-_]+)'
  local v = t.query:match(p)

  for _,u in pairs(qargs.media_url) do
    local tt = U.parse(u)
    if tt.query and  v == tt.query:match(p) then
      return -- Found duplicate. Ignore URL.
    end
  end

  table.insert(qargs.media_url, url)
end

--
-- Tests
--

--[[
local function test_normalize()
  local test_cases = {
    --
    -- query parameters must be kept.
    --
    {url='http://www.youtube.com/watch?feature=player_embedded&v=3WSQH__H1XE',
     expect='http://www.youtube.com/watch?feature=player_embedded&v=3WSQH__H1XE'},

    {url='http://www.youtube.com/watch?v=3WSQH__H1XE&list=PLfoo',
      expect='http://www.youtube.com/watch?v=3WSQH__H1XE&list=PLfoo'},

    {url='http://www.youtube.com/watch?v=3WSQH__H1XE&list=PLfoo#t=1m09s',
      expect='http://www.youtube.com/watch?v=3WSQH__H1XE&list=PLfoo#t=1m09s'},

    --
    -- Protocol scheme should remain the same (e.g. https remains https)
    -- Different embedded media URL forms -> "youtube.com/watch?v="
    -- Shortened "youtu.be" -> "youtube.com"
    --
    {url = 'http://youtu.be/3WSQH__H1XE',
     expect = 'http://youtube.com/watch?v=3WSQH__H1XE'},

    {url = 'https://youtu.be/3WSQH__H1XE',
     expect = 'https://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtu.be/v/3WSQH__H1XE?hl=en',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtu.be/watch?v=3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtu.be/embed/3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtu.be/v/3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtu.be/e/3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtube.com/watch?v=3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtube.com/embed/3WSQH__H1XE',
     expect='http://youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://jp.youtube.com/watch?v=3WSQH__H1XE',
     expect='http://jp.youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://jp.youtube.com/embed/3WSQH__H1XE',
     expect='http://jp.youtube.com/watch?v=3WSQH__H1XE'},

    {url='https://jp.youtube.com/embed/3WSQH__H1XE',
     expect='https://jp.youtube.com/watch?v=3WSQH__H1XE'},

    {url='http://youtube.com/3WSQH__H1XE', -- invalid page url
     expect='http://youtube.com/watch?v=3WSQH__H1XE'}
  }
  local i,e = 0,0
  for _,v in pairs(test_cases) do
    local r = M.normalize(v.url)
    if r ~= v.expect then
      print(string.format('input: %s (#%s)\nexpected: %s\ngot: %s',
                          v.url, i, v.expect, r))
      e = e+1
    end
    i = i+1
  end
  print((e==0) and 'tests OK' or error('failed tests: '..e))
end
test_normalize()
]]--

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
