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

local ResolveExceptions = {} -- Utility functions unique to this script

function resolve_redirections(qargs)
--[[

UPDATE (2013-10-07)

libquvi will always attempt to resolve URL redirections.  This means
that quvi_media_new (for example) will always resolve first and only
then pass the input URL to the media scripts to determine the support.

Problem:

  - g00gle now redirects the embedded media URLs to the API pages

  - This causes libquvi to return the "no support" because the
    destination URL looks nothing like a typical media page URL

Solution:

  - "normalize" (quvi/youtube) the input URL before we try to resolve
    URL redirections

  - "normalize" will convert the embedded media URLs to media page URLs,
    which can then be passed to libquvi/libcurl for URL resolving

Notes:

  - quvi_supports function skips resolving altogether unless online
    check is forced

  - This is the reason "normalize" must still be called in "ident"
    function of the media script, even if we do so here

]]--
  local Y = require 'quvi/youtube'
  local u = Y.normalize(qargs.input_url)

  -- Have libcurl resolve the URL redirections.
  local r = quvi.http.resolve(u)
  if #r.resolved_url ==1 then return u end

  -- Apply any exception rules to the destination URL.
  return ResolveExceptions.YouTube(qargs, r.resolved_url)
end

--
-- Utility functions
--

function ResolveExceptions.YouTube(qargs, dst)
  return dst -- UPDATE (2012-11-18): g00gle servers no longer strip the "#t="
  --
  -- Preserve the #t= (if any) after redirection.
  -- e.g. http://www.youtube.com/watch?v=LWxTGJ3TK1U#t=2m22s
  --   -> http://www.youtube.com/watch?v=LWxTGJ3TK1U
  --
--  return table.concat({dst, (qargs.input_url:match('(#t=%w+)') or '')})
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
