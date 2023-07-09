using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using Breakthrough.Client.Properties;
using Breakthrough.Client.Render;
using Breakthrough.Game.Engine;

namespace Breakthrough.Client.Components
{
    public partial class ChessBoard : UserControl
    {
        internal class Selection
        {
            public byte Column;
            public byte Row;
            public bool Selected;
        }
		
		internal class WorkerArgs{
			public Selection Destination;
			public Selection Source;
			public GameEngine Engine;
			public bool MoveRequired;
		}

        public enum Column
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            Unknown
        }

        #region StaticMembers
        public static void InitializeManual()
        {
            Program.IsRunning = true;
        }
        #endregion

        #region Delegates

        public delegate void DestinationSelectHandler(Column destinationColumn, short destinationRow);
        public delegate void SourceSelectHandler(Column selectedColumn, short selectedRow);
        public delegate void TurnChangedHandler(GamePieceColor whosMove);

        #endregion

        #region PrivateMembers

        private Selection CurrentDestination;
        private Selection CurrentSource;
		private BackgroundWorker Worker;
        internal Breakthrough.Game.Engine.GameEngine engine;
        private int boxHeight;
        private int maxHeight;
        
        #endregion

        #region PublicMembers

        public PlayMode Mode;
        public event SourceSelectHandler SourceSelected;
        public event DestinationSelectHandler DestinationSelected;
        public event TurnChangedHandler TurnChanged;
        private bool DoRender = true;
        #endregion

        #region Constructors

        public ChessBoard()
        {
            InitializeComponent();
            if (Program.IsRunning)
            {
                Worker = new BackgroundWorker();
                Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
                Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
                NewGame();
            }
        }

        public ChessBoard(bool doRender, PlayMode mode)
        {
            InitializeComponent();
            this.DoRender = doRender;
            this.Mode = mode;
            
            Worker = new BackgroundWorker();
            Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }


        public void NewGame()
        {
            //ChessEngine 
            engine = new Breakthrough.Game.Engine.GameEngine();
      
            CurrentSource = new Selection();
            CurrentDestination = new Selection();
            Refresh();

            engine.DoRender = DoRender;

            if(Mode == PlayMode.WhiteCPU || Mode == PlayMode.CPUvsCPU)
                EngineMove(false);


        }

        #endregion

        #region PublicMethods


        private void EngineMove(bool MoveRequired)
        {
            if (MoveRequired && !CheckWin())
            {
                
                if (!engine.IsPawn(CurrentSource.Column, CurrentSource.Row) )
                {
                    CurrentDestination.Selected = false;
                    CurrentSource.Selected = false;
                    return;
                }

                //Check if this is infact a valid move

                bool valid = engine.IsValidMove(CurrentSource.Column, CurrentSource.Row, CurrentDestination.Column, CurrentDestination.Row);

                if (valid == false)
                {
                    CurrentDestination.Selected = false;
                    CurrentSource.Selected = false;
                    return;
                }
            }

            Cursor = Cursors.WaitCursor;         

            //Clear Source for next Selection             
            CurrentSource.Selected = false;

			if(MoveRequired){
				engine.MovePiece(CurrentSource.Column, CurrentSource.Row, CurrentDestination.Column, CurrentDestination.Row);
			}
			
			// Do Move
			var args = new WorkerArgs();
			args.Engine = engine;
			args.MoveRequired = MoveRequired;
			if(MoveRequired){
				args.Source = this.CurrentSource;
				args.Destination = this.CurrentDestination;
			}

            // fire events
            if (!CheckWin())
            {
                if (TurnChanged != null)
                    TurnChanged(engine.WhosMove);

                if (MoveRequired)
                {
                    EngineMove(false);
                }

                if (!Worker.IsBusy)
                {
                    Worker.RunWorkerAsync(args);
                }
            }
            else
            {
                ShowWin();
            }

            if (DoRender)
            {
                Refresh();
                Cursor = Cursors.Default;
            }
        }


        public static Column GetColumnFromInt(int column)
        {
            Column RetColumnt;

            switch (column)
            {
                case 1:
                    RetColumnt = Column.A;
                    break;
                case 2:
                    RetColumnt = Column.B;
                    break;
                case 3:
                    RetColumnt = Column.C;
                    break;
                case 4:
                    RetColumnt = Column.D;
                    break;
                case 5:
                    RetColumnt = Column.E;
                    break;
                case 6:
                    RetColumnt = Column.F;
                    break;
                case 7:
                    RetColumnt = Column.G;
                    break;
                case 8:
                    RetColumnt = Column.H;
                    break;
                default:
                    RetColumnt = Column.Unknown;
                    break;
            }

            return RetColumnt;
        }

        #endregion

