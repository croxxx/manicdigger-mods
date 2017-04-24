/*
 * Noise3DWorldGenerator
 * Author: croxxx
 * Last change: 2013-09-15
 * 
 * This is a conversion of a rather unknown world generator included in early versions of Manic Digger.
 */

using System;

namespace ManicDigger.Mods
{
	public class Noise3DWorldGenerator : IMod
	{
		private ModManager m;
		//Generator variables
		private Random _rnd;
		private byte[,] heightcache;
		private double maxnoise = 0.32549166667822577d;
		private double[,,] interpolatednoise;

		private int waterlevel = 20;
		private int ChunkSize;
		private int Seed;

		private int TileIdEmpty;
		private int TileIdGrass;
		private int TileIdDirt;
		private int TileIdStone;
		private int TileIdLava;

		public void PreStart(ModManager m)
		{
			m.RequireMod("Default");
		}

		public void Start(ModManager m)
		{
			this.m = m;
			m.RegisterWorldGenerator(GetChunk);

			this.TileIdEmpty = m.GetBlockId("Empty");
			this.TileIdStone = m.GetBlockId("Stone");
			this.TileIdDirt = m.GetBlockId("Dirt");
			this.TileIdGrass = m.GetBlockId("Grass");
			this.TileIdLava = m.GetBlockId("Lava");

			this.ChunkSize = m.GetChunkSize();
			_rnd = new Random();
			this.Seed = m.GetSeed();
		}

		void GetChunk(int x, int y, int z, ushort[] chunk)
		{
			heightcache = new byte[this.ChunkSize, this.ChunkSize];
			x *= this.ChunkSize;
			y *= this.ChunkSize;
			z *= this.ChunkSize;
			for (int xx = 0; xx < this.ChunkSize; xx++)
			{
				for (int yy = 0; yy < this.ChunkSize; yy++)
				{
					heightcache[xx, yy] = GetHeight(x + xx, y + yy);
				}
			}
			interpolatednoise = InterpolateNoise3d(x, y, z, this.ChunkSize);

			for (int xx = 0; xx < this.ChunkSize; xx++)
			{
				for (int yy = 0; yy < this.ChunkSize; yy++)
				{
					for (int zz = 0; zz < this.ChunkSize; zz++)
					{
						int pos = m.Index3d(xx, yy, zz, this.ChunkSize, this.ChunkSize);
						chunk[pos] = (ushort)GetBlock(x + xx, y + yy, z + zz, heightcache[xx, yy], 0, xx, yy, zz);
					}
				}
			}
			for (int xx = 0; xx < this.ChunkSize; xx++)
			{
				for (int yy = 0; yy < this.ChunkSize; yy++)
				{
					int v = 1;
					for (int zz = this.ChunkSize - 2; zz >= 0; zz--)
					{
						int pos = m.Index3d(xx, yy, zz, this.ChunkSize, this.ChunkSize);
						if (chunk[pos] == TileIdEmpty) { v = 0; }
						if (chunk[pos] == TileIdGrass)
						{
							if (v == 0)
							{
							}
							else if (v < 4)
							{
								chunk[pos] = (ushort)TileIdDirt;
							}
							else
							{
								chunk[pos] = (ushort)TileIdStone;
							}
							v++;
						}
					}

				}
			}
			if (z == 0)
			{
				for (int xx = 0; xx < this.ChunkSize; xx++)
				{
					for (int yy = 0; yy < this.ChunkSize; yy++)
					{
						int pos = m.Index3d(xx, yy, 0, this.ChunkSize, this.ChunkSize);
						chunk[pos] = (ushort)this.TileIdLava;
					}
				}
			}
		}

		private int GetBlock(int x, int y, int z, int height, int special, int xx, int yy, int zz)
		{
			double d = interpolatednoise[xx, yy, zz];
			if ((d + maxnoise) > ((double)z / 128) * (maxnoise * 2))
			{
				return TileIdGrass;
			}
			return 0;
		}

		private byte GetHeight(int x, int y)
		{
			x += 30; y -= 30;
			double p = 0.5;
			double zoom = 150;
			double getnoise = 0;
			int octaves = 6;
			for (int a = 0; a < octaves - 1; a++)//This loops trough the octaves.
			{
				double frequency = Math.Pow(2, a);//This increases the frequency with every loop of the octave.
				double amplitude = Math.Pow(p, a);//This decreases the amplitude with every loop of the octave.
				getnoise += Noise(((double)x) * frequency / zoom, ((double)y) / zoom * frequency, this.Seed) * amplitude;//This uses our perlin noise functions. It calculates all our zoom and frequency and amplitude
			}
			double maxheight = 64;
			int height = (int)(((getnoise + 1) / 2.0) * (maxheight - 5)) + 3;//(int)((getnoise * 128.0) + 128.0);
			if (height > maxheight - 1) { height = (int)maxheight - 1; }
			if (height < 2) { height = 2; }
			return (byte)height;
		}

