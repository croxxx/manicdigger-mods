/*
 * Switchable Lights Mod - Version 1.1
 * Last change: 2013-08-23
 * Author: croxxx
 * 
 * Thanks to "exit151" from the forums for finding multiple bugs in the first release!
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ManicDigger.Mods
{
	public class SwitchableLights : IMod
	{
		public void PreStart(ModManager m)
		{
			m.RequireMod("Default");
		}
		public void Start(ModManager m)
		{
			this.m = m;

			m.RegisterOnBlockUse(ToggleLight);
			m.RegisterOnBlockBuild(AddBlock);
			m.RegisterOnBlockDelete(DeleteBlock);
			m.RegisterPrivilege("toggle");
			m.RegisterPrivilege("toggle_all");
			m.RegisterCommandHelp("toggle_all", "/toggle_all [0 / 1]");
			m.RegisterCommandHelp("toggle", "Allows you to toggle single light blocks.");
			m.RegisterOnCommand(ToggleAll);
			m.RegisterOnLoad(LoadLights);
			m.RegisterOnSave(SaveLights);

			m.SetString("en", "Light_Active", "Active Light Block");
			m.SetString("en", "Light_Inactive", "Inactive Light Block");

			SoundSet solidSounds = new SoundSet()
			{
				Walk = new string[] { "walk1", "walk2", "walk3", "walk4" },
				Break = new string[] { "destruct" },
				Build = new string[] { "build" },
				Clone = new string[] { "clone" },
			};
			m.SetBlockType(254, name_on, new BlockType()
			{
				AllTextures = "light_on",
				DrawType = DrawType.Solid,
				WalkableType = WalkableType.Solid,
				Sounds = solidSounds,
				LightRadius = 10,
				IsUsable = true,
			});
			m.SetBlockType(255, name_off, new BlockType()
			{
				AllTextures = "light_off",
				DrawType = DrawType.Solid,
				WalkableType = WalkableType.Solid,
				Sounds = solidSounds,
				IsUsable = true,
			});
			light_on = m.GetBlockId(name_on);
			light_off = m.GetBlockId(name_off);
			//m.AddToCreativeInventory(name_on);
			m.AddToCreativeInventory(name_off);
			m.AddCraftingRecipe2(name_off, 1, "Torch", 3, "IronBlock", 2);
		}
		ModManager m;
		int light_on;
		int light_off;
		string name_on = "Light_Active";
		string name_off = "Light_Inactive";
		List<Vector3i> blocklist = new List<Vector3i>();

		//Taken from Tnt.cs
		struct Vector3i
		{
			public Vector3i(int x, int y, int z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
			public int x;
			public int y;
			public int z;
		}

		bool IsSwitchableLight(int BlockID)
		{
			if ((BlockID == m.GetBlockId(name_on)) || (BlockID == m.GetBlockId(name_off)))
			{
				return true;
			}
			return false;
		}

		//Load and Save taken from BuildLog.cs
		void LoadLights()
		{
			try
			{
				byte[] b = m.GetGlobalData("SwitchableLights");
				if (b != null)
				{
					MemoryStream ms = new MemoryStream(b);
					BinaryReader br = new BinaryReader(ms);
					int count = br.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						int loaded_x = br.ReadInt16(); //x
						int loaded_y = br.ReadInt16(); //y
						int loaded_z = br.ReadInt16(); //z
						var data = new Vector3i(loaded_x, loaded_y, loaded_z);
						blocklist.Add(data);
					}
				}
			}
			catch
			{
				//if data corrupted
				SaveLights();
			}
		}

		void SaveLights()
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);
			bw.Write((int)blocklist.Count);
			for (int i = 0; i < blocklist.Count; i++)
			{
				Vector3i data = blocklist[i];
				bw.Write((short)data.x); //x
				bw.Write((short)data.y); //y
				bw.Write((short)data.z); //z
			}
			m.SetGlobalData("SwitchableLights", ms.ToArray());
		}

		void ToggleLight(int player, int x, int y, int z)
		{
			if (IsSwitchableLight(m.GetBlock(x, y, z)))
			{
				if (!m.PlayerHasPrivilege(player, "toggle"))
				{
					m.SendMessage(player, m.colorError() + "You are not allowed to toggle lights.");
					return;
				}
				if (m.GetBlock(x, y, z) == light_off)
				{
					m.SetBlock(x, y, z, light_on);
					return;
				}
				if (m.GetBlock(x, y, z) == light_on)
				{
					m.SetBlock(x, y, z, light_off);
					return;
				}
			}
		}

		void AddBlock(int player, int x, int y, int z)
		{
			if (IsSwitchableLight(m.GetBlock(x, y, z)))
			{
				blocklist.Add(new Vector3i(x, y, z));
			}
		}

		void DeleteBlock(int player, int x, int y, int z, int block)
		{
			if (IsSwitchableLight(block))
			{
				int pos = blocklist.IndexOf(new Vector3i(x, y, z));
				if (pos != -1)
				{
					blocklist.RemoveAt(pos);
				}
			}
		}

		bool ToggleAll(int player, string command, string argument)
		{
			if (command.Equals("toggle_all", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "toggle_all"))
				{
					m.SendMessage(player, m.colorError() + "You are not allowed to toggle ALL lights at once.");
					return true;
				}
				int option;
				try
				{
					option = Convert.ToInt32(argument);
				}
				catch (FormatException)
				{
					m.SendMessage(player, m.colorError() + "Argument is not a number.");
					return true;
				}
				catch (OverflowException)
				{
					m.SendMessage(player, m.colorError() + "The number is too big for an Int32.");
					return true;
				}
				if (option == 0 || option == 1)
				{
					int BlockID = 0;
					if (option == 0)
					{
						BlockID = m.GetBlockId(name_off);
					}
					else
					{
						BlockID = m.GetBlockId(name_on);
					}
					foreach (Vector3i coords in blocklist)
					{
						if (m.GetBlock(coords.x, coords.y, coords.z) != BlockID)
						{
							m.SetBlock(coords.x, coords.y, coords.z, BlockID);
						}
					}
					return true;
				}
				m.SendMessage(player, m.colorError() + "Invalid arguments. Type /help to see command's usage.");
				return true;
			}
			return false;
		}
	}
}
