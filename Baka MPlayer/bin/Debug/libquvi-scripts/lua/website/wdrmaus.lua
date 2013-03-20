
-- libquvi-scripts
-- Copyright (C) 2013  Guido Leisker <guido@guido-leisker.de>
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
--  wrdmaus.de falls into different sections each working
--  quite differently
--
--  elefantenseite:
--    flash gallery with link target to proper pages for each video
--    last part of url is id, configuration.php5
--    http://www.wdrmaus.de/elefantenseite/
--    http://www.wdrmaus.de/elefantenseite/#/anke_tanzt_zooztiere
--
--  kaeptnblaubaerseite:
--    single flash, users need to use html only page (html gallery)
--    and copy target links of videos
--    http://www.wdrmaus.de/kaeptnblaubaerseite/baerchen/tv.php5
--    http://www.wdrmaus.de/kaeptnblaubaerseite/baerchen/tv.php5?mid=1&dslSrc=rtmp://gffstream.fcod.llnwd.net/a792/e2/blaubaer/flash/oink_web-m.flv&isdnSrc=rtmp://gffstream.fcod.llnwd.net/a792/e2/blaubaer/flash/oink_web-s.flv
--
--  sachgeschichten:
--    html video gallery, users need to copy target links of videos
--    http://www.wdrmaus.de/sachgeschichten/sachgeschichten/
--    http://www.wdrmaus.de/sachgeschichten/sachgeschichten/sachgeschichte.php5?id=2702
--
--  entenseite (very few videos, not supported!):
--    http://www.wdrmaus.de/enteseite/index.php5
--    http://www.wdrmaus.de/enteseite/tuerenauf/index.php5?v=3#v2
--
--  There are more sections but they seem to provide no videos.

local WdrMaus = {} -- Utility functions unique to this script

-- Identify the script.
function ident(self)
  package.path = self.script_dir .. '/?.lua'
  local C      = require 'quvi/const'
  local r      = {}
  r.domain     = "wdrmaus%.de"
  r.formats    = "default"
  r.categories = C.proto_rtmp
  local U      = require 'quvi/util'
  -- there seems to be no possiblity to really
  -- make a decision here: we are using a less
  -- strict pattern here
  r.handles    =  U.handles(self.page_url, {r.domain})
  return r
end

-- Query available formats.
function query_formats(self)
    self.formats = 'default'
    return self
end

-- Parse media URL.
function parse(self)
  self.host_id = "wdrmaus"

  local a = {
      {pat='kaeptnblaubaerseite', func=WdrMaus.parseKaeptnblaubaerseite},
      {pat='sachgeschichten',     func=WdrMaus.parseSachgeschichten},
      {pat='elefantenseite',      func=WdrMaus.parseElefantenseite}
  }

  local U = require 'quvi/url'
  local t = U.parse(self.page_url)
  local s = {}

  for _,v in pairs(a) do
      if t.path:match(v.pat) then return v.func(self) end
      table.insert(s, v.pat)
  end

  error(string.format("limited support for the {%s} sections only",
                        table.concat(s, ',')))
end

--
-- Utility functions
--

function WdrMaus.parseElefantenseite(self)
  self.id = self.page_url:match('/([%w_]-)$')
                or error('no match: media ID')

  local rooturl = self.page_url:match('(%w+://.+/%w+)/.*')
                      or error('no match: root url')

  local qo = {fetch_type='config'}
  local configuration = quvi.fetch(rooturl .. '/data/configuration.php5', qo)

  local streamingServerPath =
          WdrMaus.getXMLvalue(configuration, 'streamingServerPath')

  local toc = quvi.fetch(rooturl .. '/data/tableOfContents.php5', qo)

  local metadataPath = WdrMaus.getMetadataPathFromToc(toc, self.id)
  local metadata = quvi.fetch(rooturl .. '/' .. metadataPath, qo);

  streamingServerPath = string.gsub(streamingServerPath, '/$', '')
  self.url = { streamingServerPath .. WdrMaus.getXMLvalue(metadata, 'file') }
  self.title = WdrMaus.getXMLvalue(metadata, 'title')

  -- no idea why this url has a diffent host
  self.thumbnail_url = 'http://www.wdr.de/bilder/mediendb/elefant_online'
        .. WdrMaus.getXMLvalue(metadata, 'image')

  return self
end

function WdrMaus.parseKaeptnblaubaerseite(self)
  self.id = self.page_url:match('/([^/]-)$') or error('no match: media ID')

  local qo = {fetch_type='config'}
  local metadatasite = quvi.fetch(
      'http://www.wdrmaus.de/kaeptnblaubaerseite/baerchen/tv.php5', qo)

  local matcher = string.gsub(self.id, '-' , '.')
  metadata = metadatasite:match('.*(<img.-' .. matcher .. ')')
                or error('no match: metadata')

  self.title = metadata:match('<p>%s-(.-)%s-<br') or error('no match: title')
  local thumb_rel = metadata:match('<img src="(.-)"') or ''

  thumb_rel = string.gsub(thumb_rel, '%.%.', '')
  self.thumbnail_url = 'http://www.wdrmaus.de/kaeptnblaubaerseite' ..thumb_rel

  self.url = { self.page_url:match('.-(rtmp.-flv)')
                  or error('no match: media stream URL') }
  return self
end

function WdrMaus.parseSachgeschichten(self)
  self.id = self.page_url:match('%d+$') or error('no match: media ID')
  local metadata = quvi.fetch(self.page_url)
  self.title = metadata:match('h1_019DCE">(.-)<') or error('no match: title')
  self.url = { metadata:match('(rtmp.-mp4)')
                or error('no match: media stream URL') }
  return self
end

function WdrMaus.getXMLvalue(str, value)
  ret = str:match('<' .. value .. '>(.-)</' .. value .. '>')
            or error('Cannot match ' .. value)
  return ret:match('<!%[CDATA%[(.+)]]>') or ret
end

function WdrMaus.getMetadataPathFromToc(toc, id)
  -- the toc contains all paths for this root (f.i. elefantenkino)
  local page = toc:match('<id>.-(' .. id .. '.-</xmlPath>)')
                  or error('no match: metadata path')
  return WdrMaus.getXMLvalue(page, 'xmlPath')
end

-- vim: set ts=2 sw=2 tw=72 expandtab:
