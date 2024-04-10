using System;
using System.Windows.Forms;
using ConnectFour.Model;
using ConnectFour.Persistence;

namespace ConnectFourWinForms
{
    public partial class GameForm : Form
    {
        private ConnectFourGameModel _model;
        private Button[,] _buttonGrid;
        private int _size = 10;
        private System.Windows.Forms.Timer _timer = null!;

        public GameForm()
        {
            InitializeComponent();
            _model = new ConnectFourGameModel(new TextFilePersistence(), _size);

            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.GameOver += new EventHandler(Model_GameOver);
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);
            _model.GameAdvanced += new EventHandler(Game_GameAdvanced);
            _model.ChangeSize += new EventHandler<int>(Game_ChangeSize);


            _buttonGrid = null!;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            GenerateTable();
            _timer.Start();
        }

        private void GenerateTable()
        {
            _tableLayoutPanel.RowCount = _tableLayoutPanel.ColumnCount = _size;

            _buttonGrid = new Button[_size, _size];
            int ratio = 500 / _size;

            for (Int32 i = 0; i < _size; i++)
            {
                for (Int32 j = 0; j < _size; j++)
                {
                    _buttonGrid[i, j] = new GridButton(i, j);
                    _buttonGrid[i, j].Location = new Point(ratio * i, ratio * j);
                    _buttonGrid[i, j].Size = new Size(ratio, ratio);
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, Height*ratio / 5000F, FontStyle.Bold);
                    _buttonGrid[i, j].Dock = DockStyle.None;
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    _tableLayoutPanel.Controls.Add(_buttonGrid[i, j], j, i);
                }
            }
            _tableLayoutPanel.RowStyles.Clear();
            _tableLayoutPanel.ColumnStyles.Clear();

