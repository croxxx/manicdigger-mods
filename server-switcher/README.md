[![Mod version](https://img.shields.io/badge/mod_version-1.0-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2014--08--25-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2014--08--05-brightgreen.svg?style=flat-square)]()

ServerSwitcher Mod by croxxx
============================

Description
-----------
This mod adds a command to switch from one server to another.
This could be used for server networks.


Installation
------------
1. Copy `ServerSwitcher.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
3. Start Manic Digger Server and enjoy server redirection.


Command Usage
-------------
You can manage available target servers ingame by using these commands.
Managing these requires the `switcher_manage` privilege.

- `/switcher add <ip> <port> <target_id>`
- `/switcher remove <target_id>`

Use `/go <target_id>` to get redirected to the specified target server.


Available privileges
--------------------
- `switcher_manage`


Compatibility
-------------
- This Mod just adds an extra command. It should be compatible with any other mod.
