using System;
using System.Collections.Generic;
using System.Text;

namespace Breakthrough.Game.Engine
{
    /// <summary>
    /// A transposition table storage class
    /// 
    /// A transposition table is a repository of past search results, usually implemented as a hash dictionary 
    /// or similar structure to achieve maximum speed.  When a position has been searched, the results 
    /// (i.e., evaluation, depth of the search performed from this position, best move, etc.) are stored in the table. 
    /// Then, when new positions have to be searched, we query the table first: if suitable results already 
    /// exist for a specific position, we use them and bypass the search entirely.
    /// </summary>
	internal static class BoardTT
    {
        /// <summary>
        /// Contains all random numbers
        /// 0-63 White Pawn position
        /// 64-128 Black Pawn position
        /// 129 Indicator of current side
        /// </summary>
        internal static ulong[] PRNArray;
        internal static ulong Seed = 1; // Seed for reproducibility
        internal static Dictionary<ulong, BoardTTEntry> Table;

        #region Static Constructor

        /// <summary>
        /// At program initialization, we generate an array of pseudorandom numbers:
        ///  - One number for each piece at each square
        ///  - One number to indicate the side to move is black
        /// </summary>
        static BoardTT()
        {
            // total should be: 2*64 + 1 = 129
            PRNArray = new ulong[129];
            RandomMT MT = new RandomMT(Seed);
            for (int i = 0; i < PRNArray.Length; i++)
                PRNArray[i] = MT.RandomInt();

            //Initialize the table
            Table = new Dictionary<ulong, BoardTTEntry>();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Stores the entry in a data structure
        /// </summary>
        internal static void StoreEntry(ulong Key, BoardTTEntry Entry)
        {
            if (!Table.ContainsKey(Key))
            {
                Table[Key] = Entry;
            }
        }
        #endregion
    }
}