        #region Events
		
		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
            //try
            //{
                WorkerArgs args = e.Argument as WorkerArgs;
                if (!args.MoveRequired)
                {
                    if (!CheckWin())
                    {
                        if (DoRender)
                            Console.WriteLine("AI> Working...");
                        engine.DoMove();
                        if (DoRender)
                            Console.WriteLine("AI> {0} nodes searched; Board Value: {1}", engine.NodesSearched, engine.BoardValue);
                        ShowWin();
                    }
                }
            //}
            //catch (Exception ex) { Console.WriteLine(ex.Message); }
		}
		
		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
            if (TurnChanged != null)
                TurnChanged(engine.WhosMove);

			this.Refresh();

            if(Mode == PlayMode.CPUvsCPU)
                EngineMove(false);

			// TODO: check if game is over
			//EngineMove(false);

            
        }

        #region Rendering
        private void ChessBoard_Paint(object sender, PaintEventArgs e)
        {
            if (!Program.IsRunning)
                return;
            if (!DoRender)
                return;

            GraphicsBuffer graphicsBuffer = new GraphicsBuffer();
            graphicsBuffer.CreateGraphicsBuffer(e.Graphics, Width, Height);

            Graphics g = graphicsBuffer.Graphics;
           
            SolidBrush solidWhiteBrush = new SolidBrush(Color.White);
            SolidBrush solidBlackBrush = new SolidBrush(Color.Black);
         
            Pen penBlack = new Pen(Color.Black, 1);
           
            Pen penHightlight = new Pen(Color.Black, 2);
            Pen penDestination = new Pen(Color.Yellow, 2);
            Pen penLastMove = new Pen(Color.Red, 2);
            Pen penValidMove = new Pen(Color.Black, 2);

            const int buffer = 10;

            if (Width < Height)
            {
                maxHeight = Width - 5 - buffer;
                boxHeight = maxHeight/8;
            }
            else
            {
                maxHeight = Height - 5 - buffer;
                boxHeight = maxHeight/8;
            }

            g.Clear(BackColor);

            try
            {
                int selectedX;
                int selectedY;

                //Draw Chess Board
                for (byte y = 0; y < 8; y++)
                {
                    for (byte x = 0; x < 8; x++)
                    {
                        if ((x + y)%2 == 0)
                        {
                            g.FillRectangle(solidWhiteBrush, (x * boxHeight) + buffer, (y * boxHeight) , boxHeight, boxHeight);
                        }
                        else
                        {
                            Rectangle drawArea1 = new Rectangle((x * boxHeight) + buffer, (y * boxHeight), boxHeight, boxHeight);
                            LinearGradientBrush linearBrush = new LinearGradientBrush(
                                        drawArea1, Color.Gainsboro, Color.Silver, LinearGradientMode.ForwardDiagonal );
                            g.FillRectangle(linearBrush, (x * boxHeight) + buffer, (y * boxHeight), boxHeight, boxHeight);
                        }

                        g.DrawRectangle(penBlack, (x * boxHeight) + buffer, (y * boxHeight) , boxHeight, boxHeight);

                    }
                    
                }
                for (byte i = 0; i < 8; i++)
                {
                    g.DrawString((8 - i).ToString(), new Font("Verdana", 8), solidBlackBrush, 0, (i * boxHeight)+ buffer);
                    g.DrawString(GetColumnFromInt(i + 1).ToString(), new Font("Verdana", 8), solidBlackBrush, (i * boxHeight) + (boxHeight/2) + 2, maxHeight - 2);
                }

                //Draw movement
                if (engine.MoveHistory.Count > 0)
                {
                    var LastMove = engine.MoveHistory.Peek();
                    selectedX = ((LastMove.DestinationColumn) * boxHeight) + buffer;
                    selectedY = (8 - LastMove.DestinationRow - 1) * boxHeight;

                    var P1 = new Point((((LastMove.SourceColumn) * boxHeight) + buffer + (boxHeight / 2)),
                                       ((8 - LastMove.SourceRow - 1) * boxHeight) + (boxHeight / 2));
                    var P2 = new Point((((LastMove.DestinationColumn) * boxHeight) + buffer) + (boxHeight / 2),
                                       ((8 - LastMove.DestinationRow - 1) * boxHeight) + (boxHeight / 2));
                    g.DrawLine(penLastMove, P1, P2);

                    g.DrawRectangle(penLastMove, selectedX, selectedY, boxHeight - 1, boxHeight - 1);
                }


                //Draw Pieces
                for (byte column = 0; column < 8; column++)
                {
                    for (byte row = 0; row < 8; row++)
                    {
                        if (engine.IsPawn(column, row))
                        {
                            GamePieceColor GamePieceColor = engine.ReturnPieceColorAt(column, row);
                            bool selected = engine.ReturnGamePieceSelected(column, row);

                            int x = (column)*boxHeight;
                            int y = (8 - row - 1)*boxHeight;

                            if (GamePieceColor == GamePieceColor.White)
                            {
                                g.DrawImage(Resources.WPawn, x + buffer, y, boxHeight, boxHeight);
                            }
                            else
                            {
                                g.DrawImage(Resources.BPawn, x + buffer, y, boxHeight, boxHeight);
                            }

                            if (selected)
                            {
                                selectedX = ((column)*boxHeight) + buffer;
                                selectedY = (8 - row - 1)*boxHeight;

                                g.DrawRectangle(penHightlight, selectedX, selectedY, boxHeight - 1, boxHeight - 1);


                                //Draw Valid Moves
                                if (engine.ReturnValidMoves(column, row) != null)
                                {
                                    foreach (byte[] sqr in engine.ReturnValidMoves(column, row))
                                    {
                                        int moveX = (sqr[0] * boxHeight) + buffer;
                                        int moveY = (8 - sqr[1] - 1) * boxHeight;
                                       
                                        g.DrawRectangle(penValidMove, moveX, moveY, boxHeight - 1, boxHeight - 1);
                                    }
                                }
                            }


                        }
                    }
                }

 

                graphicsBuffer.Render(CreateGraphics());
                g.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Drawing Chess Board", MessageBoxButtons.OK);
				MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        #region Controls
        private void ChessBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (CheckWin())
                return;

            byte column = 0;
            byte row = 0;

            try
            {
                //Get Column
                for (int i = 0; i < 8; i++)
                {
                    if (((i * boxHeight) + 10) < e.Location.X)
                    {
                        column++;
                    }
                    else
                    {
                        break;
                    }
                }

                //Get Row
                for (int i = 0; i < 8; i++)
                {
                    if (i*boxHeight < e.Location.Y)
                    {
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
                row = (byte) (8 - row);
                column--;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Calculating Selected Column and Row", MessageBoxButtons.OK);
            }

            //Check if row and column are within bounds
            if (column > 7 || column < 0)
            {
                return;
            }
            if (row > 7 || row < 0)
            {
                return;
            }

            try
            {
                if (CurrentSource.Column == column && CurrentSource.Row == row && CurrentSource.Selected)
                {
                    //Unselect current Selection
                    CurrentSource.Selected = false;
                    CurrentDestination.Selected = false;

                    if (engine.IsPawn(column, row))
                    {
                        engine.SetGamePieceSelection(column, row, false);
                    }
                }
                else if ((CurrentSource.Column != column || CurrentSource.Row != row) && CurrentSource.Selected)
                {
                    //Make Move
                    CurrentDestination.Selected = true;
                    CurrentDestination.Column = column;
                    CurrentDestination.Row = row;
                    
                    EngineMove(true);


                    //Unselect current Selection
                    CurrentSource.Selected = false;
                    CurrentDestination.Selected = false;

                    if (engine.IsPawn(CurrentSource.Column, CurrentSource.Row))
                    {
                        engine.SetGamePieceSelection(CurrentSource.Column, CurrentSource.Row, false);
                    }
                    this.Refresh();
                }
                else
                {
                    if (engine.IsPawn(column, row))
                    {
                        if (engine.ReturnPieceColorAt(column, row) == engine.WhosMove)
                        {
                            engine.SetGamePieceSelection(column, row, true);
                        }
                        else
                        {
                            return;
                        }
                    }

                    //Select Source
                    CurrentDestination.Selected = false;
                  
                    CurrentSource.Column = column;
                    CurrentSource.Row = row;
                    CurrentSource.Selected = true;

                    
                }

                if (SourceSelected != null)
                {
                    SourceSelected(GetColumnFromInt(CurrentSource.Column + 1), (short)(CurrentSource.Row + 1));
                }

                if (DestinationSelected != null)
                {
                    DestinationSelected(GetColumnFromInt(CurrentDestination.Column), (short) (CurrentDestination.Row + 1));
                }

                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Selecting Chess Piece", MessageBoxButtons.OK);
            }
        }

        #endregion

        #region Other EventHandlers
        private void ChessBoard_Load(object sender, EventArgs e)
        {
            if (Program.IsRunning && TurnChanged != null)
            {
                TurnChanged(engine.WhosMove);
            }          
        }

        private void ChessBoard_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }
        #endregion

        #region Check Wins
        public bool CheckWin()
        {
            return engine.BlackWins || engine.WhiteWins;
        }

        public GamePieceColor? WhoWon
        {
            get
            {
                if (!CheckWin())
                    return null;
                if (engine.BlackWins) return GamePieceColor.Black;
                else return GamePieceColor.White;
            }
        }

        public bool ShowWin()
        {
            if (engine.WhiteWins)
            {
                if (DoRender)
                    MessageBox.Show("White wins!", "Game Over", MessageBoxButtons.OK);
                return true;
            }
            if (engine.BlackWins)
            {
                if (DoRender)
                    MessageBox.Show("Black wins!", "Game Over", MessageBoxButtons.OK);
                return true;
            }
            return false;
        }
        #endregion

        #endregion
    }

    public enum PlayMode
    {
        BlackCPU,
        WhiteCPU,
        CPUvsCPU
    }
}