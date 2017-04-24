/*
 * AutoAnnouncer Mod - Version 1.0
 * Last change: 2013-10-07
 * Author: croxxx
 * 
 * This mod adds automatic announcements to your server. Intervals can be given in minutes.
 * It also supports welcome messages.
 * 
 * For more information on how to use this mod, read the included README.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ManicDigger.Mods
{
	public class AutoAnnouncer : IMod
	{
		public void PreStart(ModManager m) { }

		public void Start(ModManager m)
		{
			this.m = m;

			m.RegisterPrivilege("manage_announcements");
			m.RegisterCommandHelp("manage_announcements", "See /announcements help");
			m.RegisterOnPlayerJoin(Welcome);
			m.RegisterTimer(Announce, (double)60);
			m.RegisterOnCommand(OnCommand);

			if (!Directory.Exists(modDir))
			{
				System.Console.WriteLine("[AutoAnnouncer] Mod directory not found. Creating it.");
				Directory.CreateDirectory(modDir);
			}
			LoadSettings();
			CheckFiles();
			System.Console.WriteLine("[AutoAnnouncer] Loaded Mod Version 1.0");
		}

		//Internal variables.
		//DO NOT CHANGE!
		ModManager m;
		string modDir = "UserData" + Path.DirectorySeparatorChar + "AutoAnnouncer";
		string chatPrefix = "&8[&eAutoAnnouncer&8] ";
		int announcementID = 0;
		int minuteCounter = 0;
		AnnounceSettings settings;

		struct Announcement
		{
			public Announcement(string[] newLines)
			{
				lines = newLines;
			}
			public string[] lines;
		}
		struct AnnounceSettings
		{
			public AnnounceSettings(bool bAnnouncements, bool bWelcome, int number, int interval)
			{
				announceActive = bAnnouncements;
				welcomeActive = bWelcome;
				if (number < 1)
				{
					System.Console.WriteLine("[AutoAnnouncer] Invalid number of Announcements. Falling back to default (1).");
					announceCount = 1;
				}
				else
				{
					announceCount = number;
				}
				if (interval < 1)
				{
					System.Console.WriteLine("[AutoAnnouncer] Invalid Announcement interval. Falling back to default (20).");
					announceInterval = 20;
				}
				else
				{
					announceInterval = interval;
				}
			}
			public bool announceActive;
			public bool welcomeActive;
			public int announceCount;
			public int announceInterval;
		}

		void LoadSettings()
		{
			if (!File.Exists(modDir + Path.DirectorySeparatorChar + "settings.txt"))
			{
				System.Console.WriteLine("[AutoAnnouncer] Settings file not found. Creating new.");
				settings = new AnnounceSettings(true, true, 1, 20);
				SaveSettings();
			}
			else
			{
				DirectoryInfo di = new DirectoryInfo(modDir + Path.DirectorySeparatorChar + "settings.txt");
				try
				{
					using (TextReader tr = new StreamReader(di.FullName, Encoding.UTF8))
					{
						bool bAnnouncements = bool.Parse(tr.ReadLine());
						bool bWelcome = bool.Parse(tr.ReadLine());
						int iNumber = int.Parse(tr.ReadLine());
						int iInterval = int.Parse(tr.ReadLine());
						settings = new AnnounceSettings(bAnnouncements, bWelcome, iNumber, iInterval);
					}
				}
				catch (Exception ex)
				{
					System.Console.WriteLine("[AutoAnnouncer] ERROR:  " + ex.Message);
				}
				System.Console.WriteLine("[AutoAnnouncer] Settings successfully loaded.");
			}
		}

		void SaveSettings()
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(modDir + Path.DirectorySeparatorChar + "settings.txt"))
				{
					sw.WriteLine(settings.announceActive);
					sw.WriteLine(settings.welcomeActive);
					sw.WriteLine(settings.announceCount);
					sw.WriteLine(settings.announceInterval);
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("[AutoAnnouncer] ERROR:  " + ex.Message);
			}
		}

		void CheckFiles()
		{
			System.Console.WriteLine("[AutoAnnouncer] Checking files...");
			if (!File.Exists(modDir + Path.DirectorySeparatorChar + "Welcome.txt"))
			{
				System.Console.WriteLine("[AutoAnnouncer] Welcome.txt not found. Creating it.");
				File.CreateText(modDir + Path.DirectorySeparatorChar + "Welcome.txt").Close();
			}
			System.Console.WriteLine(string.Format("[AutoAnnouncer] {0} announcement file(s) expected. Checking...", settings.announceCount));
			for (int i = 0; i < settings.announceCount; i++)
			{
				if (!File.Exists(modDir + Path.DirectorySeparatorChar + "Announcement_" + i.ToString() + ".txt"))
				{
					System.Console.WriteLine(string.Format("[AutoAnnouncer] Announcement_{0}.txt not found. Creating it.", i));
					File.CreateText(modDir + Path.DirectorySeparatorChar + "Announcement_" + i.ToString() + ".txt").Close();
					break;
				}
			}
			System.Console.WriteLine("[AutoAnnouncer] Done checking files.");
		}

		void Announce()
		{
			if (settings.announceActive)
			{
				//Update minuteCounter
				if (minuteCounter > 0)
				{
					//Decrease counter if greater than zero
					minuteCounter--;
					return;
				}
				else
				{
					//minuteCounter = 0. This means it's time for an announcement (or interval has been changed by command)
					minuteCounter = settings.announceInterval;
				}
				//Check if announcementID exceeds maximum announcements (if so, reset)
				if (announcementID >= settings.announceCount)
				{
					announcementID = 0;
				}
				DirectoryInfo di = new DirectoryInfo(modDir + Path.DirectorySeparatorChar + "Announcement_" + announcementID.ToString() + ".txt");
				try
				{
					using (TextReader tr = new StreamReader(di.FullName, Encoding.UTF8))
					{
						//Send announcement line by line
						string line = tr.ReadLine();
						while (!string.IsNullOrEmpty(line))
						{
							m.SendMessageToAll(line);
							line = tr.ReadLine();
						}
					}
				}
				catch (Exception ex)
				{
					System.Console.WriteLine("[AutoAnnouncer] ERROR:  " + ex.Message);
				}
				//Increase announcementID
				announcementID++;
			}
		}

		void Welcome(int player)
		{
			if (settings.welcomeActive)
			{
				DirectoryInfo di = new DirectoryInfo(modDir + Path.DirectorySeparatorChar + "Welcome.txt");
				try
				{
					using (TextReader tr = new StreamReader(di.FullName, Encoding.UTF8))
					{
						//Send welcome message line by line
						string line = tr.ReadLine();
						while (!string.IsNullOrEmpty(line))
						{
							m.SendMessage(player, line);
							line = tr.ReadLine();
						}
					}
				}
				catch (Exception ex)
				{
					System.Console.WriteLine("[AutoAnnouncer] ERROR:  " + ex.Message);
				}
			}
		}

		bool OnCommand(int player, string command, string argument)
		{
			if (command.Equals("announcements", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "manage_announcements"))
				{
					m.SendMessage(player, m.colorError() + "You're not allowed to manage Announcements");
					System.Console.WriteLine(string.Format("[AutoAnnouncer] {0} tried to manage announcements (no permission)", m.GetPlayerName(player)));
					return true;
				}
				string[] args;
				try
				{
					args = argument.Split(' ');
					string option = args[0];
					if (option.Equals("help", StringComparison.InvariantCultureIgnoreCase))
					{
						m.SendMessage(player, chatPrefix + "&7Available commands:");
						m.SendMessage(player, chatPrefix + "&7/announcements on");
						m.SendMessage(player, chatPrefix + "&7/announcements off");
						m.SendMessage(player, chatPrefix + "&7/announcements welcome_on");
						m.SendMessage(player, chatPrefix + "&7/announcements welcome_off");
						m.SendMessage(player, chatPrefix + "&7/announcements count <number>");
						m.SendMessage(player, chatPrefix + "&7/announcements interval <minutes>");
						m.SendMessage(player, chatPrefix + "&7Version 1.0 (2013-10-07)");
						return true;
					}
					if (option.Equals("on", StringComparison.InvariantCultureIgnoreCase))
					{
						settings.announceActive = true;
						SaveSettings();
						m.SendMessage(player, string.Format(chatPrefix + "&2Automatic Announcements activated. Interval: {0} minutes", settings.announceInterval));
						System.Console.WriteLine(string.Format("{0} enabled automatic announcements.", m.GetPlayerName(player)));
						return true;
					}
					if (option.Equals("off", StringComparison.InvariantCultureIgnoreCase))
					{
						settings.announceActive = false;
						SaveSettings();
						m.SendMessage(player, chatPrefix + "&2Automatic Announcements disabled.");
						System.Console.WriteLine(string.Format("{0} disabled automatic announcements.", m.GetPlayerName(player)));
						return true;
					}
					if (option.Equals("welcome_on", StringComparison.InvariantCultureIgnoreCase))
					{
						settings.welcomeActive = true;
						SaveSettings();
						m.SendMessage(player, chatPrefix + "&2Welcome message activated.");
						System.Console.WriteLine(string.Format("{0} enabled welcome message.", m.GetPlayerName(player)));
						return true;
					}
					if (option.Equals("welcome_off", StringComparison.InvariantCultureIgnoreCase))
					{
						settings.welcomeActive = false;
						SaveSettings();
						m.SendMessage(player, chatPrefix + "&2Welcome message disabled.");
						System.Console.WriteLine(string.Format("{0} disabled welcome message.", m.GetPlayerName(player)));
						return true;
					}
					if (option.Equals("count", StringComparison.InvariantCultureIgnoreCase))
					{
						if (args.Length < 2)
						{
							//Too few arguments given
							throw new Exception("Invalid arguments supplied.");
						}
						settings.announceCount = int.Parse(args[1]);
						System.Console.WriteLine("Number of announcements changed - forcing file check.");
						CheckFiles();
						SaveSettings();
						m.SendMessage(player, string.Format(chatPrefix + "&2Number of announcements set to {0}.", settings.announceCount));
						System.Console.WriteLine(string.Format("{0} set number of announcements to {1}.", m.GetPlayerName(player), settings.announceCount));
						return true;
					}
					if (option.Equals("interval", StringComparison.InvariantCultureIgnoreCase))
					{
						if (args.Length < 2)
						{
							//Too few arguments given
							throw new Exception("Invalid arguments supplied.");
						}
						minuteCounter = 0;
						settings.announceInterval = int.Parse(args[1]);
						SaveSettings();
						m.SendMessage(player, string.Format(chatPrefix + "&2Announcement interval set to {0} minutes.", settings.announceInterval));
						System.Console.WriteLine(string.Format("{0} set announcement interval to {1} minutes.", m.GetPlayerName(player), settings.announceInterval));
						return true;
					}
				}
				catch
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "Invalid arguments. See /announcements help for details.");
					return true;
				}
				return true;
			}
			return false;
		}
	}
}
