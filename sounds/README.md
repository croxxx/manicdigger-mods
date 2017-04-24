[![Mod version](https://img.shields.io/badge/mod_version-1.1-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2013--11--17-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--01--17-brightgreen.svg?style=flat-square)]()

Sounds Mod by croxxx
====================

Description
-----------
This mod adds a sound system to your server.
Following commands are added:

	/soundlist          Displays a list of all available sounds.
	/sound reload       Reloads the sound list (no restart required).
	
	/s [soundname]      Plays the specified sound
	/sound [soundname]  Plays the specified sound


Installation
------------
1. Copy `Sounds.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
3. Create the directory `UserData\Sounds` and copy your sounds there (ogg file format).
4. Create a text document called `soundlist.txt` in the same directory.
5. In each line enter the name of a sound file without extension.
6. Start Manic Digger Server and enjoy playing sounds.


Available privileges
--------------------
- play_sounds
- reload_soundlist


Compatibility
-------------
- no conflicts with other mods


Additional Note
---------------
The sounds have to be served in .ogg file format.
You can either play sounds for all players or just at the user's position.
To change this just change line 37 of `Sounds.cs` with any text editor.
