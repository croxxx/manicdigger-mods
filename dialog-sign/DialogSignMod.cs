/*
 * DialogSign Mod - Version 1.1
 * last change: 2013-12-25
 * Author: croxxx
 * 
 * This mod adds sign to the game. Compatible with the other sign mods.
 * This version fixes ALL KNOWN issues that cause server crashes on startup.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace ManicDigger.Mods
{
	public class DialogSignMod : IMod
	{
		//General Settings
		int BGSizeWidth = 320;
		int BGSizeHeight = 160;
		string signTitle = "Sign";
		bool includeNames = true;

		//Fonts
		static DialogFont TitleFont = new DialogFont("Courier New", 30f, DialogFontStyle.Bold);
		static DialogFont NormalFont = new DialogFont("Courier New", 10f, DialogFontStyle.Regular);

		//Colors
		static int ColorFontTitle = Color.Black.ToArgb();
		static int ColorFontText = Color.Black.ToArgb();
		static int ColorEscTest = Color.White.ToArgb();
		static int ColorWindowBackground = Color.SaddleBrown.ToArgb();

		public void PreStart(ModManager m) { }
		public void Start(ModManager m)
		{
			this.m = m;
			m.RegisterOnBlockUse(OnUse);
			m.RegisterOnBlockDelete(OnDelete);
			SoundSet sounds = new SoundSet()
			{
				Walk = new string[] { "walk1", "walk2", "walk3", "walk4" },
				Break = new string[] { "destruct" },
				Build = new string[] { "build" },
				Clone = new string[] { "clone" },
			};

			m.SetBlockType(155, "Sign", new BlockType()
			{
				AllTextures = "Sign",
				DrawType = DrawType.Ladder,
				WalkableType = WalkableType.Solid,
				Sounds = sounds,
				IsUsable = true,
			});
			m.AddToCreativeInventory("Sign");
			m.RegisterOnCommand(OnCommand);
			m.RegisterOnDialogClick(OnClick);
			m.RegisterOnLoad(OnLoad);
		}

		ModManager m;
		bool signexists;

		void OnLoad()
		{
			if (!File.Exists("UserData/signdic.txt"))
			{
				File.Create("UserData/signdic.txt").Close();
				System.Console.WriteLine("signdic.txt created!");
			}
			else
			{
				int counter = 0;
				string[] lines = System.IO.File.ReadAllLines("UserData/signdic.txt");
				foreach (string line in lines)
				{
					counter++;
					string logs = string.Format(line);
					string[] linesplit = logs.Split(';');
					try
					{
						if (m.GetBlock(Convert.ToInt32(linesplit[0]), Convert.ToInt32(linesplit[1]), Convert.ToInt32(linesplit[2])) != m.GetBlockId("Sign"))
						{
							Array.Clear(lines, counter - 1, 1);
						}
					}
					catch
					{
						//Delete line if invalid (fixes crash when people use line-breaks)
						Console.WriteLine("Invalid entry on line {0} detected. Deleted.", counter);
						Array.Clear(lines, counter - 1, 1);
					}
				}
				using (StreamWriter Cleaner = new StreamWriter("UserData/signdic.txt"))
				{
					foreach (string entry in lines)
					{
						if (!String.IsNullOrEmpty(entry) && !String.IsNullOrEmpty(entry.Trim()))
						{
							Cleaner.WriteLine(entry);
						}
					}
				}
			}
		}

		void OnDelete(int player, int x, int y, int z, int oldblock)
		{
			if (oldblock == m.GetBlockId("Sign"))
			{
				DeleteEntry(x, y, z);
			}
		}

		void DeleteEntry(int x, int y, int z)
		{
			int counter = 0;
			string[] lines = System.IO.File.ReadAllLines("UserData/signdic.txt");
			foreach (string line in lines)
			{
				string logs = string.Format(line);
				counter++;
				string[] linesplit = logs.Split(';');
				if (x == Convert.ToInt32(linesplit[0]))
				{
					signexists = true;
					if (y == Convert.ToInt32(linesplit[1]))
					{
						signexists = true;
						if (z == Convert.ToInt32(linesplit[2]))
						{
							signexists = true;
							Array.Clear(lines, counter - 1, 1);
						}
						else
						{
							signexists = false;
						}
					}
					else
					{
						signexists = false;
					}
				}
				else
				{
					signexists = false;
				}
			}
			if (signexists == true)
			{
				using (StreamWriter Cleaner = new StreamWriter("UserData/signdic.txt"))
				{
					foreach (string entry in lines)
					{
						if (!String.IsNullOrEmpty(entry) && !String.IsNullOrEmpty(entry.Trim()))
						{
							Cleaner.WriteLine(entry);
						}
					}
				}
			}
		}

		bool OnCommand(int player, string command, string argument)
		{
			int xx = (int)m.GetPlayerPositionX(player);
			int yy = (int)m.GetPlayerPositionY(player);
			int zz = (int)m.GetPlayerPositionZ(player);
			if (command.Equals("sign", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!m.PlayerHasPrivilege(player, "sign"))
				{
					m.SendMessage(player, m.colorError() + "Insufficient privileges to set sign messages.");
					return true;
				}
				//Check for characters that cause mod crashes and output error message.
				if (argument.Contains(";") || argument.Contains("{") || argument.Contains("}"))
				{
					m.SendMessage(player, m.colorError() + "Illegal character! Not allowed are ; { and }");
					return true;
				}
				string message;
				try
				{
					message = argument;
				}
				catch
				{
					m.SendMessage(player, m.colorError() + "Invalid arguments. Type /help to see command's usage.");
					return true;
				}
				if (m.GetBlock(xx, yy, zz) != m.GetBlockId("Sign"))
				{
					m.SendMessage(player, m.colorError() + "At that postion isn't a sign!");
				}
				else
				{
					DeleteEntry(xx, yy, zz);
					string log;
					if (includeNames)
					{
						log = (xx + ";" + yy + ";" + zz + ";" + message + "  --" + m.GetPlayerName(player));
					}
					else
					{
						log = (xx + ";" + yy + ";" + zz + ";" + message);
					}
					File.AppendAllText("UserData/signdic.txt", log + Environment.NewLine);
					System.Console.WriteLine(string.Format("{0} set message for sign at {1},{2},{3}", m.GetPlayerName(player), xx, yy, zz));
					m.SendMessage(player, "&7Sign message set!");
				}
				return true;
			}
			return false;
		}

		void OnClick(int player, string widgetid)
		{
			if (widgetid == "SignEsc")
			{
				m.SendDialog(player, "SignDialog", null);
			}
		}

		Dialog GenerateDialog(string id, string title, string content, float width, float height)
		{
			Dialog d = new Dialog();
			d.IsModal = true;
			List<Widget> widgets = new List<Widget>();
			float boxW = width;
			float boxH = height;
			float boxX = 0 - (boxW / 2);
			float boxY = 0 - (boxH / 2);

			//Add background image
			Widget background = new Widget();
			background.X = (int)boxX;
			background.Y = (int)boxY;
			background.Width = BGSizeWidth;
			background.Height = BGSizeHeight;
			background.Image = "SignBG";
			widgets.Add(background);
			//Add title string
			widgets.Add(Widget.MakeText(title, TitleFont, boxX + 20, boxY + 20, ColorFontTitle));

			//Text
			float textX = boxX + 25;
			float textY = boxY + 75;
			float textW = boxW - 40;

			//Take a general line height and just use that everytime.
			float lineHeight = MeasureText(NormalFont, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890~`|\\/{}[]()").Height;

			string[] contentArray = content.Split('\n');
			foreach (string text in contentArray)
			{
				SizeF s = MeasureText(NormalFont, text);
				if (s.Width < textW)
				{
					widgets.Add(Widget.MakeText(text, NormalFont, textX, textY, ColorFontText));
					textY += lineHeight;
				}
				else
				{
					string[] wordArray = text.Split(' ');
					string curLine = "";
					string prevLine = "";
					int index = 0;
					float curLineW = 0;

					while (index < wordArray.Length)
					{
						curLine += wordArray[index] + " ";
						curLineW = MeasureText(NormalFont, curLine).Width;

						if (curLineW > textW)
						{
							widgets.Add(Widget.MakeText(prevLine, NormalFont, textX, textY, ColorFontText));
							textY += lineHeight;
							prevLine = "";

							//Setup current word again.
							if (index < wordArray.Length)
							{
								curLine = wordArray[index] + " ";
								curLineW = MeasureText(NormalFont, curLine).Width;
							}
							else
							{
								curLine = "";
								curLineW = 0;
							}
						}
						if (index == wordArray.Length - 1)
						{
							widgets.Add(Widget.MakeText(curLine, NormalFont, textX, textY, ColorFontText));
							textY += lineHeight;
						}
						prevLine = curLine;
						index++;
					}
				}

			}

			Widget okBox = new Widget();
			okBox.ClickKey = (char)27;
			okBox.Id = "SignEsc";
			widgets.Add(okBox);

			string esc = "Press ESC to Close";
			SizeF escRect = MeasureText(NormalFont, esc);
			float escX = boxX + boxW - escRect.Width - 5f;
			widgets.Add(Widget.MakeSolid(escX - 5f, boxY + (boxH + 5f), escRect.Width + 10f, escRect.Height + 10f, ColorWindowBackground));
			widgets.Add(Widget.MakeText(esc, NormalFont, escX, boxY + (boxH + 10), ColorEscTest));

			d.Widgets = widgets.ToArray();
			return d;
		}

		protected SizeF MeasureText(DialogFont f, string text)
		{
			Font font = new Font(f.FamilyName, f.Size);

			StringFormat format = new StringFormat(StringFormat.GenericDefault);
			RectangleF rectangle = new RectangleF(0, 0, 2000, 1000);
			CharacterRange[] ranges = { new CharacterRange(0, text.Length) };
			Region[] regions = new Region[1];

			try
			{
				format.SetMeasurableCharacterRanges(ranges);
				format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

				using (Bitmap b = new Bitmap(2000, 1000))
				{
					using (Graphics g = Graphics.FromImage(b))
					{
						regions = g.MeasureCharacterRanges(text, font, rectangle, format);
						rectangle = regions[0].GetBounds(g);
					}
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}

			return rectangle.Size;
		}

		void OnUse(int player, int x, int y, int z)
		{
			if (m.GetBlock(x, y, z) == m.GetBlockId("Sign"))
			{
				if (!File.Exists("UserData/signdic.txt"))
				{
					File.Create("UserData/signdic.txt").Close();
					m.SendMessage(player, m.colorError() + "File created!");
				}
				else
				{
					bool entryexists = false;
					string[] lines = System.IO.File.ReadAllLines("UserData/signdic.txt");
					foreach (string line in lines)
					{
						string logs = string.Format(line);
						string[] entry = logs.Split(';');
						if (x == Convert.ToInt32(entry[0]))
						{
							if (y == Convert.ToInt32(entry[1]))
							{
								if (z == Convert.ToInt32(entry[2]))
								{
									entryexists = true;
									string message = Convert.ToString(entry[3]);
									m.SendDialog(player, "SignDialog", GenerateDialog("SignDialog", signTitle, message, BGSizeWidth, BGSizeHeight));
									break;
								}
							}
						}

					}
					if (entryexists == false)
					{
						m.SendDialog(player, "SignDialog", GenerateDialog("SignDialog", signTitle, "This sign is empty", BGSizeWidth, BGSizeHeight));
					}
				}
			}
		}
	}

	public class DialogText
	{
		public DialogText() { }
		public DialogText(string filename, string title, string contents)
		{
			this.FileName = filename;
			this.Title = title;
			this.Contents = contents;
		}
		public string FileName { get; set; }
		public string Id { get { return this.FileName.Substring(0, this.FileName.LastIndexOf('.')); } }
		public string Title { get; set; }
		public string Contents { get; set; }
	}
}
