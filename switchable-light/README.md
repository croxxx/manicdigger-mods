[![Mod version](https://img.shields.io/badge/mod_version-1.1-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2013--08--23-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--01--17-brightgreen.svg?style=flat-square)]()

Switchable Lights Mod by croxxx
===============================

Description
-----------
This Mod adds a new light block to creative Inventory that can be toggled on and off.
To be able to toggle lights the player must have the privilege `toggle`.
  
Additionally there is a command `/toggle_all [0/1]` that switches all lights on the map.
To be able to use this command the player must have the `toggle_all` privilege.


Installation
------------
1. Copy `SwitchableLight.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Copy the 2 texture files `light_on.png` and `light_off.png` into the subfolder `data\public\blocks` in your Manic Digger installation directory.
3. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
4. Start Manic Digger and enjoy switchable light blocks.


Available privileges
--------------------
- toggle
- toggle_all


Compatibility
-------------
- uses Block IDs 254 and 255
- no conflicts with other mods (so far)


Additional Note
---------------
All textures for this mod were created entirely by me. No templates or other pictures were used.
The original GIMP .xcf files are included in the subfolder "_resources". They have a resolution of 512 x 512 pixels.
