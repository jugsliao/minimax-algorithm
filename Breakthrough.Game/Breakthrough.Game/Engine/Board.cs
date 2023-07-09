using System;

namespace Breakthrough.Game.Engine
{
    internal sealed class Board
    {
        internal BoardSquare[] BoardSquares;
        internal int Value;
        internal bool BlackWins;
        internal bool WhiteWins;
        internal MoveContent LastMove;
        internal int MoveCount;
        internal GamePieceColor WhosMove;
        internal ulong HashValue;
        internal int Pieces = 0;

        #region Constructors

        internal Board()
        {
            BoardSquares = new BoardSquare[64];
            LastMove = new MoveContent();
        }

        private Board(BoardSquare[] squares)
        {
            BoardSquares = new BoardSquare[64];
            for (byte i = 0; i < 64; i++)
            {
                if (squares[i].CurrentPiece != null)
                {
                    BoardSquares[i] = new BoardSquare(squares[i]);
                    Pieces++;
                }
                else
                {
                    BoardSquares[i] = new BoardSquare();
                }
            }


            LastMove = new MoveContent();
        }

        internal Board(int val): this()
        {
            Value = val;
        }

        internal Board(Board board)
        {
            BoardSquares = new BoardSquare[64];

            for (byte i = 0; i < 64; i++)
            {
                if (board.BoardSquares[i].CurrentPiece != null)
                {
                    BoardSquares[i] = new BoardSquare(board.BoardSquares[i]);
                    Pieces++;
                }
                else
                {
                    BoardSquares[i] = new BoardSquare();
                }
            }

            LastMove = new MoveContent();

            WhiteWins = board.WhiteWins;
            BlackWins = board.BlackWins;
            WhosMove = board.WhosMove;

            Value = board.Value;

            LastMove = new MoveContent(board.LastMove);

            MoveCount = board.MoveCount;
        }

        #endregion

        #region Internal Methods
        
        /// <summary>
        /// Performs a fast copy of the board
        /// </summary>
        internal Board FastCopy()
        {
            var ClonedBoard = new Board(BoardSquares);

            ClonedBoard.WhosMove = WhosMove;
            ClonedBoard.MoveCount = MoveCount;
            ClonedBoard.HashValue = HashValue;

            return ClonedBoard;
        }

    
        /// <summary>
        /// Moves a piece on the board
        /// </summary>
        internal static bool MovePiece(Board board, byte SourceColumn, byte SourceRow, byte DestinationColumn, byte DestinationRow)
        {
            int SourcePosition = GetPosition(SourceColumn, SourceRow);
            int DestinationPosition = GetPosition(DestinationColumn, DestinationRow);
            GamePiece Piece = board.BoardSquares[SourcePosition].CurrentPiece;

            //Record my last move
            board.LastMove = new MoveContent(SourceColumn, SourceRow, DestinationColumn, DestinationRow, Piece.PieceColor);

            if (Piece.PieceColor == GamePieceColor.Black)
            {
                board.MoveCount++;
            }

            if (board.BoardSquares[DestinationPosition].CurrentPiece != null)
            {
                board.LastMove.PieceTakenColor = board.BoardSquares[DestinationPosition].CurrentPiece.PieceColor;
                board.HashValue ^= (BoardTT.PRNArray[(int)board.LastMove.PieceTakenColor * 64 + DestinationPosition]);
                board.Pieces--;
            }

            //Delete the piece in its source position
            board.BoardSquares[SourcePosition].CurrentPiece = null;

            //Add the piece to its new position
            board.BoardSquares[DestinationPosition].CurrentPiece = Piece;
            board.BoardSquares[DestinationPosition].CurrentPiece.Moved = true;
            board.BoardSquares[DestinationPosition].CurrentPiece.Selected = false;

            //Update hash
            board.HashValue ^= (BoardTT.PRNArray[(int)Piece.PieceColor * 64 + SourcePosition]);
            board.HashValue ^= (BoardTT.PRNArray[(int)Piece.PieceColor * 64 + DestinationPosition]);
            board.HashValue ^= BoardTT.PRNArray[128];

            if (board.WhosMove == GamePieceColor.White)
            {
                board.WhosMove = GamePieceColor.Black;
            }
            else
            {
                board.WhosMove = GamePieceColor.White;
            }

           return true;
        }

