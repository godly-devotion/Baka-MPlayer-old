Baka MPlayer Changelog
======================

Latest Version 1.3.9 (2014-01-17)
---------------------------------

- NOTE: YOU MUST REMOVE ALL PREVIOUS FILES
- Large parts of the code have been refactored for efficiency
- Fixed: Several bugs
- Fixed: Seeking from seekbar is much more accurate
- Changed: Settings file is named 'Baka MPlayer.settings'
- Added: Program creates 'error_info.txt' if a fatal problem is encountered
- Added: Can permanently delete file from playlist via Shift+Del
- Updated: mpv build

Version 1.3.7.1 (2013-12-16)
----------------------------

- Fixed: Small bugs
- Fixed: General lag in player response (temporary fix by using older mpv build)
- Changed: Warn about URLs starting with HTTPS
- Added: Can delete file from playlist
- Added: Previous file is now located at the top of the playlist


Version 1.3.7.0 (2013-11-27)
----------------------------

- Fixed: Small bugs
- Fixed: Better cpu/memory usage
- Changed: Minor interface tweaks
- Changed: Switched from UltraID3Lib to taglib-sharp
- Added: New voice commands
- Added: Voice audio level indicator
- Updated: mpv build

Version 1.3.6.0 (2013-11-17)
----------------------------

- Fixed: Small bugs
- Fixed: Minor interface tweaks
- Fixed: Unfocus from resizing splitter after resizing command line
- Fixed: Network messages appear more accurately
- Fixed: Input error messages in command line
- Changed: Must double click on the same location twice to go into full screen
- Added: Navigate chapters via Page Up and Page Down keys
- Added: Changelog link in update window
- Added: Shows error message if file is unplayable
- Updated: mpv build

Version 1.3.5.0 (2013-11-03)
----------------------------

- Fixed: Unfocus from resizing splitter after resizing playlist
- Fixed: Unfocus from playlist and console when hidden
- Changed: Common unplayable files will no longer show up in the playlist (even with Show All Files enabled)
- Changed: Media Info window will show accurate file type information
- Added: XButton1 & XButton2 seek backwards and forwards respectively (side/thumb mouse buttons)
- Added: Double click to go into full screen
- Added: Double click on the rewind button to stop playing
- Added: Warn if slow/stuck network hinders playback

Version 1.3.4.0 (2013-10-06)
----------------------------

- Fixed: Small bugs
- Fixed: Default file name for snapshot
- Changed: Playlist will not auto show if there is only one file
- Added: Frame step backwards
- Added: Customize voice command call name from settings file (default: 'baka')
- Updated: mpv build

Version 1.3.3.0 (2013-09-17)
----------------------------

- Fixed: Next file not playing automatically
- Changed: 'OptionalArgs' has been removed from settings. Use mpv's config file instead
- Changed: After reaching the end, you can press play instead of pressing the reverse button to reload the file
- Changed: Updates a checked weekly
- Added: Right-clicking on the video will toggle play state
- Removed: Ability to set mpv executable name in settings
- Removed: Ability to minimize to tray (was broken)
- Updated: mpv build

Version 1.3.2.0 (2013-09-09)
----------------------------

- Fixed: Small bugs
- Fixed: Sometimes videos would not go into full screen properly
- Changed: 'framedrop=yes' has been moved into the settings file
- Added: Media length and video dimensions to Info window
- Updated: mpv build

Version 1.3.0.1 (2013-08-23)
----------------------------

- Updated: Changed website host (bakamplayer.u8sand.net)

Version 1.3.0.0 (2013-08-18)
----------------------------

- Fixed: Several minor bugs
- Fixed: Loading external subtitle files
- Fixed: Disable screensaver and screen blankers from running
- Fixed: Typo in Help window
- Fixed: General cursor problems in full screen mode
- Changed: Full screen mode enabled to all media types
- Added: User customizable optional arguments support for mpv

Version 1.2.2.0 (2013-08-05)
----------------------------

- Fixed: Small bugs
- Changed: Simplified seek and volume control's looks
- Added: Chapter marks on seek bar (or go to Media -> Chapters)
- Added: Can Show/Hide sub's via Ctrl+W
- Updated: mpv build

Version 1.2.1.0 (2013-07-18)
----------------------------

- Fixed: Crashes when filename contained certain characters
- Changed: mpv no longer needs to cache font
- Changed: Updates are only checked once a month (you can still manually check)
- Updated: mpv build

Version 1.2.0.0 (2013-06-09)
----------------------------

- Fixed: Some bugs
- Fixed: Better memory usage
- Fixed: Crashes when closing program
- Added: Status output for Audio, Sub, or Chapter change

Version 1.1.0.0 (2013-05-22)
----------------------------

- Fixed: Small bugs
- Fixed: Problems closing program on first try
- Fixed: Crashes when a mic is not detected for voice command
- Changed: Playlist doesn't auto show unless it needs to now
- Updated: mpv build

Version 1.0.0.0 (2013-04-17)
----------------------------

- Note: You MUST remove all previous files before version 0.5
- Fixed: Small bugs
- Fixed: Playlist code has been painfully rewritten (e.g. playlist wont crash if file doesn't exist on playlist anymore)
- Fixed: Properly gets the media title on quvi supported sites (e.g. Youtube)
- Fixed: Crashes when viewing online file's Media Info

Version 0.5.0.0 Beta (2013-04-06)
---------------------------------

- Note: You MUST remove all previous files
- Changed: Using mpv (fork of mplayer2) as backend now
- Changed: Cleaned up code
- Fixed: Problems with cultures that use '.' as ',' (e.g. Brazil)
- Fixed: Crashes when opening online URLs
- Added: Can exit fullscreen mode by Escape key
- Added: Ability to open external subtitle files
- Added: Ability to change the aspect ratio
- Removed: Undo button on URL window (it didn't work anyway)

Version 0.4.7.0 Alpha (2013-02-19)
----------------------------------

- Fixed: Some bugs
- Updated: Some graphical components
- Updated: mplayer2 build

Version 0.4.1.0 Alpha (2012-05-29)
----------------------------------

- Fixed: Some bugs
- Fixed: Mouse auto hide for fullscreen mode
- Updated: Small speed & memory improvements
- Updated: Can copy screenshot to clipboard
- Updated: Media Info window
- Updated: mplayer2 build

Version 0.4.0.2 Alpha (2012-04-29)
----------------------------------

- Fixed: Some bugs

Version 0.4.0.1 Alpha (2012-04-26)
----------------------------------

- Fixed: Some bugs
- Fixed: Album art
- Updated: Updates are linked to your specific OS version now (64bit or 32bit)
- Updated: mplayer2 build
- Updated: Small UI tweaks
- Updated: Win 7 taskbar buttons

Version 0.3.2.1 Alpha (2012-04-03)
----------------------------------

- Initial Release