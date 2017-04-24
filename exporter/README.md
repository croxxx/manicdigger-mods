[![Mod version](https://img.shields.io/badge/mod_version-1.0-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2014--03--12-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--02--01-brightgreen.svg?style=flat-square)]()

Exporter Mod by croxxx
======================

Description
-----------
This mod allows you to export certain areas of your map as a file (and import these again).
Useful for cross-map copying.

Following commands are added

	/export start		 Sets the start point for the export area
	/export end			 Sets the start point for the export area
	/export save [name]	 Saves the marked area as "name.mdexp"
	/import	[filename]	 Tries to import the file given


Installation
------------
1. Copy `Exporter.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
3. Start Manic Digger Server and enjoy a few useful extra commands.


Available privilege
-------------------
- export


Compatibility
-------------
- no conflicts with other mods


Additional Notes
----------------
- Areas get imported in positive X and Y direction. Use .pos command to determine the correct start point.
- **The size of exported areas is not limited**. Be aware that big areas result in big files!
- The import will fail if the .mdexp file contains blocks that belong to mods not installed and can't be identified.
  An error containing the missing block name will show up in the console.
- **Changes cannot be undone!** Always do a backup before importing!
