[![Mod version](https://img.shields.io/badge/mod_version-1.2-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2015--01--28-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--08--05-brightgreen.svg?style=flat-square)]()

Teleport Blocks Mod by croxxx
=============================

Description
-----------
This mod adds "Teleport Blocks" to your server.

Place any Teleport Block you want anywhere in the world. You will be reminded if the placed block has no target set.
Set a target for the placed Teleport Block using `/set_tptarget`. Syntax: `/set_tptarget [Teleport_Block_ID] [Target_X] [Target_Y] [Target_Z]`
If you destroy a Teleport Block its target position will be reset, too.
If you want to move a Teleport Block and keep its target position just place the same Teleport Block anywhere again. The old block will be removed automatically.


Installation
------------
1. Copy `TeleportBlocks.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Copy the 10 texture files named `Teleport_[0-9].png` into the subfolder `data\public\blocks` in your Manic Digger installation directory.
3. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
4. Start Manic Digger and have fun with Teleport Blocks.


Available privileges
--------------------
- `use_tpblock`  
  Allows the user/group to use the teleport functions of teleport blocks
- `set_tptarget`  
  Allows the user/group to set the target positions for teleport blocks
- `get_tpblocks`  
  Allows the user/group to get all teleport blocks available with the command "/get_tpblocks"
- `reload_tpblocks`  
  Allows the user/group to reload teleport targets
- `delete_tpblocks`  
  Allows the user/group to delete single Teleport Blocks
- `delete_all_tpblocks`  
  Allows the user/group to delete all Teleport Blocks at once (useful if BlockIDs shall be changed)


Compatibility
-------------
- uses Block IDs 245 to 254 (default, can be easily adjusted)
- Incompatible with PermissionBlock Mod.


Additional Note
---------------
All textures for this mod were created entirely by me. No templates or other pictures were used.
The original GIMP .xcf files are included in the subfolder `_resources`. They have a resolution of 512 x 512 pixels.


Changelog
---------
#### Version 1.2 (2015-01-28)
- Added some messages to help debugging
- Added command to reload block targets at runtime: `/reload_tpblocks`
- Changed saving. Now blocks are only saved if changed. Prevents random corruption.

#### Version 1.1 (2013-11-26)
- Added a check for empty names when adding locations
- Reworked internal structures.
- Fixed people being teleported to 0,0,0 under certain conditions.
- Introduced new save system. All Locations are now stored in `UserData\TeleportBlocks.txt`
  NOTE: If the file the Teleport Blocks are stored in gets corupted it will be automatically deleted.
