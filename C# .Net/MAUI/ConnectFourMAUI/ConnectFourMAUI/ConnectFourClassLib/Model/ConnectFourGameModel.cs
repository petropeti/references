using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ConnectFourClassLib.Persistence;

namespace ConnectFourClassLib.Model
{
    public class ConnectFourGameModel
    {
        #region Private fields

        private Player _currentPlayer;
        private Player[,] _gameTable;
        private bool[,] _isAvailable;
        private Int32 _stepNumber;
        private Int32 _gameTimeX;
        private Int32 _gameTimeO;
        private IPersistence _persistence;
        private bool _isGameOver;
        private int _size;

        #endregion

        #region Public properties

        public Int32 StepNumber { get { return _stepNumber; } }
        public int TableSize { get { return _size; } }
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

        #endregion

        #region Constructors

        public ConnectFourGameModel(IPersistence persistence, int size)
        {
            _size = size;

            _gameTable = new Player[_size, _size];
            _isAvailable = new bool[_size, _size];
            _persistence = persistence;

            NewGame();
        }

        #endregion

        #region Public methods

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
            OnCurrentPlayerChanged();

            OnGameStarted();
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
            OnCurrentPlayerChanged();

            CheckGame();
        }
        public async Task LoadGameAsync(String path)
        {
            if (_persistence == null)
                return;

            (Player[] values, _gameTimeX, _gameTimeO) = await _persistence.LoadAsync(path);

            _size = (int)Math.Sqrt(values.Length);
            if (!(_size == 10 || _size == 20 || _size == 30))
                throw new DataException("Error occurred during game loading: Wrong table size.");

            _gameTable = new Player[_size, _size];
            _isAvailable= new bool[_size, _size];

            OnChangeSize(_size);

            if (_gameTable.Length == 0)
                throw new DataException("Error occurred during game loading: Empty file.");

            if (values.Count(value => value == Player.PlayerO) != values.Count(value => value == Player.PlayerX) && values.Count(value => value == Player.PlayerO) + 1 != values.Count(value => value == Player.PlayerX))
                throw new DataException("Error occurred during game loading: Too much difference between X and O.");

            _stepNumber = values.Count(value => value != Player.NoPlayer);
            _currentPlayer = _stepNumber % 2 == 0 ? Player.PlayerX : Player.PlayerO;
            OnCurrentPlayerChanged();

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
                            throw new DataException("Error occurred during game loading: Gravity is enabled.");
                        j++;
                    }
                }
            }
            for (Int32 i = 0; i < _gameTable.GetLength(1); i++)
            {
                _isAvailable[_gameTable.GetLength(0) - 1, i] = true;
            }

            OnGameStarted();
            CheckGame();
        }
        public async Task SaveGameAsync(String path)
        {
            if (_persistence == null)
                return;

            Player[] values = new Player[_gameTable.Length];
            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    values[i * _gameTable.GetLength(0) + j] = _gameTable[i, j];
                }
            }
            await _persistence.SaveAsync(path, values, _gameTimeX,_gameTimeO);
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
        public void TableInit(int size)
        {
            _size = size;
            _gameTable = new Player[size, size];
            _isAvailable = new bool[size, size];
        }

        #endregion

        #region Private methods

        private void CheckGame()
        {
            Player won = Player.NoPlayer;
            int winCountX = 0;
            int winCountO = 0;
            (int, int) startCoord = (0, 0);
            int direction = 0; //0 ha vizszintes, 1 ha jobble atlo, -1 ha balle atlo

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

            if (winCountO != 0 && winCountX != 0)
                throw new DataException("Error occurred during game loading: More than one player have already won.");

            if (won != Player.NoPlayer)
            {
                OnGameWon(won, direction, startCoord);
            }
            else if (_stepNumber == _gameTable.Length)
            {
                OnGameOver();
            }
        }

        #endregion

        #region Events

        public event EventHandler? GameStarted;
        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler? GameOver;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;
        public event EventHandler? GameAdvanced;
        public event EventHandler<int>? ChangeSize;
        public event EventHandler? CurrentPlayerChanged;

        #endregion

        #region Event triggers

        private void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
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
        private void OnCurrentPlayerChanged()
        {
            CurrentPlayerChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
