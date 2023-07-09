using Breakthrough.Client.Components;
namespace Breakthrough.Client.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MenuStrip = new System.Windows.Forms.ToolStrip();
            this.bNewGame = new System.Windows.Forms.ToolStripButton();
            this.tAI = new System.Windows.Forms.ToolStripComboBox();
            this.strpStatus = new System.Windows.Forms.StatusStrip();
            this.lblSelectedColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelecteRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDestinationColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDestinationRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTurn = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlClient = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblWhiteTime = new System.Windows.Forms.Label();
            this.lblBlackTime = new System.Windows.Forms.Label();
            this.TurnTimer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tTime = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Informations = new Breakthrough.Client.Components.InfoPanel();
            this.chessBoard = new Breakthrough.Client.Components.ChessBoard();
            this.cTop = new System.Windows.Forms.CheckBox();
            this.MenuStrip.SuspendLayout();
            this.strpStatus.SuspendLayout();
            this.pnlClient.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bNewGame,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.tAI,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tTime});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(461, 25);
            this.MenuStrip.TabIndex = 0;
            // 
            // bNewGame
            // 
            this.bNewGame.Image = global::Breakthrough.Client.Properties.Resources.NGame;
            this.bNewGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNewGame.Name = "bNewGame";
            this.bNewGame.Size = new System.Drawing.Size(85, 22);
            this.bNewGame.Text = "New Game";
            this.bNewGame.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // tAI
            // 
            this.tAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tAI.Items.AddRange(new object[] {
            "Player vs Black CPU",
            "Player vs White CPU",
            "CPU vs CPU"});
            this.tAI.Name = "tAI";
            this.tAI.Size = new System.Drawing.Size(131, 25);
            this.tAI.SelectedIndexChanged += new System.EventHandler(this.tAI_SelectedIndexChanged);
            // 
            // strpStatus
            // 
            this.strpStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.strpStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSelectedColumn,
            this.lblSelecteRow,
            this.lblDestinationColumn,
            this.lblDestinationRow,
            this.lblTurn});
            this.strpStatus.Location = new System.Drawing.Point(0, 374);
            this.strpStatus.Name = "strpStatus";
            this.strpStatus.Size = new System.Drawing.Size(461, 22);
            this.strpStatus.TabIndex = 1;
            this.strpStatus.Text = "statusBar";
            // 
            // lblSelectedColumn
            // 
            this.lblSelectedColumn.Name = "lblSelectedColumn";
            this.lblSelectedColumn.Size = new System.Drawing.Size(53, 17);
            this.lblSelectedColumn.Text = "Column:";
            // 
            // lblSelecteRow
            // 
            this.lblSelecteRow.Name = "lblSelecteRow";
            this.lblSelecteRow.Size = new System.Drawing.Size(33, 17);
            this.lblSelecteRow.Text = "Row:";
            // 
            // lblDestinationColumn
            // 
            this.lblDestinationColumn.Name = "lblDestinationColumn";
            this.lblDestinationColumn.Size = new System.Drawing.Size(50, 17);
            this.lblDestinationColumn.Text = "Column";
            // 
            // lblDestinationRow
            // 
            this.lblDestinationRow.Name = "lblDestinationRow";
            this.lblDestinationRow.Size = new System.Drawing.Size(30, 17);
            this.lblDestinationRow.Text = "Row";
            // 
            // lblTurn
            // 
            this.lblTurn.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(32, 17);
            this.lblTurn.Text = "Turn";
            // 
            // pnlClient
            // 
            this.pnlClient.Controls.Add(this.Informations);
            this.pnlClient.Controls.Add(this.chessBoard);
            this.pnlClient.Controls.Add(this.pnlTop);
            this.pnlClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClient.Location = new System.Drawing.Point(0, 25);
            this.pnlClient.Name = "pnlClient";
            this.pnlClient.Size = new System.Drawing.Size(461, 349);
            this.pnlClient.TabIndex = 2;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlTop.Controls.Add(this.cTop);
            this.pnlTop.Controls.Add(this.lblWhiteTime);
            this.pnlTop.Controls.Add(this.lblBlackTime);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(461, 24);
            this.pnlTop.TabIndex = 1;
            // 
            // lblWhiteTime
            // 
            this.lblWhiteTime.AutoSize = true;
            this.lblWhiteTime.Location = new System.Drawing.Point(3, 5);
            this.lblWhiteTime.Name = "lblWhiteTime";
            this.lblWhiteTime.Size = new System.Drawing.Size(83, 13);
            this.lblWhiteTime.TabIndex = 6;
            this.lblWhiteTime.Text = "White: 00:00:00";
            // 
            // lblBlackTime
            // 
            this.lblBlackTime.AutoSize = true;
            this.lblBlackTime.Location = new System.Drawing.Point(88, 5);
            this.lblBlackTime.Name = "lblBlackTime";
            this.lblBlackTime.Size = new System.Drawing.Size(82, 13);
            this.lblBlackTime.TabIndex = 5;
            this.lblBlackTime.Text = "Black: 00:00:00";
            // 
            // TurnTimer
            // 
            this.TurnTimer.Interval = 1000;
            this.TurnTimer.Tick += new System.EventHandler(this.TurnTimer_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "SaveGame.xml";
            this.saveFileDialog.Filter = "Save Game Files|*.xml";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "SaveGame.xml";
            this.openFileDialog.Filter = "Save Game Files|*.xml";
            // 
            // tTime
            // 
            this.tTime.Name = "tTime";
            this.tTime.Size = new System.Drawing.Size(50, 25);
            this.tTime.Text = "5000";
            this.tTime.TextChanged += new System.EventHandler(this.tTime_TextChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(64, 22);
            this.toolStripLabel1.Text = "Time (ms):";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel2.Text = "Mode:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Informations
            // 
            this.Informations.BackColor = System.Drawing.Color.LightGray;
            this.Informations.Board = null;
            this.Informations.Dock = System.Windows.Forms.DockStyle.Left;
            this.Informations.Location = new System.Drawing.Point(0, 24);
            this.Informations.Name = "Informations";
            this.Informations.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Informations.Size = new System.Drawing.Size(137, 325);
            this.Informations.TabIndex = 2;
            // 
            // chessBoard
            // 
            this.chessBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chessBoard.BackColor = System.Drawing.Color.Gainsboro;
            this.chessBoard.Location = new System.Drawing.Point(143, 24);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(318, 325);
            this.chessBoard.TabIndex = 0;
            this.chessBoard.DestinationSelected += new Breakthrough.Client.Components.ChessBoard.DestinationSelectHandler(this.chessBoard_DestinationSelected);
            this.chessBoard.TurnChanged += new Breakthrough.Client.Components.ChessBoard.TurnChangedHandler(this.Board_TurnChanged);
            this.chessBoard.SourceSelected += new Breakthrough.Client.Components.ChessBoard.SourceSelectHandler(this.chessBoard_SourceSelected);
            // 
            // cTop
            // 
            this.cTop.AutoSize = true;
            this.cTop.Location = new System.Drawing.Point(216, 3);
            this.cTop.Name = "cTop";
            this.cTop.Size = new System.Drawing.Size(92, 17);
            this.cTop.TabIndex = 7;
            this.cTop.Text = "Always on top";
            this.cTop.UseVisualStyleBackColor = true;
            this.cTop.CheckedChanged += new System.EventHandler(this.cTop_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(461, 396);
            this.Controls.Add(this.pnlClient);
            this.Controls.Add(this.strpStatus);
            this.Controls.Add(this.MenuStrip);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Breakthrough";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.strpStatus.ResumeLayout(false);
            this.strpStatus.PerformLayout();
            this.pnlClient.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MenuStrip;
        private System.Windows.Forms.StatusStrip strpStatus;
        private System.Windows.Forms.Panel pnlClient;
        private Components.ChessBoard chessBoard;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedColumn;
        private System.Windows.Forms.ToolStripStatusLabel lblSelecteRow;
        private System.Windows.Forms.ToolStripStatusLabel lblDestinationColumn;
        private System.Windows.Forms.ToolStripStatusLabel lblDestinationRow;
        private System.Windows.Forms.Label lblBlackTime;
        private System.Windows.Forms.Label lblWhiteTime;
        private System.Windows.Forms.Timer TurnTimer;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel lblTurn;
        private Breakthrough.Client.Components.InfoPanel Informations;
        private System.Windows.Forms.ToolStripButton bNewGame;
        private System.Windows.Forms.ToolStripComboBox tAI;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tTime;
        private System.Windows.Forms.CheckBox cTop;

        
    }
}