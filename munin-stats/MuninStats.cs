/*
* Munin Stats Mod - Version 1.1
* Last change: 2015-06-05
* Author: croxxx
*
* This mod adds pages to the builtin webserver that can be parsed by a munin plugin to gather statistics.
*/

using System;
using FragLabs.HTTP;

namespace ManicDigger.Mods.Fortress
{
	public class MuninStats : IMod
	{
		MuninStatsModule module;
		MuninStats_BlocksModule module_blocks;
		MuninStats_PlayersModule module_players;
		MuninStats_ChatModule module_chat;

		public void PreStart(ModManager m) { }
		public void Start(ModManager m)
		{
			module = new MuninStatsModule();
			module.m = m;
			m.InstallHttpModule("munin.stats", () => "Server stats for Munin - Overview", module);

			module_blocks = new MuninStats_BlocksModule();
			module_blocks.m = m;
			m.InstallHttpModule("munin.stats.blocks", () => "", module_blocks);

			module_players = new MuninStats_PlayersModule();
			module_players.m = m;
			m.InstallHttpModule("munin.stats.players", () => "", module_players);

			module_chat = new MuninStats_ChatModule();
			module_chat.m = m;
			m.InstallHttpModule("munin.stats.chat", () => "", module_chat);

			m.RegisterOnBlockUse(OnUse);
			m.RegisterOnBlockBuild(OnBuild);
			m.RegisterOnBlockDelete(OnDelete);
			m.RegisterOnPlayerChat(OnChat);
		}

		void OnUse(int player, int x, int y, int z)
		{
			module_blocks.blocks_used++;
		}
		void OnBuild(int player, int x, int y, int z)
		{
			module_blocks.blocks_placed++;
		}
		void OnDelete(int player, int x, int y, int z, int block)
		{
			module_blocks.blocks_destroyed++;
		}
		string OnChat(int player, string message, bool toTeam)
		{
			module_chat.sent_messages++;
			return message;
		}
	}
	public class MuninStatsModule : IHttpModule
	{
		public ModManager m;

		public void Installed(HttpServer server) { }

		public void Uninstalled(HttpServer server) { }

		public bool ResponsibleForRequest(HttpRequest request)
		{
			if (request.Uri.AbsolutePath.ToLower() == "/munin.stats")
				return true;
			return false;
		}

		public bool ProcessAsync(ProcessRequestEventArgs args)
		{
			string output = "";
			output += "<a href=\"munin.stats.blocks\">munin.stats.blocks</a><br>\n";
			output += "<a href=\"munin.stats.players\">munin.stats.players</a><br>\n";
			output += "<a href=\"munin.stats.chat\">munin.stats.chat</a><br>\n";
			args.Response.Producer = new BufferedProducer(output);
			return false;
		}
	}

	public class MuninStats_PlayersModule : IHttpModule
	{
		public ModManager m;
		int players_online;
		int players_npc;
		int players_spectator;
		int players_max;

		public void Installed(HttpServer server)
		{
			players_online = 0;
			players_npc = 0;
			players_spectator = 0;
			players_max = 0;
		}

		public void Uninstalled(HttpServer server) { }

		public bool ResponsibleForRequest(HttpRequest request)
		{
			if (request.Uri.AbsolutePath.ToLower() == "/munin.stats.players")
				return true;
			return false;
		}

		public bool ProcessAsync(ProcessRequestEventArgs args)
		{
			calculatePlayerStats();
			string output = "";
			output += string.Format("{0}: {1}\n", "players_online", players_online);
			output += string.Format("{0}: {1}\n", "players_npc", players_npc);
			output += string.Format("{0}: {1}\n", "players_spectator", players_spectator);
			output += string.Format("{0}: {1}\n", "players_max", players_max);
			args.Response.Producer = new BufferedProducer(output);
			return false;
		}

		void calculatePlayerStats()
		{
			players_online = 0;
			players_npc = 0;
			players_spectator = 0;
			players_max = 0;

			int[] allPlayers = m.AllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				if (m.IsBot(allPlayers[i]))
				{
					players_npc++;
				}
				if (m.IsPlayerSpectator(allPlayers[i]))
				{
					players_spectator++;
				}
			}
			players_online = allPlayers.Length;
			players_max = m.GetMaxPlayers();
		}
	}

	public class MuninStats_BlocksModule : IHttpModule
	{
		public ModManager m;
		public int blocks_placed;
		public int blocks_destroyed;
		public int blocks_used;

		public void Installed(HttpServer server)
		{
			resetStats();
		}

		public void Uninstalled(HttpServer server) { }

		public bool ResponsibleForRequest(HttpRequest request)
		{
			if (request.Uri.AbsolutePath.ToLower() == "/munin.stats.blocks")
				return true;
			return false;
		}

		public bool ProcessAsync(ProcessRequestEventArgs args)
		{
			string output = "";
			output += string.Format("{0}: {1}\n", "blocks_placed", blocks_placed);
			output += string.Format("{0}: {1}\n", "blocks_destroyed", blocks_destroyed);
			output += string.Format("{0}: {1}\n", "blocks_used", blocks_used);
			args.Response.Producer = new BufferedProducer(output);
			resetStats();
			return false;
		}

		void resetStats()
		{
			blocks_placed = 0;
			blocks_destroyed = 0;
			blocks_used = 0;
		}
	}

	public class MuninStats_ChatModule : IHttpModule
	{
		public ModManager m;
		public int sent_messages;

		public void Installed(HttpServer server)
		{
			resetStats();
		}

		public void Uninstalled(HttpServer server) { }

		public bool ResponsibleForRequest(HttpRequest request)
		{
			if (request.Uri.AbsolutePath.ToLower() == "/munin.stats.chat")
				return true;
			return false;
		}

		public bool ProcessAsync(ProcessRequestEventArgs args)
		{
			string output = "";
			output += string.Format("{0}: {1}\n", "sent_messages", sent_messages);
			args.Response.Producer = new BufferedProducer(output);
			resetStats();
			return false;
		}

		void resetStats()
		{
			sent_messages = 0;
		}
	}
}
