[![Mod version](https://img.shields.io/badge/mod_version-1.2-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2015--11--27-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2015--02--17-brightgreen.svg?style=flat-square)]()

Munin Statistics Mod by croxxx
==============================

Description
-----------
This mod adds pages to the builtin webserver that contain server statistics in an easy to parse format.
It is intended for use with the included plugins but can also be fetched by any other application capable of doing web requests.

Included are plugins/scripts for:
- Munin
- CollectD

As the statistics can be accessed by anyone opening the website it is recommended to configure the firewall on the server accordingly!


Installation
------------
1. Copy `MuninStats.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Make sure you have the builtin HTTP server enabled.
3. Start Manic Digger Server.
4. On the machine running Munin copy `manicdigger_` to `/usr/share/munin/plugins` and make it executeable (`chmod +x`)
5. Change `STATUS_URL` to the adress the server listens to (or set `statusurl` accordingly in Munin environment).
   Default will only work if Munin is running on the same machine and Manic Digger Port has not been changed.
5. Create symlinks to enable the graphs in Munin (you don't need both, but it is recommended):
	- `ln -s /usr/share/munin/plugins/manicdigger_ /etc/munin/plugins/manicdigger_players`
	- `ln -s /usr/share/munin/plugins/manicdigger_ /etc/munin/plugins/manicdigger_blocks`
6. Wait a few minutes for Munin to fetch the data


Compatibility
-------------
- no conflicts with other mods
