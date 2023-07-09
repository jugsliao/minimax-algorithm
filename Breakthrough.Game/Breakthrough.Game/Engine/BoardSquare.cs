namespace Breakthrough.Game.Engine
{
    internal struct BoardSquare
    {
        internal GamePiece CurrentPiece;
        internal BoardSquare(BoardSquare square)
        {
            CurrentPiece = new GamePiece(square.CurrentPiece);
        }

        public override string ToString()
        {
            return (CurrentPiece != null ? CurrentPiece.PieceColor.ToString() : "");
        }
    }
}