        /// <summary>
        /// Makes a null move
        /// </summary>
        internal static void MakeNullMove(Board board)
        {
            board.Value = -board.Value;
            board.HashValue ^= BoardTT.PRNArray[128];
        }

        /// <summary>
        /// Unmakes a null move
        /// </summary>
        internal static void UnmakeNullMove(Board board)
        {
            board.HashValue ^= BoardTT.PRNArray[128];
            board.Value = -board.Value;
        }

        /// <summary>
        /// Creates a LoS-reduced version of a board
        /// </summary>
        internal static Board ReduceBoard(Board board, int column, int row)
        {
            Board Result = new Board();
            int Position = GetPosition(column, row);
            GamePieceColor color = board.BoardSquares[Position].CurrentPiece.PieceColor;

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
            if (color == GamePieceColor.Black)
            {
                int expansion = 0;
                for (int i = row; i >= 0; i--)
                { // down LoS

                    int LeftCol = column - expansion;
                    if (LeftCol < 0) LeftCol = 0;
                    int RightCol = column + expansion;
                    if (RightCol > 7) RightCol = 7;

                    int LeftBoundary = GetPosition(LeftCol, i);
                    int RightBoundary = GetPosition(RightCol, i);

                    //copy loop
                    for (int j = LeftBoundary; j <= RightBoundary;j++ )
                        Result.BoardSquares[j] = board.BoardSquares[j];

                    expansion++;
                    
                }
            }else if (color == GamePieceColor.White)
            {
                int expansion = 0;
                for (int i = row; i < 8; i++)
                { // up LoS
                    
                    int LeftCol = column - expansion;
                    if (LeftCol < 0) LeftCol = 0;
                    int RightCol = column + expansion;
                    if (RightCol > 7) RightCol = 7;

                    int LeftBoundary = GetPosition(LeftCol, i);
                    int RightBoundary = GetPosition(RightCol, i);

                    //copy loop
                    for (int j = LeftBoundary; j <= RightBoundary;j++ )
                        Result.BoardSquares[j] = board.BoardSquares[j];

                    expansion++;
                }
            }


            return Result;
        }

        /// <summary>
        /// Method to get a hash value of a board
        /// Remarks: This method needs serious optimization at some point
        /// </summary>
        internal ulong GetHashValue()
        {
            GamePiece CurrentPiece;
            HashValue = 0;
            for (byte i = 0; i < 64; i++)
            {
                CurrentPiece = BoardSquares[i].CurrentPiece;
                if (CurrentPiece != null)
                {
                    HashValue ^= (BoardTT.PRNArray[(int)CurrentPiece.PieceColor * 64 + i]); //Append the hash xor
                }
            }
            if(WhosMove == GamePieceColor.Black)
            {
                HashValue ^= BoardTT.PRNArray[128];
            }
            return HashValue;
        }


        internal static string GetColumnFromByte(byte column)
        {
            switch (column)
            {
                case 0:
                    return "a";
                case 1:
                    return "b";
                case 2:
                    return "c";
                case 3:
                    return "d";
                case 4:
                    return "e";
                case 5:
                    return "f";
                case 6:
                    return "g";
                case 7:
                    return "h";
                default:
                    return "a";
            }
        }

        private static byte GetRow(byte position) { return (byte)(7 - (position >> 3)); }
        private static byte GetColumn(byte position) { return (byte)(position % 8); }
        private static int GetPosition(int column, int row) { return ((7 - row) << 3) + column; }


        internal bool IsEnded()
        {
            return BlackWins || WhiteWins;
        }

        public override string ToString()
        {
            string Winner = "";
            if (BlackWins) Winner = ", Black wins";
            if (WhiteWins) Winner = ", White wins";
            return String.Format("Board, Value = {0}, {1} will move, Last move = {2}{3}", Value, WhosMove, LastMove, Winner);
        }
        #endregion
    }
}