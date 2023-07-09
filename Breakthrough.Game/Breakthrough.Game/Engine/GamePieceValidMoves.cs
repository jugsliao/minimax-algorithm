using System.Collections.Generic;

namespace Breakthrough.Game.Engine
{
    internal static class GamePieceValidMoves
    {
        private static void ResetBoard(Board board)
        {
            for (byte i = 0; i < 64; i++)
            {
                BoardSquare sqr = board.BoardSquares[i];

                if (sqr.CurrentPiece == null)
                    continue;

                if (sqr.CurrentPiece.AttackedValue != 0){
                    sqr.CurrentPiece.AttackedValue = 0;
                }

                sqr.CurrentPiece.ValidMoves = new Stack<BoardPosition>(sqr.CurrentPiece.LastValidMoveCount);
                sqr.CurrentPiece.ProtectedValue = 0;
                sqr.CurrentPiece.ConnectedV = false;
                sqr.CurrentPiece.ConnectedH = false;
            }
        }

        private static void AnalyzeMove(BoardPosition[] Moves, BoardSquare Square, byte Column, byte Row, Board Board, byte MovesCount)
        {
            // field shortcuts
            BoardPosition Move;
            int MovePosition;

            //Analyze horizontal connections
            if (!Square.CurrentPiece.ConnectedH)
            {
                if (Row > 1)
                {
                    var LeftPiece = Board.BoardSquares[GetPosition((int)Column, Row - 1)].CurrentPiece;
                    if (LeftPiece != null && LeftPiece.PieceColor == Square.CurrentPiece.PieceColor)
                    {
                        LeftPiece.ConnectedH = true;
                        Square.CurrentPiece.ConnectedH = true;
                    }
                }
                if (Row < 7)
                {
                    var RightPiece = Board.BoardSquares[GetPosition((int)Column, Row + 1)].CurrentPiece;
                    if (RightPiece != null && RightPiece.PieceColor == Square.CurrentPiece.PieceColor)
                    {
                        RightPiece.ConnectedH = true;
                        Square.CurrentPiece.ConnectedH = true;
                    }
                }
            }

            for (byte i = 0; i < MovesCount; i++)           
            {                
				// update shortcuts
                Move = Moves[i];
                MovePosition = GetPosition(Move.BoardColumn, Move.BoardRow);

                if (Move.BoardColumn != Column)
                {
                    //If there is a piece there I can potentialy kill
                    GamePiece PieceMoving = Square.CurrentPiece;
                    BoardSquare DestinationSquare = Board.BoardSquares[MovePosition];

                    //If there is a piece there I can potentialy kill
                    if (DestinationSquare.CurrentPiece != null)
                    {
                        GamePiece pieceAttacked = DestinationSquare.CurrentPiece;

                        //if that piece is a different color
                        if (pieceAttacked.PieceColor != PieceMoving.PieceColor)
                        {
                            //Add this as a valid move
                            PieceMoving.ValidMoves.Push(new BoardPosition(Move.BoardColumn, Move.BoardRow));
                            //Add to this pieces Attacked Value
                            pieceAttacked.AttackedValue += BoardEvaluation.PieceAttackValue;
                        }
                        else
                        {
                            //Else I am protecting this piece so make this piece protected count +1
                            pieceAttacked.ProtectedValue += BoardEvaluation.PieceProtectionValue;

                        }
                        
                    }
                    else
                    {
                        PieceMoving.ValidMoves.Push(new BoardPosition(Move.BoardColumn, Move.BoardRow));
                    }
                }
                // if there is something if front pawns can't move there
                else if (Board.BoardSquares[MovePosition].CurrentPiece != null)
                {
                    var FrontPiece = Board.BoardSquares[MovePosition].CurrentPiece;
                    if (FrontPiece.PieceColor == Square.CurrentPiece.PieceColor)
                    { // if it's same color, the pieces are connected verticaly
                        FrontPiece.ConnectedV = true;
                        Square.CurrentPiece.ConnectedV = true;
                    }
                    return;
                }
                else //if there is nothing in front of me 
                {
                    Square.CurrentPiece.ValidMoves.Push(new BoardPosition(Move.BoardColumn, Move.BoardRow));
                }

            }
        }


        internal static void GenerateValidMoves(Board board)
        {
            ResetBoard(board);

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

            //Generate Moves
            for (byte x = 0; x < 8; x++)
            {
                for (byte y = 0; y < 8; y++)
                {
                    int Position = GetPosition(x,y);
                    BoardSquare sqr = board.BoardSquares[Position];

                    if (sqr.CurrentPiece == null)
                        continue;

                    if (sqr.CurrentPiece.PieceColor == GamePieceColor.White)
                    {
                        AnalyzeMove(MoveArrays.WhitePawnMoves[Position].Moves, sqr, x, y, board,
                            MoveArrays.WhitePawnTotalMoves[Position]);
                    }
                    else //if (sqr.CurrentPiece.PieceColor == GamePieceColor.Black)
                    {
                        AnalyzeMove(MoveArrays.BlackPawnMoves[Position].Moves, sqr, x, y, board,
                            MoveArrays.BlackPawnTotalMoves[Position]);
                    }
                }
            }
			
        }

        private static byte GetRow(byte position) { return (byte)(7 - (position >> 3)); }
        private static byte GetColumn(byte position) { return (byte)(position % 8); }
        private static int GetPosition(int column, int row) { return ((7 - row) << 3) + column; }
    }
}