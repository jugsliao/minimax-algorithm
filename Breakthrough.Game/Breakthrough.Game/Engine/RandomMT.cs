using System;

namespace Breakthrough.Game.Engine
{
	/// <summary>
	/// A Pseudo-Random Number Generator (PRNG) - Mersenne Twister
    /// 
    /// The Mersenne Twister (MT) is a pseudorandom number generator (PRNG) 
    /// developed by Makoto Matsumoto and Takuji Nishimura[1][2] during 1996-1997. MT has the following merits:
    /// 
    ///  - It is designed with consideration on the flaws of various existing generators.
    ///  - Far longer period and far higher order of equidistribution than any other implemented generators. 
    ///    (It has been proven that the period is 2^19937-1 and 623-dimensional equidistribution property is assured.)
    ///  - Fast generation. (Although it depends on the system, it is reported that MT is sometimes faster
    ///    than the standard ANSI-C library in a system with pipeline and cache memory.)
    ///  - Efficient use of the memory.
	/// </summary>
	public class RandomMT
	{
		private const int	N				= 624;
		private const int	M				= 397;
		private const uint	K				= 0x9908B0DFU;
		private const uint	DEFAULT_SEED	= 4357;
        
		private ulong []	state			= new ulong[N+1];
		private int			next			= 0;
		private ulong		seedValue;


		public RandomMT()
		{
			SeedMT(DEFAULT_SEED);
		}
		public RandomMT(ulong _seed)
		{
			seedValue = _seed;
			SeedMT(seedValue);
		}

		public ulong RandomInt()
		{
			ulong y;

			if((next + 1) > N)
				return(ReloadMT());

			y  = state[next++];
			y ^= (y >> 11);
			y ^= (y <<  7) & 0x9D2C5680U;
			y ^= (y << 15) & 0xEFC60000U;
			return(y ^ (y >> 18));
		}

		private void SeedMT(ulong _seed)
		{
			ulong x = (_seed | 1U) & 0xFFFFFFFFU;
			int j = N;

			for(j = N; j >=0; j--)
			{
				state[j] = (x*=69069U) & 0xFFFFFFFFU;
			}
			next = 0;
		}

		public int RandomRange(int lo, int hi)
		{		
			return (Math.Abs((int)RandomInt() % (hi - lo + 1)) + lo);
		}

		public int RollDice(int face, int number_of_dice)
		{
			int roll = 0;
			for(int loop=0; loop < number_of_dice; loop++)
			{
				roll += (RandomRange(1,face));
			}
			return roll;
		}

		public int HeadsOrTails()		{ return((int)(RandomInt()) % 2); }

		public int D6(int die_count)	{ return RollDice(6,die_count); }
		public int D8(int die_count)	{ return RollDice(8,die_count); }
		public int D10(int die_count)	{ return RollDice(10,die_count); }
		public int D12(int die_count)	{ return RollDice(12,die_count); }
		public int D20(int die_count)	{ return RollDice(20,die_count); }
		public int D25(int die_count)	{ return RollDice(25,die_count); }


		private ulong ReloadMT()
		{
			ulong [] p0 = state;
			int p0pos = 0;
			ulong [] p2 = state;
			int p2pos = 2;
			ulong [] pM = state;
			int pMpos = M;
			ulong s0;
			ulong s1;

			int    j;

			if((next + 1) > N)
				SeedMT(seedValue);

			for(s0=state[0], s1=state[1], j=N-M+1; --j > 0; s0=s1, s1=p2[p2pos++])
				p0[p0pos++] = pM[pMpos++] ^ (mixBits(s0, s1) >> 1) ^ (loBit(s1) != 0 ? K : 0U);


			for(pM[0]=state[0],pMpos=0, j=M; --j > 0; s0=s1, s1=p2[p2pos++])
				p0[p0pos++] = pM[pMpos++] ^ (mixBits(s0,s1) >> 1) ^ (loBit(s1) != 0 ? K : 0U);
			

			s1=state[0];
			p0[p0pos] = pM[pMpos] ^ (mixBits(s0, s1) >> 1) ^ (loBit(s1) != 0 ? K : 0U);
			s1 ^= (s1 >> 11);
			s1 ^= (s1 <<  7) & 0x9D2C5680U;
			s1 ^= (s1 << 15) & 0xEFC60000U;
			return(s1 ^ (s1 >> 18));
		}

		private ulong hiBit(ulong _u)
		{
			return((_u) & 0x80000000U);
		}
		private ulong loBit(ulong _u)
		{
			return((_u) & 0x00000001U);
		}
		private ulong loBits(ulong _u)
		{
			return((_u) & 0x7FFFFFFFU);
		}
		private ulong mixBits(ulong _u, ulong _v)
		{
			return(hiBit(_u)|loBits(_v));
		}
	}
}
