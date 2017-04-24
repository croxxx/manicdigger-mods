[![Mod version](https://img.shields.io/badge/mod_version-1.1-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2013--12--25-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--01--17-brightgreen.svg?style=flat-square)]()

DialogSign Mod by croxxx
========================

Description
-----------
This mod adds writable signs to the game. This is a variant of [Anthony's version](http://manicdigger.sourceforge.net/forum/viewtopic.php?f=17&t=3147) of [Nuan's original mod](http://manicdigger.sourceforge.net/forum/viewtopic.php?f=17&t=2907).

To write on a sign stand inside it and type `/sign [message]`.
You can display the sign contents by looking at the sign and pressing E.


Installation
------------
1. Copy `DialogSignMod.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Copy `Sign.png` and `SignBG.png` to `data\public\blocks`.
   If you decide to use a different background image, replace the standard image and adjust the size parameters in `DialogSignMod.cs`
3. Open `ServerConfig.txt` in `UserData\Configuration` and add the privileges you need.
4. Done.


Available privileges
--------------------
- sign


Compatibility
-------------
- no conflicts with other mods
- uses block ID 155


Changelog
---------
- [1.1] Fixed multiple server crashes that occured on some characters
