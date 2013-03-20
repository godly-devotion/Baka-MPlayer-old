
-- libquvi-scripts
-- Copyright (C) 2012  Guido Leisker <guido@guido-leisker.de>
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

-- About
--  Each video url ends with an id composed of digits.
--  This id leads us to a metadata xml file (see function
--  MySpass.getMetadataValue) containing all necessary information
--  including download link.

local MySpass = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
  package.path = self.script_dir .. '/?.lua'
  local C      = require 'quvi/const'
  local r      = {}
  r.domain     = "myspass%.de"
  r.formats    = "default"
  r.categories = C.proto_http
  local U      = require 'quvi/util'
  -- expect all urls ending with digits to be videos
  r.handles    = U.handles(self.page_url, {r.domain}, {"/myspass/.-/%d+/?$"})
  return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
  self.host_id = "myspass"

  self.id = self.page_url:match("(%d+)/?$")
      or error("no match: media ID")

  local format  = MySpass.getMetadataValue(self, 'format')
  local title   = MySpass.getMetadataValue(self, 'title')
  local season  = MySpass.getMetadataValue(self, 'season')
  local episode = MySpass.getMetadataValue(self, 'episode')
  self.thumbnail_url = MySpass.getMetadataValue(self, 'imagePreview') or ''

  self.title = string.format("%s %03d %03d %s", format, season,
                             episode, title)

  self.url = {MySpass.getMetadataValue(self, 'url_flv')}

  return self
end

--
-- Utility functions
--

function MySpass.getMetadataValue(self, key)
  if self.metadata == nil then
    self.metadata =  quvi.fetch(
      'http://www.myspass.de/myspass/'
          .. 'includes/apps/video/getvideometadataxml.php?id='
          .. self.id ) or error("cannot fetch meta data xml file")
  end
  local p = string.format("<%s>(.-)</%s>", key, key)
  local temp = self.metadata:match(p) or error("meta data: no match: " .. key)
  local value = temp:match('<!%[CDATA%[(.+)]]>') or temp
  return value
end

-- vim: set ts=2 sw=2 tw=72 expandtab:

