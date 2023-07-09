using System;
using System.Collections.Generic;
using System.Text;

namespace Breakthrough.Game.Engine
{
    /// <summary>
    /// Transposition table entry type, stored as a byte
    /// </summary>
    internal enum BoardTTEntryType : byte
    {
        ExactValue,
        Lowerbound,
        Upperbound
    }

    /// <summary>
    /// A structure for the transposition table entry
    /// </summary>
	internal sealed class BoardTTEntry
	{
        internal BoardTTEntry(int BoardValue, BoardTTEntryType EntryType, byte SearchDepth)
	    {
            Value = BoardValue;
            Type = EntryType;
            Depth = SearchDepth;
	    }

        /// <summary>
        /// The value of the board
        /// </summary>
        internal int Value;
        /// <summary>
        /// Cut type
        /// </summary>
        internal BoardTTEntryType Type;
        /// <summary>
        /// Depth of the search for value precision
        /// </summary>
        internal byte Depth;
	}
}