            for (Int32 i = 0; i < _size; i++)
            {
                _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1 / _size));
            }
            for (Int32 j = 0; j < _size; j++)
            {
                _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1 / _size));
            }

        }
        private void SetTable()
        {
            for (Int32 i = 0; i < _size; i++)
                for (Int32 j = 0; j < _size; j++)
                {
                    switch (_model[i, j])
                    {
                        case Player.PlayerO:
                            _buttonGrid[i, j].BackColor = Color.Yellow;
                            _buttonGrid[i, j].Text = "O";
                            break;
                        case Player.PlayerX:
                            _buttonGrid[i, j].BackColor = Color.Red;
                            _buttonGrid[i, j].Text = "X";
                            break;
                        case Player.NoPlayer:
                            _buttonGrid[i, j].BackColor = Color.White;
                            _buttonGrid[i, j].Text = String.Empty;
                            break;
                    }
                }
        }

        private void Model_GameWon(object? sender, GameWonEventArgs e)
        {
            switch (e.Direction)
            {
                case 0: //vizszintes
                    for (int i = 0; i < 4; i++)
                    {
                        _buttonGrid[e.StartX, e.StartY + i].BackColor = Color.Green;
                    }
                    break;
                case 1: //jobble
                    for (int i = 0; i < 4; i++)
                    {
                        _buttonGrid[e.StartX+i, e.StartY + i].BackColor = Color.Green;
                    }
                    break;
                case -1: //balle
                    for (int i = 0; i < 4; i++)
                    {
                        _buttonGrid[e.StartX-i, e.StartY + i].BackColor = Color.Green;
                    }
                    break;
            }
            switch (e.Player)
            {
                case Player.PlayerO:
                    MessageBox.Show("A kör játékos gyõzött!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                case Player.PlayerX:
                    MessageBox.Show("A kereszt játékos gyõzött!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
            }
            _model.NewGame();
            SetTable();
        }

        private void Model_GameOver(object? sender, EventArgs e)
        {
            MessageBox.Show("Döntetlen játék!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            _model.NewGame();
            SetTable();
        }

        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            switch (e.Player)
            {
                case Player.PlayerO:
                    _buttonGrid[e.X, e.Y].BackColor = Color.Yellow;
                    _buttonGrid[e.X, e.Y].Text = "O";
                    break;
                case Player.PlayerX:
                    _buttonGrid[e.X, e.Y].BackColor = Color.Red;
                    _buttonGrid[e.X, e.Y].Text = "X";
                    break;
                case Player.NoPlayer:
                    _buttonGrid[e.X, e.Y].BackColor = Color.White;
                    _buttonGrid[e.X, e.Y].Text = String.Empty;
                    break;
            }
        }


        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is GridButton button)
            {
                Int32 x = button.GridX;
                Int32 y = button.GridY;

                try
                {
                    _model.StepGame(x, y);
                    if (_model.CurrentPlayer == Player.PlayerO)
                        nextPlayerLabel.Text = "PlayerO make a move!";
                    else
                        nextPlayerLabel.Text = "PlayerX make a move!";
                }
                catch
                {
                }
            }
        }

        private void MenuGameLoad_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _model.LoadGame(openFileDialog.FileName);
                        pauseButton.Text = "Start Game";
                        if (_model.CurrentPlayer == Player.PlayerO)
                            nextPlayerLabel.Text = "PlayerO make a move!";
                        else
                            nextPlayerLabel.Text = "PlayerX make a move!";
                        OnPauseGame();
                    }
                    catch (DataException)
                    {
                        MessageBox.Show("Hiba keletkezett a betöltés során.", "ConnectFour", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void MenuGameSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "C:\\";
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _model.SaveGame(saveFileDialog.FileName);
                    }
                    catch (DataException)
                    {
                        MessageBox.Show("Hiba keletkezett a mentés során.", "ConnectFour", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void MenuGameExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void MenuNewGame_Click(object? sender, EventArgs e)
        {
            _model.NewGame();
            pauseButton.Text = "Start Game";
            OnPauseGame();
            SetTable();
        }

        private void _menuGame10_Click(object sender, EventArgs e)
        {
            changeSize(10);
        }

        private void _menuGame20_Click(object sender, EventArgs e)
        {
            changeSize(20);
        }

        private void _menuGame30_Click(object sender, EventArgs e)
        {
            changeSize(30);
        }

        private void changeSize(int size)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _tableLayoutPanel.Controls.Remove(_buttonGrid[i, j]);
                }
            }
            _size = size;
            _model = new ConnectFourGameModel(new TextFilePersistence(), _size);
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.GameOver += new EventHandler(Model_GameOver);
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);
            _model.GameAdvanced += new EventHandler(Game_GameAdvanced);
            _model.ChangeSize += new EventHandler<int>(Game_ChangeSize);
            _buttonGrid = null!;
            GenerateTable();
            _model.NewGame();
        }

        private void Timer_Tick(Object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        private void Game_GameAdvanced(Object? sender, EventArgs e)
        {
            _timeXLabel.Text = TimeSpan.FromSeconds(_model.GameTimeX).ToString("g");
            _timeOLabel.Text = TimeSpan.FromSeconds(_model.GameTimeO).ToString("g");

        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            OnPauseGame();

        }
        private void OnPauseGame()
        {
            if (pauseButton.Text == "Pause Game")
            {
                _timer.Stop();
                pauseButton.Text = "Start Game";
                for (Int32 i = 0; i < _size; i++)
                    for (Int32 j = 0; j < _size; j++)
                    {
                        _buttonGrid[i, j].Enabled = false;
                    }
            }
            else if (pauseButton.Text == "Start Game")
            {
                _timer.Start();
                pauseButton.Text = "Pause Game";
                for (Int32 i = 0; i < _size; i++)
                    for (Int32 j = 0; j < _size; j++)
                    {
                        _buttonGrid[i, j].Enabled = true;
                    }
            }
        }

        private void Game_ChangeSize(object? sender, int e)
        {
            changeSize(e);
        }
    }
}