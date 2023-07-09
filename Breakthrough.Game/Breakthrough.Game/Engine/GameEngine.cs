using System;
using System.Collections.Generic;
using Breakthrough.Game;

[assembly : CLSCompliant(false)]

namespace Breakthrough.Game.Engine
{
    public sealed class GameEngine
    {        

        #region Members

        public GamePieceColor HumanPlayer;
        public bool DoRender = true;
		public int NodesSearched;
        public SimpleStopwatch Watch = new SimpleStopwatch();
        public Stack<MoveContent> MoveHistory = new Stack<MoveContent>();

        public GamePieceColor WhosMove
        {
            get { return ChessBoard.WhosMove; }
            set { ChessBoard.WhosMove = value; }
        }
        

        internal Board ChessBoard;
        internal Board PreviousChessBoard;
     
        #endregion

        #region Constructors

        public GameEngine()
        {
            ChessBoard = new Board();
           
            for (byte i = 0; i < 64; i++)
            {
                BoardSquare square = new BoardSquare();
                square.CurrentPiece = null;
                ChessBoard.BoardSquares[i] = square;
            }

            GamePieceMoves.InitiateGamePieceMotion();

            ChessBoard.WhosMove = GamePieceColor.White;

            // set a layout
            LayoutNormal();
            //LayoutTest4();
            //LayoutTest2();


			//Generate valid moves
            GamePieceValidMoves.GenerateValidMoves(ChessBoard);
        }

        #endregion   

        #region Board Layouts 
        private void LayoutNormal()
        {
            GamePiece piece;
            BoardPosition position;

            //White Pieces
            for (byte i = 0; i < 8; i++)
            {
                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 1);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 0);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }


            //Black Pieces
            for (byte i = 0; i < 8; i++)
            {
                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 6);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 7);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }
        }

        private void LayoutTest1()
        {
            GamePiece piece;
            BoardPosition position;

            //White Pieces
            for (byte i = 3; i < 5; i++)
            {
                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 3);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

            }

            //Black Pieces
            for (byte i = 3; i < 5; i++)
            {
                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 4);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

        }

        private void LayoutTest2()
        {
            GamePiece piece;
            BoardPosition position;

            //White Pieces
            for (byte i = 3; i < 5; i++)
            {
                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 3);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 2);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

            //Black Pieces
            for (byte i = 3; i < 5; i++)
            {
                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 4);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

        }

        private void LayoutTest3()
        {
            GamePiece piece;
            BoardPosition position;

            //White Pieces
            for (byte i = 3; i < 5; i++)
            {
                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 3);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 2);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

            //Black Pieces
            for (byte i = 3; i < 5; i++)
            {

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 6);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 4);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

        }

        private void LayoutTest4()
        {
            GamePiece piece;
            BoardPosition position;

            //White Pieces
            for (byte i = 0; i < 8; i++)
            {
                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 1);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.White);
                position = new BoardPosition(i, 0);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }


            //Black Pieces
            for (byte i = 0; i < 8; i++)
            {
                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 2);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 3);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 4);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 5);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 6);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);

                piece = new GamePiece(GamePieceColor.Black);
                position = new BoardPosition(i, 7);
                RegisterPiece(position.BoardColumn, position.BoardRow, piece);
            }

        }
        #endregion

        #region Public

        /// <summary>
		/// Performs an AI movement
		/// </summary>
		public void DoMove()
		{
			//only if not winning board
            if (ChessBoard != null && !ChessBoard.IsEnded())
            {
                Watch.Start();
                List<Board> Tree = new List<Board>();

                NodesSearched = 0;
                Board BestBoard = new Board();
                BestBoard = MoveSearchTree.IterativeDeepeningAlphaBeta(ChessBoard, MoveSearchTree.PlyDepth, WhosMove, DoRender);
                //BestBoard = MoveSearchTree.FixedDepthAlphaBeta(ChessBoard, 8, WhosMove, DoRender);
                //BestBoard = MoveSearchTree.AlphaBetaRoot(ChessBoard, 7, WhosMove);

                if (BestBoard != null && BestBoard.LastMove != null)
                {
                    var LastMove = BestBoard.LastMove;
                    MovePiece(LastMove.SourceColumn, LastMove.SourceRow, LastMove.DestinationColumn, LastMove.DestinationRow);
                    GamePieceValidMoves.GenerateValidMoves(ChessBoard);

                    ChessBoard.Value = BestBoard.Value;
                    ChessBoard.BlackWins = BestBoard.BlackWins;
                    ChessBoard.WhiteWins = BestBoard.WhiteWins;
                }
                else
                {
                    Console.WriteLine("Bug?");
                }

                NodesSearched = MoveSearchTree.NodesSearchedLast;
                Watch.Stop();
            }
			
		}

        public void ReduceTest()
        {
            ChessBoard = Board.ReduceBoard(ChessBoard, 4, 1);
        }

        public bool MovePiece(byte SourceColumn, byte SourceRow, byte DestinationColumn, byte DestinationRow)
        {
            MoveHistory.Push(new MoveContent(SourceColumn,SourceRow,DestinationColumn,DestinationRow,WhosMove));
            PreviousChessBoard = new Board(ChessBoard);

            if (Board.MovePiece(
                ChessBoard, SourceColumn, SourceRow, DestinationColumn,
                DestinationRow))
            {
                GamePieceValidMoves.GenerateValidMoves(ChessBoard);
                BoardEvaluation.GetValue(ChessBoard, WhosMove);
                return true;
            }

            return false;
        }

        public bool IsValidMove(byte SourceColumn, byte SourceRow, byte DestinationColumn, byte DestinationRow)
        {
            if (ChessBoard == null)
            {
                return false;
            }

            if (ChessBoard.BoardSquares == null)
            {
                return false;
            }

            int SourcePosition = GetPosition(SourceColumn, SourceRow);
            foreach (BoardPosition bs in ChessBoard.BoardSquares[SourcePosition].CurrentPiece.ValidMoves)
            {
                if (bs.BoardColumn == DestinationColumn)
                {
                    if (bs.BoardRow == DestinationRow)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsPawn(byte BoardColumn, byte BoardRow)
        {
            return ChessBoard.BoardSquares[GetPosition(BoardColumn,BoardRow)].CurrentPiece != null;
        }

        public GamePieceColor ReturnPieceColorAt(byte BoardColumn,byte BoardRow)
        {
            if (ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece == null)
            {
                return GamePieceColor.White;
            }
            return
                ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece.
                    PieceColor;
        }

        public bool ReturnGamePieceSelected(byte BoardColumn, byte BoardRow)
        {
            if (ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece == null)
            {
                return false;
            }

            return
                ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece.Selected;
        }

        public byte[][] ReturnValidMoves(byte BoardColumn, byte BoardRow)
        {
            if (ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece == null)
            {
                return null;
            }


            var returnArray = new byte[ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece.ValidMoves.Count][];
            int counter = 0;

            foreach (
                BoardPosition square in
                    ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece.
                        ValidMoves)
            {
                returnArray[counter] = new byte[2];
                returnArray[counter][0] = square.BoardColumn;
                returnArray[counter][1] = square.BoardRow;
                counter++;
            }

            return returnArray;
        }
		
		public int BoardValue{
			get{ return ChessBoard.Value;}
		}

        public bool WhiteWins{
            get { return ChessBoard.WhiteWins; }
        }

        public bool BlackWins{
            get { return ChessBoard.BlackWins; }
        }

        public void SetGamePieceSelection(byte BoardColumn, byte BoardRow,bool Selection)
        {
            if (ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece == null)
            {
                return;
            }
            ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece.Selected = Selection;
        }

        public void SetTimeToMoveMs(int Time)
        {
            MoveSearchTree.TimeToMoveMs = Time;
        }

        #endregion

        #region Private

        private void RegisterPiece(byte BoardColumn, byte BoardRow, GamePiece Piece)
        {
            ChessBoard.BoardSquares[GetPosition(BoardColumn, BoardRow)].CurrentPiece = Piece;

            return;
        }

        private static byte GetRow(byte position) { return (byte)(7 - (position >> 3)); }
        private static byte GetColumn(byte position) { return (byte)(position % 8); }
        private static int GetPosition(int column, int row) { return ((7 - row) << 3) + column; }
        #endregion   

    }
}
