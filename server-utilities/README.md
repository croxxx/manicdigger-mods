[![Mod version](https://img.shields.io/badge/mod_version-1.3-brightgreen.svg?style=flat-square)]()
[![Mod release](https://img.shields.io/badge/release_date-2015--02--18-brightgreen.svg?style=flat-square)]()
[![Mod status](https://img.shields.io/badge/mod_status-stable-brightgreen.svg?style=flat-square)]()
[![Mod requirement](https://img.shields.io/badge/manicdigger_version->2015--02--17-brightgreen.svg?style=flat-square)]()

Server Utilities Mod by croxxx
==============================

Description
-----------
This mod adds some useful commands to your server.
Following commands are added:

| Command      | Description |
| ------------ | ----------- |
| tp_id        | Allows the user/group to teleport to the given player ID. |
| pull         | Allows the user/group to teleport other players to their own position (identified by player name) |
| p            | Alias for `pull` |
| pull_id      | Allows the user/group to teleport other players to their own position (identified by player ID). Usage of the pull commands can be monitored by setting `NotificationsActive` to `true`. Every user having a permission level equal to or above the one specified by `NotificationPlayerGroup` will then be notified every time the pull command is used. |
| p_id         | Alias for `pull_id` |
| countdown    | Allows the user/group to start a countdown. Syntax: `/countdown <time> <event>` |
| count        | Alias for `countdown` |
| colors       | Displays all colors that can be used in chat. No privilege needed. |
| copy         | Use `/copy start` and `/copy end` to define the area to be copied. `copypaste` privilege needed. |
| paste        | Use `/paste` to copy the previously defined area to your position. Use `/paste x` to crop the original area. `copypaste` privilege needed. **This action cannot be undone!** |
| vote         | You can vote by typing `/vote yes` or `/vote no`. Players with privilege `startvote` can also start votings by typing `/vote start <duration in min> <topic>`. |
| warn         | Allows players with `warn` privilege to warn other players for a specific reason. |
| report       | Allows players with `report` privilege to report other players. All users of a specified group will then be notified. If noone is currently online a message will be displayed the next time they join. |
| afk          | Sends a message to all players that the user is AFK. |
| btk          | Sends a message to all players that the user is BTK. |
| uptime       | Displays the user the current server uptime (useful for finding out when the next restart will be) |
| me           | Displays a message in the format `<playername> <argument>` to all. (i.e. `croxxx eats a cookie`) |
| vanish       | Makes the user (if permitted) invisible to other players and visible again |
| notification | Allows toggling notification options. Syntax: `/notification [vanish/warn]` |
| language     | Allows changing the language. Syntax. `/language [DE/EN]` |
| mute         | Mute the given player (if not already). Syntax: `/mute <playername>` |
| unmute       | Unmutes the given player (if muted). Syntax: `/unmute <playername>` |
| version      | Displays the Mod version. No privilege needed. |
| spectate     | Spectates the selected player. **Really buggy currently, MD problem.** Syntax: `/spectate <playername>` |
| stopspectate | Stops spectating a player. |
| spawn        | Respawns the player at their default spawn position (useful if they set their temporary spawn to another point (P)) |
| su_reload    | Reloads the file `settings.txt` so no restart is required to make them work. |
| namecolor    | Changes the color of the name displayed above a player. Takes color codes as argument. |


Installation
------------
1. Copy `ServerUtilities.cs` into the folder `Mods\Fortress` of your Manic Digger installation directory.
2. Open `ServerClient.txt` in `UserData\Configuration` and add the privileges you need.
3. Start Manic Digger Server and enjoy a lot of useful extra commands.


Available privileges
--------------------
- pull
- countdown
- copypaste
- vote
- startvote
- warn
- report
- afk
- me
- vanish
- notification
- language
- mute
- spectate
- su_reload
- namecolor


Compatibility
-------------
- no conflicts with other mods


Additional Note
---------------
You can now change settings permanently (without touching code) by changing "settings.txt" in the directory `UserData\ServerUtilities`.
The file will be created on first start. Changes will be applied on next restart or when you use `/su_reload`.
You can check the included `settings-example.txt` file to see what options are available.

Changelog
---------
#### Version 1.3 (2015-02-18)
* Some minor fixes and optimizations

#### Version 1.2 (2014-08-24)
+ Added /namecolor command which changes the player's name color (the name displayed above them)
+ Modified /uptime to display restart interval of the server
+ Added a notification when the server is about to restart
+ Added player's names automatically being assigned their group color on join
* Fixed grass blocks not being cloneable when season changed manually

#### Version 1.1 (2013-11-19)
+ Added command to change the language at runtime (permanently)
+ Added command to change the notification options at runtime (permanently)
+ Added /version command to show version of the mod
+ Added function to mute players (effective until next restart or revoke)
+ Added function to spectate players. DO NOT USE - buggy on Client-side
+ Added command to teleport to default spawn
+ Added logging of all warned players in WarnedPlayersLog.txt - Includes time, names and reason
+ Added logging of all finished (and also cancelled) votes to VoteLog.txt
+ Added additional security check to /paste
+ Added chat prefix (all Mod messages are now prefixed with "[ServerUtilities] ")
+ Added possibility to reload settings at runtime using /su_reload

* Fixed reported players not being saved when server restarts
  (now writes to file ReportedPlayers.txt and logs all reports ever made in ReportedPlayersLog.txt)
* Fixed a typo in german translation
* Fixed some general typos/wrong messages
* Fixed incorrect console output on /paste