using System.Collections.Generic;

namespace Breakthrough.Game.Engine
{

    public enum GamePieceColor
    {
        White,
        Black
    }

    internal sealed class GamePiece
    {
        #region InternalMembers

        internal GamePieceColor PieceColor;

        internal short AttackedValue;
        internal bool ConnectedH;
        internal bool ConnectedV;
        internal int LastValidMoveCount;
        internal bool Moved;
        internal short ProtectedValue;
        internal bool Selected;
        internal Stack<BoardPosition> ValidMoves;

        #endregion

        #region Constructors

        internal GamePiece(GamePiece piece)
        {
            PieceColor = piece.PieceColor;
            Moved = piece.Moved;

            if (piece.ValidMoves != null)
                LastValidMoveCount = piece.ValidMoves.Count;
        }

        internal GamePiece(GamePieceColor GamePieceColor)
        {
            PieceColor = GamePieceColor;
            LastValidMoveCount = 5;

        }

        #endregion

        #region PublicMembers

        internal string GamePieceTypeShort
        {
            get
            {
                return "P";
            }
        }

        #endregion


    }
}