		//From NoiseTools
		public static double Noise(double x, double y, int seed)
		{
			double floorx = (double)((int)x);//This is kinda a cheap way to floor a double integer.
			double floory = (double)((int)y);
			double s, t, u, v;//Integer declaration
			s = FindNoise2(floorx, floory, seed);
			t = FindNoise2(floorx + 1, floory, seed);
			u = FindNoise2(floorx, floory + 1, seed);//Get the surrounding pixels to calculate the transition.
			v = FindNoise2(floorx + 1, floory + 1, seed);
			double int1 = Interpolate(s, t, x - floorx);//Interpolate between the values.
			double int2 = Interpolate(u, v, x - floorx);//Here we use x-floorx, to get 1st dimension. Don't mind the x-floorx thingie, it's part of the cosine formula.
			return Interpolate(int1, int2, y - floory);//Here we use y-floory, to get the 2nd dimension.
		}

		public static double[,,] InterpolateNoise3d(double x, double y, double z, int chunksize)
		{
			double[,,] noise = new double[chunksize, chunksize, chunksize];
			int n = 8;
			for (int xx = 0; xx < chunksize; xx += n)
			{
				for (int yy = 0; yy < chunksize; yy += n)
				{
					for (int zz = 0; zz < chunksize; zz += n)
					{
						double f000 = GetNoise(x + xx, y + yy, z + zz);
						double f100 = GetNoise(x + xx + (n - 1), y + yy, z + zz);
						double f010 = GetNoise(x + xx, y + yy + (n - 1), z + zz);
						double f110 = GetNoise(x + xx + (n - 1), y + yy + (n - 1), z + zz);
						double f001 = GetNoise(x + xx, y + yy, z + zz + (n - 1));
						double f101 = GetNoise(x + xx + (n - 1), y + yy, z + zz + (n - 1));
						double f011 = GetNoise(x + xx, y + yy + (n - 1), z + zz + (n - 1));
						double f111 = GetNoise(x + xx + (n - 1), y + yy + (n - 1), z + zz + (n - 1));
						for (int ix = 0; ix < n; ix++)
						{
							for (int iy = 0; iy < n; iy++)
							{
								for (int iz = 0; iz < n; iz++)
								{
									noise[xx + ix, yy + iy, zz + iz] = Trilinear((double)ix / (n - 1), (double)iy / (n - 1), (double)iz / (n - 1),
																				 f000, f010, f100, f110, f001, f011, f101, f111);
								}
							}
						}
					}
				}
			}
			return noise;
		}

		public static double Interpolate(double a, double b, double x)
		{
			double ft = x * 3.1415927;
			double f = (1.0 - Math.Cos(ft)) * 0.5;
			return a * (1.0 - f) + b * f;
		}

		public static double FindNoise2(double x, double y, int seed)
		{
			int n = (int)x + (int)y * 57;
			return FindNoise1(n, seed);
		}

		public static double FindNoise1(int n, int seed)
		{
			n += seed;
			n = (n << 13) ^ n;
			int nn = (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
			return 1.0 - ((double)nn / 1073741824.0);
		}

		public static double Trilinear(double x, double y, double z,
									   double f000, double f010, double f100, double f110,
									   double f001, double f011, double f101, double f111)
		{
			double up0 = (f100 - f000) * x + f000;
			double down0 = (f110 - f010) * x + f010;
			double all0 = (down0 - up0) * y + up0;

			double up1 = (f101 - f001) * x + f001;
			double down1 = (f111 - f011) * x + f011;
			double all1 = (down1 - up1) * y + up1;

			return (all1 - all0) * z + all0;
		}

		public static double GetNoise(double x, double y, double z)
		{
			return Noise3.noise((double)(x) / 50, (double)(y) / 50, (double)(z) / 50);
		}
	}

	public static class Noise3
	{
		static int i, j, k;
		static int[] A = new[] { 0, 0, 0 };
		static double u, v, w;
		public static double noise(double x, double y, double z)
		{
			double s = (x + y + z) / 3;
			i = (int)Math.Floor(x + s); j = (int)Math.Floor(y + s); k = (int)Math.Floor(z + s);
			s = (i + j + k) / 6.0; u = x - i + s; v = y - j + s; w = z - k + s;
			A[0] = A[1] = A[2] = 0;
			int hi = u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2;
			int lo = u < w ? u < v ? 0 : 1 : v < w ? 1 : 2;
			return K(hi) + K(3 - hi - lo) + K(lo) + K(0);
		}
		static double K(int a)
		{
			double s = (A[0] + A[1] + A[2]) / 6.0;
			double x = u - A[0] + s, y = v - A[1] + s, z = w - A[2] + s, t = .6 - x * x - y * y - z * z;
			int h = shuffle(i + A[0], j + A[1], k + A[2]);
			A[a]++;
			if (t < 0)
				return 0;
			int b5 = h >> 5 & 1, b4 = h >> 4 & 1, b3 = h >> 3 & 1, b2 = h >> 2 & 1, b = h & 3;
			double p = b == 1 ? x : b == 2 ? y : z, q = b == 1 ? y : b == 2 ? z : x, r = b == 1 ? z : b == 2 ? x : y;
			p = (b5 == b3 ? -p : p); q = (b5 == b4 ? -q : q); r = (b5 != (b4 ^ b3) ? -r : r);
			t *= t;
			return 8 * t * t * (p + (b == 0 ? q + r : b2 == 0 ? q : r));
		}
		static int shuffle(int i, int j, int k)
		{
			return b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) +
				b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7);
		}
		static int b(int i, int j, int k, int B) { return T[b(i, B) << 2 | b(j, B) << 1 | b(k, B)]; }
		static int b(int N, int B) { return N >> B & 1; }
		static int[] T = new[] { 0x15, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a };
	}
}
