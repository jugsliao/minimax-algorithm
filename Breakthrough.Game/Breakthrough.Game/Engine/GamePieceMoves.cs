using System.Collections.Generic;

namespace Breakthrough.Game.Engine
{
    internal struct GamePieceMoveSet
    {
        internal readonly BoardPosition[] Moves;

        internal GamePieceMoveSet(List<BoardPosition> moves)
        {
            Moves = moves.ToArray();
        }

    }

    internal struct MoveArrays
    {
        /*
            0  1  2  3  4  5  6  7
            8  9  10 11 12 13 14 15
            16 17 18 19 20 21 22 23
            24 25 26 27 28 29 30 31
            32 33 34 35 36 37 38 39
            40 41 42 43 44 45 46 47
            48 49 50 51 52 53 54 55
            56 57 58 59 60 61 62 63
         */
        internal static GamePieceMoveSet[] BlackPawnMoves;
        internal static byte[] BlackPawnTotalMoves;

        internal static GamePieceMoveSet[] WhitePawnMoves;
        internal static byte[] WhitePawnTotalMoves;
    }

    internal static class GamePieceMoves
    {

        internal static void InitiateGamePieceMotion()
        {
            MoveArrays.WhitePawnMoves = new GamePieceMoveSet[64];
            MoveArrays.WhitePawnTotalMoves = new byte[64];
            MoveArrays.BlackPawnMoves = new GamePieceMoveSet[64];
            MoveArrays.BlackPawnTotalMoves = new byte[64];

            SetMovesWhitePawn();
            SetMovesBlackPawn();

        }

        private static void SetMovesWhitePawn()
        {
            for (byte column = 0; column < 8; column++)
            {
                for (byte row = 0; row < 7; row++)
                {
                    var moves = new List<BoardPosition>();
                    var position = GetPosition(column, row);
                    BoardPosition move;
                    //Diagonal

                    if (column < 7)
                    {
                        move = new BoardPosition();
                        move.BoardColumn = (byte)(column + 1);
                        move.BoardRow = (byte)(row + 1);
                        moves.Add(move);
                        MoveArrays.WhitePawnTotalMoves[position]++;
                    }
                    if (column > 0)
                    {
                        move = new BoardPosition();
                        move.BoardColumn = (byte)(column - 1);
                        move.BoardRow = (byte)(row + 1);
                        moves.Add(move);
                        MoveArrays.WhitePawnTotalMoves[position]++;
                    }


                    move = new BoardPosition();

                    move.BoardColumn = column;
                    move.BoardRow = (byte)(row + 1);

                    moves.Add(move);
                    MoveArrays.WhitePawnTotalMoves[position]++;
                    MoveArrays.WhitePawnMoves[position] = new GamePieceMoveSet(moves);
                }
            }
        }

        private static void SetMovesBlackPawn()
        {
            for (byte column = 0; column < 8; column++)
            {
                for (byte row = 1; row < 8; row++)
                {
                    var moves = new List<BoardPosition>();
                    var position = GetPosition(column, row);
                    BoardPosition move;
                    //Diagonal
                    if (column < 7)
                    {
                        move = new BoardPosition();
                        move.BoardColumn = (byte)(column + 1);
                        move.BoardRow = (byte)(row - 1);

                        moves.Add(move);
                        MoveArrays.BlackPawnTotalMoves[position]++;
                    }
                    if (column > 0)
                    {
                        move = new BoardPosition();
                        move.BoardColumn = (byte)(column - 1);
                        move.BoardRow = (byte)(row - 1);
                        moves.Add(move);
                        MoveArrays.BlackPawnTotalMoves[position]++;
                    }

                    //One Forward
                    move = new BoardPosition();
                    move.BoardColumn = column;
                    move.BoardRow = (byte)(row - 1);

                    moves.Add(move);
                    MoveArrays.BlackPawnTotalMoves[position]++;

                    MoveArrays.BlackPawnMoves[position] = new GamePieceMoveSet(moves);
                }
            }
        }


        private static byte GetRow(byte position) { return (byte)(7 - (position >> 3)); }
        private static byte GetColumn(byte position) { return (byte)(position % 8); }
        private static int GetPosition(int column, int row) { return ((7 - row) << 3) + column; }
    }
}