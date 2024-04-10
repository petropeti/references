using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Persistence;

namespace ConnectFour.Model
{
    public class ConnectFourGameModel
    {
        private Player _currentPlayer;
        private Player[,] _gameTable;
        private bool[,] _isAvailable;
        private Int32 _stepNumber;
        private Int32 _gameTimeX;
        private Int32 _gameTimeO;
        private IPersistence _persistence;
        private bool _isGameOver;

        public Int32 StepNumber { get { return _stepNumber; } }


        public int TableSize
        {
            get { return _gameTable.GetLength(0); }
        }

        public Player CurrentPlayer { get { return _currentPlayer; } }
        public Int32 GameTimeX { get { return _gameTimeX; } }
        public Int32 GameTimeO { get { return _gameTimeO; } }


        public Player this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= _gameTable.GetLength(0))
                    throw new ArgumentException("Bad column index.", nameof(x));
                if (y < 0 || y >= _gameTable.GetLength(1))
                    throw new ArgumentException("Bad row index.", nameof(y));

                return _gameTable[x, y];
            }
        }

        public ConnectFourGameModel(IPersistence persistence, int size)
        {
            _gameTable = new Player[size, size];
            _isAvailable= new bool[size, size];
            _persistence = persistence;

            NewGame();
        }

        public void NewGame()
        {
            _gameTimeO = 0;
            _gameTimeX = 0;
            _isGameOver = false;
            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    _gameTable[i, j] = Player.NoPlayer;
                    _isAvailable[i, j] = false;
                }
            }
            for (Int32 i = 0; i < _gameTable.GetLength(1); i++)
            {
                _isAvailable[_gameTable.GetLength(0) -1 , i] = true;
            }

            _stepNumber = 0;
            _currentPlayer = Player.PlayerX;
        }

        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");
            if (_stepNumber >= _gameTable.Length)
                throw new InvalidOperationException("Game is over!");
            if (_gameTable[x, y] != Player.NoPlayer)
                throw new InvalidOperationException("Field is not empty!");
            if (!_isAvailable[x, y])
                throw new InvalidOperationException("Gravity in enabled!");

            _gameTable[x, y] = _currentPlayer;
            if (x!=0)
            _isAvailable[x - 1, y] = true;
            OnFieldChanged(x, y, _currentPlayer);

            _stepNumber++;
            _currentPlayer = _currentPlayer == Player.PlayerO ? Player.PlayerX : Player.PlayerO;


            CheckGame();
        }

        public void LoadGame(String path)
        {
            (Player[] values,_gameTimeX, _gameTimeO) = _persistence.Load(path);

            int size = (int)Math.Sqrt(values.Length);
            OnChangeSize(size);
            _gameTable = new Player[size, size];
            _isAvailable= new bool[size,size];

            if (_gameTable.Length == 0)
                throw new DataException("Error occurred during game loading.");

            if (values.Count(value => value == Player.PlayerO) != values.Count(value => value == Player.PlayerX) && values.Count(value => value == Player.PlayerO) + 1 != values.Count(value => value == Player.PlayerX))
                throw new DataException("Error occurred during game loading.");

            _stepNumber = values.Count(value => value != Player.NoPlayer);
            _currentPlayer = _stepNumber % 2 == 0 ? Player.PlayerX : Player.PlayerO;

            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    _gameTable[i, j] = values[i * _gameTable.GetLength(0) + j];
                    if (i>0 && _gameTable[i,j]!= Player.NoPlayer)
                        _isAvailable[i-1, j] = true;

                    OnFieldChanged(i, j, _gameTable[i, j]);
                }
            }

            for (Int32 i = 0; i < _gameTable.GetLength(1); i++)
            {
                Int32 j = 0;
                while (j < _gameTable.GetLength(0) && _gameTable[j, i] == Player.NoPlayer)
                {
                    j++;
                }
                if (j < _gameTable.GetLength(0))
                {
                    j++;
                    while (j < _gameTable.GetLength(0))
                    {
                        if (_gameTable[j,i]== Player.NoPlayer)
                            throw new DataException("Error occurred during game loading.");
                        j++;
                    }
                }
            }
            for (Int32 i = 0; i < _gameTable.GetLength(1); i++)
            {
                _isAvailable[_gameTable.GetLength(0) - 1, i] = true;
            }
            CheckGame();
        }

        public void SaveGame(String path)
        {
            Player[] values = new Player[_gameTable.Length];
            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    values[i * _gameTable.GetLength(0) + j] = _gameTable[i, j];
                }
            }
            _persistence.Save(path, values, _gameTimeX,_gameTimeO);
        }

        private void CheckGame()
        {
            Player won = Player.NoPlayer;
            int winCountX = 0;
            int winCountO = 0;
            (int, int) startCoord=(0,0);
            int direction=0; //0 ha vizszintes, 1 ha jobble atlo, -1 ha balle atlo
            
            //Vizszintes
            for (int i = 0; i < _gameTable.GetLength(0); ++i)
                for (int j = 0; j < _gameTable.GetLength(1) - 3; ++j)
                {
                    if (_gameTable[i, j] != Player.NoPlayer && _gameTable[i, j] == _gameTable[i, j + 1] &&
                        _gameTable[i, j] == _gameTable[i, j + 2] && _gameTable[i, j] == _gameTable[i, j + 3])
                    {
                        won = _gameTable[i, j];
                        if (won == Player.PlayerX)
                            winCountX++;
                        else
                            winCountO++;
                        direction = 0;
                        startCoord = (i, j);
                    }
                }
            //Jobb-le átló (\)
            for (int i = 0; i < _gameTable.GetLength(0) - 3; ++i)
                for (int j = 0; j < _gameTable.GetLength(1) - 3; ++j)
                {
                    if (_gameTable[i, j] != Player.NoPlayer &&
                        _gameTable[i, j] == _gameTable[i + 1, j + 1] && _gameTable[i, j] == _gameTable[i + 2, j + 2] && _gameTable[i, j] == _gameTable[i + 3, j + 3])
                    {
                        won = _gameTable[i, j];
                        if (won == Player.PlayerX)
                            winCountX++;
                        else
                            winCountO++;
                        direction = 1;
                        startCoord = (i, j);
                    }
                }
            //Bal-le átló (/)
            for (int i = 3; i < _gameTable.GetLength(0); ++i)
                for (int j = 0; j < _gameTable.GetLength(1) - 3; ++j)
                {
                    if (_gameTable[i, j] != Player.NoPlayer &&
                        _gameTable[i, j] == _gameTable[i - 1, j + 1] && _gameTable[i, j] == _gameTable[i - 2, j + 2] && _gameTable[i, j] == _gameTable[i - 3, j + 3])
                    {
                        won = _gameTable[i, j];
                        if (won == Player.PlayerX)
                            winCountX++;
                        else
                            winCountO++;
                        direction = -1;
                        startCoord = (i, j);
                    }
                }

            if (winCountO!=0 && winCountX!=0)
                throw new DataException("Error occurred during game loading.");

            if (won != Player.NoPlayer)
            {
                OnGameWon(won,direction,startCoord);
            }
            else if (_stepNumber == _gameTable.Length) 
            {
                OnGameOver(); 
            }
        }

        public void AdvanceTime()
        {
             if (_isGameOver)
               return;

            if (_currentPlayer == Player.PlayerX)
                _gameTimeX++;
            else
                _gameTimeO++;

            OnGameAdvanced();

        }


        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler? GameOver;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;
        public event EventHandler? GameAdvanced;
        public event EventHandler<int>? ChangeSize;


        private void OnGameWon(Player player, int direction, (int,int) startCoord)
        {
            _isGameOver = true;
            GameWon?.Invoke(this, new GameWonEventArgs(player, direction, startCoord));
        }
        private void OnGameOver()
        {
            _isGameOver = true;
            GameOver?.Invoke(this, EventArgs.Empty);
        }
        private void OnFieldChanged(Int32 x, Int32 y, Player player)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, player));
        }
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, EventArgs.Empty);
        }
        private void OnChangeSize(int size)
        {
            ChangeSize?.Invoke(this, size);
        }
    }
}
