using System;
using System.Collections.Generic;

namespace Breakthrough.Game.Engine
{
	internal static class BoardEvaluation
	{
        internal const int WinValue = 500000;
        internal const short PieceAlmostWinValue = 10000;
        internal const short PieceValue = 1300;
        internal const short PieceDangerValue = 10;
        internal const short PieceHighDangerValue = 100;
        internal const short PieceAttackValue = 50;
        internal const short PieceProtectionValue = 65;
        internal const short PieceConnectionHValue = 35;
        internal const short PieceConnectionVValue = 15;
        internal const short PieceColumnHoleValue = 20;
        internal const short PieceHomeGroundValue = 10;
		
        internal static void GetValue(Board board, GamePieceColor ColorMoving)
        {
	        board.Value = 0;
        	
	        // evaluate remaining pieces and the state of the game
            int RemainingWhitePieces = 0;
            int RemainingBlackPieces = 0;
        	
	        // scan all squares
            for (byte column = 0; column < 8; column++)
            {
                int BlackPiecesOnColumn = 0;
                int WhitePiecesOnColumn = 0;

                for (byte row = 0; row < 8; row++)
                {

                    int Position = GetPosition(column, row);
                    BoardSquare square = board.BoardSquares[Position];

                    if (square.CurrentPiece == null)
                        continue;

                    if (square.CurrentPiece.PieceColor == GamePieceColor.White)
                    {
                        RemainingWhitePieces++;
                        WhitePiecesOnColumn++;
                        board.Value += GetPieceValue(square, column, row);
                        if (row == 7)
                        { // winning position
                            board.WhiteWins = true;
                        }
                        else if (row == 6)
                        { // check almost win
                            bool ThreatA = false;
                            bool ThreatB = false;
                            if (column > 0) ThreatA = (board.BoardSquares[GetPosition(column - 1, 7)].CurrentPiece == null);
                            if (column < 7) ThreatB = (board.BoardSquares[GetPosition(column + 1, 7)].CurrentPiece == null);
                            if (!(ThreatA && ThreatB)) // almost win
                                board.Value += PieceAlmostWinValue;
                        }
                        else if (row == 0)
                        { // home ground
                            board.Value += PieceHomeGroundValue;
                        }
                    }
                    else
                    {
                        RemainingBlackPieces++;
                        BlackPiecesOnColumn++;
                        board.Value -= GetPieceValue(square, column, row);
                        if (row == 0)
                        { // winning position
                            board.BlackWins = true;
                        }
                        else if (row == 1)
                        { // check almost win
                            bool ThreatA = false;
                            bool ThreatB = false;
                            if (column > 0) ThreatA = (board.BoardSquares[GetPosition(column - 1, 0)].CurrentPiece == null);
                            if (column < 7) ThreatB = (board.BoardSquares[GetPosition(column + 1, 0)].CurrentPiece == null);
                            if(!(ThreatA && ThreatB)) // almost win
                                board.Value -= PieceAlmostWinValue;
                        }
                        else if (row == 7)
                        { // home ground
                            board.Value -= PieceHomeGroundValue;
                        }
                    }
                }

                // Row hole feature
                if (WhitePiecesOnColumn == 0) board.Value -= PieceColumnHoleValue;
                if (BlackPiecesOnColumn == 0) board.Value += PieceColumnHoleValue;

	        }

            // if no more material available
            if (RemainingWhitePieces == 0) board.BlackWins = true; 
            if (RemainingBlackPieces == 0) board.WhiteWins = true;

            // winning positions
            if (board.WhiteWins) board.Value += WinValue;
            if (board.BlackWins) board.Value -= WinValue;

            // invert Value for Negamax
            if (GetOppositeColor(ColorMoving) == GamePieceColor.Black)
                board.Value = -board.Value;
        }


		/// <summary>
		/// Evaluates current piece value
		/// </summary>
		private static int GetPieceValue(BoardSquare square, byte Column, byte Row)
		{
            int Value = PieceValue;
			var Piece = square.CurrentPiece;
			
            // add connections value
            if (Piece.ConnectedH) Value += PieceConnectionHValue;
            if (Piece.ConnectedV) Value += PieceConnectionVValue;

			// add to the value the protected value
			Value += Piece.ProtectedValue;
			
			// evaluate attack
			if (Piece.AttackedValue > 0)
            {
                Value -= Piece.AttackedValue;
				if (Piece.ProtectedValue == 0)
					Value -= Piece.AttackedValue;
			}else{
				if (Piece.ProtectedValue != 0)
				{ 
					// pawns at the end that are not attacked are worth more points
					if (Piece.PieceColor == GamePieceColor.White)
					{
                        if (Row == 5) Value += PieceDangerValue;
                        else if (Row == 6) Value += PieceHighDangerValue;
					}
					else
					{
                        if (Row == 2) Value += PieceDangerValue;
                        else if (Row == 1) Value += PieceHighDangerValue;
					}
				}
			}

            // danger value
            if (Piece.PieceColor == GamePieceColor.White)
                Value += Row * PieceDangerValue;
            else
                Value += (8-Row) * PieceDangerValue;

			
			// mobility feature
			Value += Piece.ValidMoves.Count;
			
			return Value;
		}


        #region Helper Methods
        private static GamePieceColor GetOppositeColor(GamePieceColor color){ if (color == GamePieceColor.Black) return GamePieceColor.White; else return GamePieceColor.Black; }
        private static byte ModifyDepth(byte depth, int PossibleMoves){if (PossibleMoves < 9) depth = (byte)(depth + 2); return depth;}
		private static int Sort(Board board1, Board board2){ return board1.Value - board2.Value; }
        private static byte GetRow(byte position) { return (byte)(7 - (int)(position / 8)); }
        private static byte GetColumn(byte position) { return (byte)(position % 8); }
        private static int GetPosition(int column, int row) { return (7 - row) * 8 + column; }

        #endregion


	}
}
