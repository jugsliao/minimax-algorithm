using System;
using System.Windows.Forms;
using Breakthrough.Client.Components;
using Breakthrough.Game.Engine;

namespace Breakthrough.Client.Forms
{
    public partial class MainForm : Form
    {
        private TimeSpan BlackTime;
        private DateTime TurnTick;
        private TimeSpan WhiteTime;
        public GamePieceColor WhosMove;

        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            tAI.SelectedIndex = 0;

            Informations.Board = this.chessBoard;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {           
            TurnTick = DateTime.Now;
            BlackTime = TimeSpan.Zero;
            WhiteTime = TimeSpan.Zero;
            TurnTimer.Enabled = true;                            
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            chessBoard.Refresh();
        }

        private void chessBoard_SourceSelected(ChessBoard.Column selectedColumn, short selectedRow)
        {
            lblSelectedColumn.Text = "Source: " + selectedColumn;
            lblSelecteRow.Text = selectedRow.ToString();
            lblDestinationColumn.Text = "";
            lblDestinationRow.Text = "";
        }

        private void chessBoard_DestinationSelected(ChessBoard.Column selectedColumn, short selectedRow)
        {
            lblDestinationColumn.Text = "Destination: " + selectedColumn;
            lblDestinationRow.Text = selectedRow.ToString();
        }

        private void Board_TurnChanged(GamePieceColor whosMove)
        {
            if (whosMove == GamePieceColor.White)
            {
                lblTurn.Text = @"White's Turn";
                WhosMove = GamePieceColor.White;
            }
            if (whosMove == GamePieceColor.Black)
            {
                lblTurn.Text = @"Black's Turn";
                WhosMove = GamePieceColor.Black;   
            }

        }

        private void TurnTimer_Tick(object sender, EventArgs e)
        {
            if (WhosMove == GamePieceColor.White)
            {
                WhiteTime = WhiteTime.Add(DateTime.Now - TurnTick);
            }
            else if (WhosMove == GamePieceColor.Black)
            {
                BlackTime = BlackTime.Add(DateTime.Now - TurnTick);
            }

            
            lblWhiteTime.Text = "White: " + (WhiteTime.ToString()).Substring(0, 8);
            lblBlackTime.Text = "Black: " + (BlackTime.ToString()).Substring(0, 8);

            TurnTick = DateTime.Now;
        }      


        private void mnuNew_Click(object sender, EventArgs e)
        {
            chessBoard.NewGame();            
            TurnTick = DateTime.Now;
            BlackTime = TimeSpan.Zero;
            WhiteTime = TimeSpan.Zero;            
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chessBoard.Mode = (PlayMode)tAI.SelectedIndex;
        }

        private void tTime_TextChanged(object sender, EventArgs e)
        {
            bool Failed = false;
            try
            {
                if (string.IsNullOrEmpty(tTime.Text)) Failed = true;
                int Time = 5000;
                if (int.TryParse(tTime.Text, out Time))
                {
                    chessBoard.engine.SetTimeToMoveMs(Time);
                }
            }
            catch {Failed = true;}

            if (Failed)
            {
                tTime.Text = "5000";
            }
        }

        private void cTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = cTop.Checked;
        }

    }
}
