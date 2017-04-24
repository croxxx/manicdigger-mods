/*
 * TooManyColors Mod - Version 1.0
 * Author: croxxx
 * Last change: 2013-07-11
 * 
 * This Mod adds a ton of new single-color blocks for building pleasure
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ManicDigger.Mods
{
	public class TooManyColors : IMod
	{
		public void PreStart(ModManager m)
		{
			//m.RequireMod("Default");
		}
		public void Start(ModManager m)
		{
			this.m = m;

			SoundSet solidSounds = new SoundSet()
			{
				Walk = new string[] { "walk1", "walk2", "walk3", "walk4" },
				Break = new string[] { "destruct" },
				Build = new string[] { "build" },
				Clone = new string[] { "clone" },
			};

			int blockCount = 0;

			for (int r = 0; r < 16; r += 3)
			{
				for (int g = 0; g < 16; g += 3)
				{
					for (int b = 0; b < 16; b += 3)
					{
						int r32 = r * 17;
						int g32 = g * 17;
						int b32 = b * 17;

						string blockName = string.Format("R{0}G{1}B{2}", r32.ToString(), g32.ToString(), b32.ToString());

						m.SetBlockType(blockName, new BlockType()
						{
							AllTextures = blockName,
							DrawType = DrawType.Solid,
							WalkableType = WalkableType.Solid,
							Sounds = solidSounds,
						});
						m.AddToCreativeInventory(blockName);
						blockCount++;
					}
				}
			}
			System.Console.WriteLine(string.Format("INFO:   TooManyColors Mod v1.0 loaded. {0} blocks registered.", blockCount));
		}

		ModManager m;
	}
}


/* Below is the code I used to generate the .png files.
 * Feel free to use it to create your own set.
 * 
 * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 * 
 * for (int r = 0; r < 16; r += 3)
            {
                for (int g = 0; g < 16; g += 3)
                {
                    for (int b = 0; b < 16; b += 3)
                    {
                        int r32 = r * 17;
                        int g32 = g * 17;
                        int b32 = b * 17;

                        Color c = Color.FromArgb(r32, g32, b32);

                        Bitmap tmp = new Bitmap(32, 32);
                        for (int x = 0; x < 32; x++)
                        {
                            for (int y = 0; y < 32; y++)
                            {
                                tmp.SetPixel(x, y, c);
                            }
                        }
                        tmp.Save(string.Format("R{0}G{1}B{2}.png", r32.ToString(), g32.ToString(), b32.ToString()),System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }*/
