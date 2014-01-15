-- libquvi-scripts
-- Copyright (C) 2013  Toni Gundogdu <legatvs@gmail.com>
-- Copyright (C) 2012  Ross Burton <ross@burtonini.com>
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

local Lego = {} -- Utility functions unique to this script

-- Identify the media script.
function ident(qargs)
  return {
    can_parse_url = Lego.can_parse_url(qargs),
    domains = table.concat({'lego.com'}, ',')
  }
end

-- Parse media properties.
function parse(qargs)
  local p = quvi.http.fetch(qargs.input_url).data

  if p:match('FirstVideoData') then
    Lego.parse_movies(qargs, p)
  else
    Lego.parse_videos(qargs ,p)
  end

  return qargs
end

--
-- Utility functions.
--

function Lego.can_parse_url(qargs)
  local U = require 'socket.url'
  local t = U.parse(qargs.input_url)
  if t and t.scheme and t.scheme:lower():match('^http$')
       and t.host   and t.host:lower():match('lego%.com$')
       and t.path   and (t.path:lower():match('/movies/')
                         or t.path:lower():match('/videos$'))
  then
    return true
  else
    return false
  end
end

function Lego.movies_iter_streams(j)
  local v = j['VideoHtml5'] or error('no match: VideoHtml5')
  local u = v['Url'] or error('no match: media stream URL')
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

function Lego.movies_thumb(qargs, p)
  local t = {'thumbNavigation.+', '<img src="(.-)" alt="',qargs.title, '"/>',}
  qargs.thumb_url = p:match(table.concat(t) or '')
end

function Lego.parse_movies(qargs, p) -- /movies/
  local d = p:match('FirstVideoData = (.-);')
              or error('no match: FirstVideoData')

  local J = require 'json'
  local j = J.decode(d)

  qargs.title = j['Name'] or ''
  Lego.movies_thumb(qargs, p)

  qargs.streams = Lego.movies_iter_streams(j)
  qargs.id = j['LikeObjectGuid'] or ''
end

function Lego.videos_iter_streams(L, i)
  local u = L.find_first_tag(i, 'movie')[1]
  local S = require 'quvi/stream'
  return {S.stream_new(u)}
end

function Lego.parse_videos(qargs, p)  -- /videos?video=ID
  local d = p:match('name="flashvars" value="configxml=(.-</lego>)')
              or error('no match: flashvars: configxml')

  local L = require 'quvi/lxph'
  local P = require 'lxp.lom'

  local x = P.parse(d)
  local m = L.find_first_tag(x, 'movieplayer')

  local c = L.find_first_tag(m, 'content')
  local i = L.find_first_tag(c, 'item')

  qargs.title = L.find_first_tag(i, 'trackingName')[1]
  qargs.thumb_url = L.find_first_tag(i, 'cover')[1]

  local U = require 'socket.url'
  local q = U.unescape(U.parse(qargs.input_url).query)
  qargs.id = (q:match('video=%{(.-)%}') or ''):lower()

  qargs.streams = Lego.videos_iter_streams(L, i)
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
