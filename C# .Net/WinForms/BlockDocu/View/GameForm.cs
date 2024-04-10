using BlockDocu.Model;
using System.Numerics;
using System.Windows.Forms;

namespace BlockDocu.View
{
    public partial class GameForm : Form
    {
        //Private fields

        private BlockDocuModel _model;
        private Button[,] _buttonGrid;
        private Button[,] _nextBlockGrid;

        //Constructor
        public GameForm()
        {
            InitializeComponent();
            _model = new BlockDocuModel();
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.NextBlockChanged += new EventHandler(Model_NextBlockChanged);
            _model.LineFilled += new EventHandler(Model_LineFilled);
            _model.GameOver += new EventHandler<int>(Model_GameOver);
            _model.PointChanged += new EventHandler(Model_PointChanged);

            _buttonGrid = null!;
            _nextBlockGrid = null!;

            GenerateTable();
            GenerateNextBlock();
            SetNextBlock();
        }

        //Private methods
        private void GenerateTable()
        {
            _buttonGrid = new Button[4, 4];
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    _buttonGrid[i, j] = new GridButton(i, j);
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].Size = new Size(100,100);
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    mainTable.Controls.Add(_buttonGrid[i, j], j, i);
                }
            }
        }
        private void GenerateNextBlock()
        {
            _nextBlockGrid = new Button[2, 2];
            for (Int32 i = 0; i < 2; i++)
            {
                for (Int32 j = 0; j < 2; j++)
                {
                    _nextBlockGrid[i, j] = new GridButton(i, j);
                    _nextBlockGrid[i, j].BackColor = Color.White;
                    _nextBlockGrid[i, j].Size = new Size(100, 100);
                    _nextBlockGrid[i, j].Enabled = false;

                    nextBlock.Controls.Add(_nextBlockGrid[i, j], j, i);
                }
            }
        }
        private void SetTable()
        {
            for (Int32 i = 0; i < 4; i++)
                for (Int32 j = 0; j < 4; j++)
                {
                    switch (_model[i, j])
                    {
                        case false:
                            _buttonGrid[i, j].BackColor = Color.White;
                            break;
                        case true:
                            _buttonGrid[i, j].BackColor = Color.Blue;
                            break;  
                    }
                }
        }
        private void SetNextBlock()
        {
            for (Int32 i = 0; i < 2; i++)
                for (Int32 j = 0; j < 2; j++)
                {
                    switch (_model.NextBlock(i,j))
                    {
                        case false:
                            _nextBlockGrid[i, j].BackColor = Color.White;
                            break;
                        case true:
                            _nextBlockGrid[i, j].BackColor = Color.Blue;
                            break;
                    }
                }
        }

        //Event handlers
        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is GridButton button)
            {
                Int32 x = button.GridX;
                Int32 y = button.GridY;

                try
                {
                    _model.StepGame(x, y);
                }
                catch
                {
                }
            }
        }
        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            switch (e.IsBlue)
            {
                case true:
                    _buttonGrid[e.X, e.Y].BackColor = Color.Blue;
                    break;
                case false:
                    _buttonGrid[e.X, e.Y].BackColor = Color.White;
                    break;
            }
        }
        private void Model_NextBlockChanged(object? sender, EventArgs e)
        {
            SetNextBlock();
        }
        private void Model_LineFilled(object? sender, EventArgs e)
        {
            SetTable();
        }
        private void Model_GameOver(object? sender, int e)
        {
            DialogResult dialogResult = 
                MessageBox.Show("Congratulations!\nYour score is: " + e.ToString() +" points!\nDo you want to start a new game?",
                                                    "Game Over", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _model.NewGame();
                SetTable();
                _model.NewBlock();
                SetNextBlock();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }
        private void Model_PointChanged(object? sender, EventArgs e)
        {
            pointLabel.Text = _model.Points.ToString();
        }
    }
}
