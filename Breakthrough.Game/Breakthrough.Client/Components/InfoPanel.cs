using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Breakthrough.Game.Engine;

namespace Breakthrough.Client.Components
{
    public partial class InfoPanel : UserControl
    {
        private ChessBoard board;
        public ChessBoard Board
        {
            get { return board; }
            set { 
                board = value;
                if(board!= null)
                    board.TurnChanged += new ChessBoard.TurnChangedHandler(Board_TurnChanged);
            }
        }

        void Board_TurnChanged(GamePieceColor whosMove)
        {
            var history = board.engine.MoveHistory;
            HistoryList.Items.Clear();
            var array = history.ToArray();
            for (int i = array.Length-1; i >= 0; i--)
                HistoryList.Items.Add(array[i]);
        }

        public InfoPanel()
        {
            InitializeComponent();
        }


        #region Helpers
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
        #endregion
    }
}
