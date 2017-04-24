[![Mod version](https://img.shields.io/badge/mod_version-1.0-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2013--10--07-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2013--02--06-brightgreen.svg?style=flat-square)]()

AutoAnnouncer Mod by croxxx
===========================

Description
-----------
This mod adds automated announcements to your server.
Announcement intervals can be given in minutes.
It also supports welcome messages.

All required files are generated automatically as needed. You don't need to create them on your own.
Just run the Mod once, then edit the files to fit your needs.
Please ensure to save your announcement text files as UTF8 when using non-ASCII-characters.
Empty lines in announcements/the welcome message will be skipped.
You can use a line containing just a whitespace to insert empty lines, if you need to.


Available commands:
	/announcements on				 - Turns announcements on
	/announcements off				 - Turns announcements off
	/announcements welcome_on		 - Turns the welcome message on
	/announcements welcome_off		 - Turns the welcome message off
	/announcements count [number]	 - Sets the total number of announcements
	/announcements interval [number] - Sets the interval to display announcements (in minutes).

These are also displayed when `/announcements help` is entered in chat.


Default values are:
  - Announcements active
  - Welcome message active
  - 1 Announcement
  - 20 minutes Interval


All mod files are stored in `UserData\AutoAnnouncer\`.
Following files will be created:
  - settings.txt
  - Welcome.txt
  - Announcement_X.txt (*There will be as many files as total announcements. Numbered from 0 up.*)


Installation
------------
1. Copy `AutoAnnouncer.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Open `ServerConfig.txt` in `UserData\Configuration` and add the privileges you need.
3. Done.


Available privileges
--------------------
- manage_announcements


Compatibility
-------------
- no conflicts with other mods
