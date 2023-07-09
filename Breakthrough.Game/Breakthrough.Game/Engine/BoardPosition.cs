namespace Breakthrough.Game.Engine
{
    public sealed class BoardPosition
    {
        public byte BoardColumn;
        public byte BoardRow;

        #region Constructors

        public BoardPosition()
        {
        }

        public BoardPosition(BoardPosition position)
        {
            BoardColumn = position.BoardColumn;
            BoardRow = position.BoardRow;
        }

        public BoardPosition(byte boardColumn, byte boardRow)
        {
            BoardColumn = boardColumn;
            BoardRow = boardRow;
        }

        #endregion
    }
}