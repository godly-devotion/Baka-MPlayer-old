-- libquvi-scripts
-- Copyright (C) 2010-2013  Toni Gundogdu <legatvs@gmail.com>
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

--
-- (q)uvi (err)or codes
--
-- static
M.qerr_ok                         = 0x00
M.qerr_callback_aborted           = 0x01
M.qerr_no_subtitle_export_scripts = 0x02
M.qerr_no_subtitle_scripts        = 0x03
M.qerr_no_playlist_scripts        = 0x04
M.qerr_no_media_scripts           = 0x05
M.qerr_no_scan_scripts            = 0x06
M.qerr_no_util_scripts            = 0x07
M.qerr_keyword_croak              = 0x08
M.qerr_invalid_arg                = 0x09
M.qerr_proxy_init                 = 0x0a
M.qerr_curl_init                  = 0x0b
M.qerr_lua_init                   = 0x0c
-- dynamically created
M.qerr_no_support                 = 0x40
M.qerr_callback                   = 0x41
-- an error occurred in a callback, e.g. a network error.
M.qerr_script                     = 0x42

--
-- (q)uvi (o)bject (o)ption
--
M.qoo_croak_if_error      = 0x01 -- Terminate if an error occurs
-- fetch
M.qoo_fetch_from_charset  = 0x20 -- Convert (to UTF-8) from this charset
-- http
M.qoo_http_cookie_mode    = 0x40 -- HTTP cookie function
-- crypto
M.qoo_crypto_cipher_flags = 0x60 -- Cipher flags
M.qoo_crypto_cipher_mode  = 0x61 -- Cipher mode
M.qoo_crypto_cipher_key   = 0x62 -- Cipher key (hex-string)
M.qoo_crypto_algorithm    = 0x63 -- Algorithm, e.g. 'aes' or 'sha1'

--
-- (q)uvi (o)bject (c)rypto (o)ption
-- See gcrypt.h, and http://www.gnupg.org/documentation/manuals/gcrypt/
--
-- cipher modes
M.qoco_cipher_mode_ecb    = 0x01
M.qoco_cipher_mode_cfb    = 0x02
M.qoco_cipher_mode_cbc    = 0x03
M.qoco_cipher_mode_stream = 0x04
M.qoco_cipher_mode_ofb    = 0x05
M.qoco_cipher_mode_ctr    = 0x06
-- cipher flags
M.qoco_cipher_secure      = 0x01
M.qoco_cipher_enable_sync = 0x02
M.qoco_cipher_cbc_cts     = 0x04
M.qoco_cipher_cbc_mac     = 0x08

--
-- (q)uvi (o)bject (h)ttp (c)ookie (o)ption
--
M.qohco_mode_session  = 0x01
M.qohco_mode_file     = 0x02
M.qohco_mode_list     = 0x03
M.qohco_mode_jar      = 0x04

--
-- subtitle
--

-- (s)ubtitle (t)ype
M.st_tts = 0x1  -- text-to-speech
M.st_cc  = 0x2  -- closed caption

-- (s)ubtitle (i)nternal (f)ormat
M.sif_tt = 0x1 -- timed text

return M

-- vim: set ts=2 sw=2 tw=72 expandtab:
