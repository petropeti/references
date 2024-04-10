namespace ConnectFourWinForms
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileLoadGame = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this._menuGame10 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuGame20 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuGame30 = new System.Windows.Forms.ToolStripMenuItem();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.nextPlayerLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._timeXLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._timeOLabel = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFile,
            this._menuSettings});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Padding = new System.Windows.Forms.Padding(13, 7, 0, 7);
            this._menuStrip.Size = new System.Drawing.Size(1239, 38);
            this._menuStrip.TabIndex = 1;
            this._menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            this._menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFileNewGame,
            this.toolStripMenuItem1,
            this._menuFileLoadGame,
            this._menuFileSaveGame,
            this.toolStripMenuItem2,
            this._menuFileExit});
            this._menuFile.Name = "_menuFile";
            this._menuFile.Size = new System.Drawing.Size(46, 24);
            this._menuFile.Text = "File";
            // 
            // _menuFileNewGame
            // 
            this._menuFileNewGame.Name = "_menuFileNewGame";
            this._menuFileNewGame.Size = new System.Drawing.Size(173, 26);
            this._menuFileNewGame.Text = "New game";
            this._menuFileNewGame.Click += new System.EventHandler(this.MenuNewGame_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(170, 6);
            // 
            // _menuFileLoadGame
            // 
            this._menuFileLoadGame.Name = "_menuFileLoadGame";
            this._menuFileLoadGame.Size = new System.Drawing.Size(173, 26);
            this._menuFileLoadGame.Text = "Load game..";
            this._menuFileLoadGame.Click += new System.EventHandler(this.MenuGameLoad_Click);
            // 
            // _menuFileSaveGame
            // 
            this._menuFileSaveGame.Name = "_menuFileSaveGame";
            this._menuFileSaveGame.Size = new System.Drawing.Size(173, 26);
            this._menuFileSaveGame.Text = "Save game..";
            this._menuFileSaveGame.Click += new System.EventHandler(this.MenuGameSave_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(170, 6);
            // 
            // _menuFileExit
            // 
            this._menuFileExit.Name = "_menuFileExit";
            this._menuFileExit.Size = new System.Drawing.Size(173, 26);
            this._menuFileExit.Text = "Exit";
            this._menuFileExit.Click += new System.EventHandler(this.MenuGameExit_Click);
            // 
            // _menuSettings
            // 
            this._menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuGame10,
            this._menuGame20,
            this._menuGame30});
            this._menuSettings.Name = "_menuSettings";
            this._menuSettings.Size = new System.Drawing.Size(76, 24);
            this._menuSettings.Text = "Settings";
            // 
            // _menuGame10
            // 
            this._menuGame10.Name = "_menuGame10";
            this._menuGame10.Size = new System.Drawing.Size(131, 26);
            this._menuGame10.Text = "10x10";
            this._menuGame10.Click += new System.EventHandler(this._menuGame10_Click);
            // 
            // _menuGame20
            // 
            this._menuGame20.Name = "_menuGame20";
            this._menuGame20.Size = new System.Drawing.Size(131, 26);
            this._menuGame20.Text = "20x20";
            this._menuGame20.Click += new System.EventHandler(this._menuGame20_Click);
            // 
            // _menuGame30
            // 
            this._menuGame30.Name = "_menuGame30";
            this._menuGame30.Size = new System.Drawing.Size(131, 26);
            this._menuGame30.Text = "30x30";
            this._menuGame30.Click += new System.EventHandler(this._menuGame30_Click);
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 75);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 2;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(1239, 897);
            this._tableLayoutPanel.TabIndex = 2;
            // 
            // nextPlayerLabel
            // 
            this.nextPlayerLabel.AutoSize = true;
            this.nextPlayerLabel.Location = new System.Drawing.Point(12, 38);
            this.nextPlayerLabel.Name = "nextPlayerLabel";
            this.nextPlayerLabel.Size = new System.Drawing.Size(210, 20);
            this.nextPlayerLabel.TabIndex = 3;
            this.nextPlayerLabel.Text = "PlayerX should start the game!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(239, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "PlayerX\'s time:";
            // 
            // _timeXLabel
            // 
            this._timeXLabel.AutoSize = true;
            this._timeXLabel.Location = new System.Drawing.Point(349, 38);
            this._timeXLabel.Name = "_timeXLabel";
            this._timeXLabel.Size = new System.Drawing.Size(17, 20);
            this._timeXLabel.TabIndex = 5;
            this._timeXLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(449, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "PlayerO\'s time:";
            // 
            // _timeOLabel
            // 
            this._timeOLabel.AutoSize = true;
            this._timeOLabel.Location = new System.Drawing.Point(561, 38);
            this._timeOLabel.Name = "_timeOLabel";
            this._timeOLabel.Size = new System.Drawing.Size(17, 20);
            this._timeOLabel.TabIndex = 7;
            this._timeOLabel.Text = "0";
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(658, 39);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(146, 29);
            this.pauseButton.TabIndex = 8;
            this.pauseButton.Text = "Pause Game";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 972);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this._timeOLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._timeXLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nextPlayerLabel);
            this.Controls.Add(this._tableLayoutPanel);
            this.Controls.Add(this._menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GameForm";
            this.Text = "Connect Four Game";
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuFileNewGame;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem _menuFileLoadGame;
        private ToolStripMenuItem _menuFileSaveGame;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem _menuFileExit;
        private ToolStripMenuItem _menuSettings;
        private ToolStripMenuItem _menuGame10;
        private ToolStripMenuItem _menuGame20;
        private ToolStripMenuItem _menuGame30;
        private TableLayoutPanel _tableLayoutPanel;
        private Label nextPlayerLabel;
        private Label label1;
        private Label _timeXLabel;
        private Label label3;
        private Label _timeOLabel;
        private Button pauseButton;
    }
}