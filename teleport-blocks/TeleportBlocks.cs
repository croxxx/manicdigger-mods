/*
 * Teleport Blocks Mod - Version 1.2
 * Author: croxxx
 * Last change: 2015-01-28
 * 
 * This mod adds "Teleport Blocks" to your server.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ManicDigger.Mods
{
	public class TeleportBlocks : IMod
	{
		public void PreStart(ModManager m) { }
		public void Start(ModManager m)
		{
			this.m = m;

			m.RegisterOnBlockUse(Teleport);
			m.RegisterOnBlockBuild(AddBlock);
			m.RegisterOnBlockDelete(DeleteBlock);
			m.RegisterPrivilege("use_tpblock");
			m.RegisterPrivilege("set_tptarget");
			m.RegisterPrivilege("get_tpblocks");
			m.RegisterPrivilege("reload_tpblocks");
			m.RegisterPrivilege("delete_tpblocks");
			m.RegisterPrivilege("delete_all_tpblocks");
			m.RegisterCommandHelp("use_tpblock", "Allows you to use teleport blocks");
			m.RegisterCommandHelp("set_tptarget", "/set_tptarget [blockID] [X] [Y] [Z]");
			m.RegisterCommandHelp("get_tpblocks", "Gives you all available teleport blocks");
			m.RegisterCommandHelp("reload_tpblocks", "Reloads the list of teleport blocks");
			m.RegisterCommandHelp("delete_all_tpblocks", "Deletes ALL teleport blocks and their targets");
			m.RegisterOnCommand(SetTarget);
			m.RegisterOnCommand(GiveBlocks);
			m.RegisterOnCommand(ReloadTargets);
			m.RegisterOnCommand(DeleteAll);
			//m.RegisterOnLoad(LoadTargetPositions);

			SoundSet solidSounds = new SoundSet()
			{
				Walk = new string[] { "walk1", "walk2", "walk3", "walk4" },
				Break = new string[] { "destruct" },
				Build = new string[] { "build" },
				Clone = new string[] { "clone" },
			};

			Console.WriteLine(consolePrefix + "Registering {0} teleport BlockTypes...", number_of_teleport_blocks);
			for (int i = 0; i < number_of_teleport_blocks; i++)
			{
				m.SetBlockType(blockID_start + i, "Teleport_Block_" + i.ToString(), new BlockType()
				{
					AllTextures = "Teleport_" + i.ToString(),
					DrawType = DrawType.Transparent,
					WalkableType = WalkableType.Solid,
					Sounds = solidSounds,
					IsUsable = true,
				});
			}
			Console.WriteLine(consolePrefix + "Done.");

			// Load targets here as RegisterOnLoad is kinda broken...
			LoadTargetPositions();
			Console.WriteLine(consolePrefix + "Loaded Mod Version 1.2");
			m.LogServerEvent(consolePrefix + "Loaded Mod Version 1.2");
		}

		ModManager m;
		List<TeleportBlock> tpblocks = new List<TeleportBlock>();   //Stores positions and targets of all teleport blocks
		string chatPrefix = "&8[&6TPBlocks&8] ";
		string consolePrefix = "[TeleportBlocks] ";

		const int number_of_teleport_blocks = 10;                   //The total number of teleport blocks that shall be available
		const int blockID_start = 245;                              //The block ID of the first teleport block

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

		struct TeleportBlock
		{
			public TeleportBlock(Vector3i pos, Vector3i tar)
			{
				position = pos;
				target = tar;
				isPlaced = false;
			}
			public Vector3i position;
			public Vector3i target;
			public bool isPlaced;
		}

		bool IsTargetValid(Vector3i vector)
		{
			if ((vector.x == -1) || (vector.y == -1) || (vector.z == -1))
			{
				return false;
			}
			return true;
		}

		bool IsTeleportBlock(int BlockID)
		{
			if ((BlockID >= blockID_start) && (BlockID < (blockID_start + number_of_teleport_blocks)))
			{
				return true;
			}
			return false;
		}

		void LoadTargetPositions()
		{
			try
			{
				tpblocks.Clear();
				if (!File.Exists("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt"))
				{
					SaveTargetPositions();
				}
				Console.WriteLine(consolePrefix + "Loading teleport blocks...");
				using (TextReader tr = new StreamReader("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt", System.Text.Encoding.UTF8))
				{
					string positionString = tr.ReadLine();
					while (!string.IsNullOrEmpty(positionString))
					{
						string[] positionCoords = positionString.Split(';');
						int px = int.Parse(positionCoords[0]);
						int py = int.Parse(positionCoords[1]);
						int pz = int.Parse(positionCoords[2]);

						string targetString = tr.ReadLine();
						string[] targetCoords = targetString.Split(';');
						int tx = int.Parse(targetCoords[0]);
						int ty = int.Parse(targetCoords[1]);
						int tz = int.Parse(targetCoords[2]);

						bool pl = bool.Parse(tr.ReadLine());

						TeleportBlock block = new TeleportBlock(new Vector3i(px, py, pz), new Vector3i(tx, ty, tz));
						block.isPlaced = pl;
						tpblocks.Add(block);
						Console.WriteLine(consolePrefix + "Loaded {0}: {1}{2}{3} Target: {4}{5}{6} Placed: {7}", tpblocks.Count - 1, block.position.x, block.position.y, block.position.z, block.target.x, block.target.y, block.target.z, block.isPlaced);

						positionString = tr.ReadLine();
					}
				}
				if (number_of_teleport_blocks != tpblocks.Count)
				{
					Console.WriteLine(consolePrefix + "Error while loading blocks file. Deleted.");
					File.Delete("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt");
					LoadTargetPositions();
				}
				Console.WriteLine(consolePrefix + "Successfully loaded.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(consolePrefix + "ERROR:  " + ex.Message);
			}
		}

		void SaveTargetPositions()
		{
			try
			{
				if ((!File.Exists("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt")) && tpblocks.Count != number_of_teleport_blocks)
				{
					Console.WriteLine(consolePrefix + "TeleportBlocks.txt does not exist. Create new.");
					using (StreamWriter sw = new StreamWriter("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt", false, System.Text.Encoding.UTF8))
					{
						for (int i = 0; i < number_of_teleport_blocks; i++)
						{
							sw.WriteLine("-1;-1;-1");
							sw.WriteLine("-1;-1;-1");
							sw.WriteLine(false);
						}
					}
					Console.WriteLine(consolePrefix + "Done.");
					return;
				}
				using (StreamWriter sw = new StreamWriter("UserData" + Path.DirectorySeparatorChar + "TeleportBlocks.txt", false, System.Text.Encoding.UTF8))
				{
					for (int i = 0; i < tpblocks.Count; i++)
					{
						sw.WriteLine(tpblocks[i].position.x + ";" + tpblocks[i].position.y + ";" + tpblocks[i].position.z);
						sw.WriteLine(tpblocks[i].target.x + ";" + tpblocks[i].target.y + ";" + tpblocks[i].target.z);
						sw.WriteLine(tpblocks[i].isPlaced);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(consolePrefix + "ERROR:  " + ex.Message);
			}
			Console.WriteLine(string.Format("Saved all teleport blocks."));
		}

		void Teleport(int player, int x, int y, int z)
		{
			int ID = m.GetBlock(x, y, z);
			if (IsTeleportBlock(ID))
			{
				if (!m.PlayerHasPrivilege(player, "use_tpblock"))
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to use teleport blocks.");
					return;
				}
				int relativeID = ID - blockID_start;
				if (tpblocks.Count <= relativeID)
				{
					//Should not happen. Did happen, though...
					Console.WriteLine(consolePrefix + "ERROR: Tried to use not existing teleport block!");
					Console.WriteLine(consolePrefix + "       Block ID: {0}, Relative ID: {1}, ", ID, relativeID);
					m.SendMessage(player, chatPrefix + m.colorError() + "Internal error! More details on console.");
					return;
				}
				if (IsTargetValid(tpblocks[relativeID].target))
				{
					Vector3i targetPosition = tpblocks[relativeID].target;
					m.SetPlayerPosition(player, targetPosition.x, targetPosition.y, targetPosition.z);
					return;
				}
				else
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "This block has no target set.");
					return;
				}
			}
		}

		void AddBlock(int player, int x, int y, int z)
		{
			int ID = m.GetBlock(x, y, z);
			if (IsTeleportBlock(ID))
			{
				int ID2 = ID - blockID_start;
				TeleportBlock block = tpblocks[ID2];
				if (block.isPlaced)                                 //Check if Block with same ID is already set
				{
					Vector3i oldPos = block.position;               //Get position of old block
					m.SetBlock(oldPos.x, oldPos.y, oldPos.z, 0);    //Set block at old position to "Empty"
					block.position = new Vector3i(x, y, z);         //Save position of new block
					m.SendMessage(player, chatPrefix + "&aTeleport Block position updated. Old block deleted.");
				}
				else
				{
					block.position = new Vector3i(x, y, z);         //Save position of new block
					block.isPlaced = true;                          //Set BlockID as used
				}
				tpblocks[ID2] = block;
				if (!IsTargetValid(tpblocks[ID2].target))
				{
					m.SendMessage(player, chatPrefix + "&6Remember to set a target for this teleport block.");  //Notify user if there is no target set
				}
				SaveTargetPositions();
			}
		}

		void DeleteBlock(int player, int x, int y, int z, int BlockID)
		{
			if (IsTeleportBlock(BlockID))
			{
				if (!m.PlayerHasPrivilege(player, "delete_tpblocks"))
				{
					m.SetBlock(x, y, z, BlockID);
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to delete teleport blocks.");
					return;
				}
				int ID = BlockID - blockID_start;
				TeleportBlock block = tpblocks[ID];
				block.position = new Vector3i(-1, -1, -1);
				block.target = new Vector3i(-1, -1, -1);
				block.isPlaced = false;
				tpblocks[ID] = block;
				m.SendMessage(player, chatPrefix + "&aThe block and its target position have been deleted.");
				SaveTargetPositions();
			}
		}

		bool SetTarget(int player, string command, string argument)
		{
			if (command.Equals("set_tptarget", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "set_tptarget"))
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to set teleport block targets.");
					return true;
				}
				int teleportBlockID, targetX, targetY, targetZ;
				try
				{
					string[] args = argument.Split(' ');
					teleportBlockID = int.Parse(args[0]);
					targetX = int.Parse(args[1]);
					targetY = int.Parse(args[2]);
					targetZ = int.Parse(args[3]);
				}
				catch
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "Invalid arguments. Try /help.");
					return true;
				}

				if ((teleportBlockID < number_of_teleport_blocks) && (teleportBlockID >= 0))
				{
					if (m.IsValidPos(targetX, targetY, targetZ))
					{
						TeleportBlock block = tpblocks[teleportBlockID];
						block.target = new Vector3i(targetX, targetY, targetZ);
						tpblocks[teleportBlockID] = block;
						m.SendMessage(player, chatPrefix + "&aSuccesfully set target coordinates:");
						m.SendMessage(player, chatPrefix + string.Format("&aTeleport Block {0} now teleports to {1}, {2}, {3}", teleportBlockID, targetX, targetY, targetZ));
						m.LogServerEvent(string.Format("[TeleportBlocks] {0} set Teleport Block {1} target to {2}, {3}, {4}", m.GetPlayerName(player), teleportBlockID, targetX, targetY, targetZ));
						SaveTargetPositions();
						return true;
					}
					else
					{
						m.SendMessage(player, chatPrefix + m.colorError() + "The coordinates you entered are not valid.");
						return true;
					}
				}
				else
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "The teleport block ID you entered does not exist.");
					return true;
				}
			}
			return false;
		}

		bool GiveBlocks(int player, string command, string argument)
		{
			if (command.Equals("get_tpblocks", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "get_tpblocks"))
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to give yourself teleport blocks.");
					return true;
				}
				for (int i = blockID_start; i < (blockID_start + number_of_teleport_blocks); i++)
				{
					m.GrabBlock(player, i);
				}
				m.NotifyInventory(player);
				m.SendMessage(player, chatPrefix + "&aAdded teleport blocks to inventory.");
				return true;
			}
			return false;
		}

		bool DeleteAll(int player, string command, string argument)
		{
			if (command.Equals("delete_all_tpblocks", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "delete_all_tpblocks"))
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to delete ALL teleport blocks.");
					return true;
				}
				for (int i = 0; i < number_of_teleport_blocks; i++)
				{
					Vector3i emptyVector = new Vector3i(-1, -1, -1);
					TeleportBlock block = tpblocks[i];
					block.target = emptyVector;
					Vector3i tempPos = block.position;
					m.SetBlock(tempPos.x, tempPos.y, tempPos.z, 0); //Set Block to "Empty"
					block.position = emptyVector;
					block.isPlaced = false;
					tpblocks[i] = block;
				}
				m.SendMessage(player, chatPrefix + "&aALL teleport blocks and their targets have been deleted.");
				SaveTargetPositions();
				return true;
			}
			return false;
		}

		bool ReloadTargets(int player, string command, string argument)
		{
			if (command.Equals("reload_tpblocks", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "reload_tpblocks"))
				{
					m.SendMessage(player, chatPrefix + m.colorError() + "You are not allowed to reload teleport blocks.");
					return true;
				}
				LoadTargetPositions();
				m.SendMessage(player, chatPrefix + "&aTeleport blocks have been reloaded.");
				return true;
			}
			return false;
		}
	}
}
