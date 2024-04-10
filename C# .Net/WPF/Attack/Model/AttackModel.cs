using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attack.Model
{
    public class AttackModel
    {
        //Private fields

        private int _size;
        private TableField[,] _gameTable;
        private bool[,] _isAvailable;
        private TableField _currentPiece;
        private bool[] _p1LivePieces;
        private bool[] _p2LivePieces;
        private int _p1CurrentPieceNum;
        private int _p2CurrentPieceNum;
        private int _p1PieceCount;
        private int _p2PieceCount;

        //Public properties

        public int TableSize { get { return _size; } }
        public TableField this[Int32 x, Int32 y]
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
        public bool IsAvailable (Int32 x, Int32 y)
        {
            return _isAvailable[x, y];
        }

        //Constructor
        public AttackModel()
        {
            _size = 6;
            _gameTable = new TableField[_size, _size];
            _isAvailable = new bool[_size, _size];
            _currentPiece = new TableField();
            _p1LivePieces = new bool[5];
            _p2LivePieces = new bool[5];
        }

        //Public methods
        public void NewGame()
        {
            for (Int32 i = 0; i < _size; i++)
            {
                for (Int32 j = 0; j < _size; j++)
                {
                    _gameTable[i, j] = new TableField();
                    _gameTable[i, j].Player = Player.NoPlayer;
                    _gameTable[i, j].Piece = 0;
                    _gameTable[i, j].X = i;
                    _gameTable[i, j].Y = j;
                }
            }
            _gameTable[0, _size-1].Player = Player.Player1;
            _gameTable[0, _size-1].Piece = 4;
            _gameTable[0, _size - 2].Player = Player.Player1;
            _gameTable[0, _size - 2].Piece = 3;
            _gameTable[1, _size-1].Player = Player.Player1;
            _gameTable[1, _size-1].Piece = 1;
            _gameTable[1, _size - 2].Player = Player.Player1;
            _gameTable[1, _size - 2].Piece = 2;

            _gameTable[_size - 2, 0].Player = Player.Player2;
            _gameTable[_size - 2, 0].Piece = 1;
            _gameTable[_size - 2, 1].Player = Player.Player2;
            _gameTable[_size - 2, 1].Piece = 2;
            _gameTable[_size - 1, 0].Player = Player.Player2;
            _gameTable[_size - 1, 0].Piece = 4;
            _gameTable[_size - 1, 1].Player = Player.Player2;
            _gameTable[_size - 1, 1].Piece = 3;

            _currentPiece.Player= Player.Player1;
            _currentPiece.Piece = 1;
            _currentPiece.X = 1;
            _currentPiece.Y = (_size - 1);

            for (int i = 1; i < 5; i++)
            {
                _p1LivePieces[i] = true;
                _p2LivePieces[i] = true;
            }
            _p1CurrentPieceNum = 1;
            _p2CurrentPieceNum = 1;
            _p1PieceCount = 4;
            _p2PieceCount = 4;

            ManageAvailability();

            OnGameStarted();
            OnNextPieceChanged();
        }
        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");
            if (!_isAvailable[x, y])
                throw new Exception();

            if (_gameTable[x,y].Player!=Player.NoPlayer)    //Ütés történt
            {
                switch (_gameTable[x,y].Player)
                {
                    case Player.Player1:
                        _p1PieceCount--;
                        _p1LivePieces[_gameTable[x, y].Piece] = false;
                        if (_p1PieceCount > 0)
                        {
                            while (!_p1LivePieces[_p1CurrentPieceNum])
                            {
                                _p1CurrentPieceNum = NextNum(_p1CurrentPieceNum);
                            }
                        }
                        break;
                    case Player.Player2:
                        _p2PieceCount--;
                        _p2LivePieces[_gameTable[x, y].Piece] = false;
                        if (_p2PieceCount > 0)
                        {
                            while (!_p2LivePieces[_p2CurrentPieceNum])
                            {
                                _p2CurrentPieceNum = NextNum(_p2CurrentPieceNum);
                            }
                        }
                        break;
                }
            }

            _gameTable[x, y].Player = _currentPiece.Player;
            _gameTable[x, y].Piece = _currentPiece.Piece;
            OnFieldChanged(x, y, _currentPiece.Player, _currentPiece.Piece);

            _gameTable[_currentPiece.X, _currentPiece.Y].Player = Player.NoPlayer;
            _gameTable[_currentPiece.X, _currentPiece.Y].Piece = 0;
            OnFieldChanged(_currentPiece.X, _currentPiece.Y, Player.NoPlayer,0);

            if (!CheckGame())
            {
                NextPiece();
                ManageAvailability();

                OnTableChanged();
                OnNextPieceChanged();
            }
        }
        //Private methods
        private void ManageAvailability()
        {
            for (Int32 i = 0; i < _size; i++)
            {
                for (Int32 j = 0; j < _size; j++)
                {
                    _isAvailable[i, j] = false;
                }
            }
            int x = _currentPiece.X;
            int y = _currentPiece.Y;
            for (Int32 i = x - 1; i <= x + 1; i++)
            {
                for (Int32 j = y - 1; j <= y + 1; j++)
                {
                    if (i < _size && j < _size && i>=0 && j>=0 && !(i == x && j == y)
                        && _gameTable[i, j].Player == Player.NoPlayer)
                    {
                        _isAvailable[i, j] = true;
                    }
                    if (i < _size && j < _size && i >= 0 && j >= 0 && 
                        ( (i, j) == (x - 1, y - 1) || (i, j) == (x - 1, y + 1) ||
                        (i, j) == (x + 1, y - 1) || (i, j) == (x + 1, y + 1)) && 
                        _gameTable[i, j].Player == OtherPlayer(_currentPiece.Player))
                    {
                        _isAvailable[i, j] = true;
                    }
                }
            }
        }
        private void NextPiece()
        {
            switch ( _currentPiece.Player )
            {
                case Player.Player1 :
                    if (_p1PieceCount > 0)
                    {
                        do
                        {
                            _p1CurrentPieceNum = NextNum(_p1CurrentPieceNum);
                        } while (!_p1LivePieces[_p1CurrentPieceNum]);
                    }
                    if (_p2PieceCount>0)
                    {
                        _currentPiece.Piece = _p2CurrentPieceNum;
                        _currentPiece.Player = Player.Player2;
                    }
                    else
                    {
                        _currentPiece.Piece = _p1CurrentPieceNum;
                        _currentPiece.Player = Player.Player1;
                    }
                    break;
                case Player.Player2 :
                    if (_p2PieceCount > 0)
                    {
                        do
                        {
                            _p2CurrentPieceNum = NextNum(_p2CurrentPieceNum);
                        } while (!_p2LivePieces[_p2CurrentPieceNum]);
                    }
                    if (_p1PieceCount > 0)
                    {
                        _currentPiece.Piece = _p1CurrentPieceNum;
                        _currentPiece.Player = Player.Player1;
                    }
                    else
                    {
                        _currentPiece.Piece = _p2CurrentPieceNum;
                        _currentPiece.Player = Player.Player2;
                    }
                    break;
            }
            (_currentPiece.X, _currentPiece.Y) = FindPiece();
        }
        private static int NextNum(int x)
        {
            if (x < 4)
                return x + 1;
            return 1;
        }
        private (int, int) FindPiece()
        {
            for (Int32 i = 0; i < _size; i++)
            {
                for (Int32 j = 0; j < _size; j++)
                {
                    if (_gameTable[i,j].Player == _currentPiece.Player && _gameTable[i,j].Piece == _currentPiece.Piece)
                    {
                        return (i, j);
                    }
                }
            }
            return (0,0);
        }
        private static Player OtherPlayer(Player p)
        {
            switch (p)
            {
                case Player.Player1:
                    return Player.Player2;
                case Player.Player2:
                    return Player.Player1;
                default:
                    return Player.NoPlayer;
            }
        }
        private bool CheckGame()
        {
            if (_gameTable[0, _size - 1].Player == Player.Player2)
            {
                OnGameWon(Player.Player2);
                return true;
            }
            if (_gameTable[_size - 1, 0].Player == Player.Player1)
            {
                OnGameWon(Player.Player1);
                return true;
            }
            return false;
        }
        //Events
        public event EventHandler? GameStarted;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;
        public event EventHandler<FieldChangedEventArgs>? NextPieceChanged;
        public event EventHandler? TableChanged;
        public event EventHandler<Player>? GameWon;

        //Event triggers
        private void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
        private void OnFieldChanged(Int32 x, Int32 y, Player player, Int32 pieceNumber)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, player, pieceNumber));
        }
        private void OnNextPieceChanged()
        {
            NextPieceChanged?.Invoke(this, new FieldChangedEventArgs(_currentPiece.X, _currentPiece.Y, _currentPiece.Player, _currentPiece.Piece));
        }
        private void OnTableChanged()
        {
            TableChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameWon(Player p)
        {
            GameWon?.Invoke(this, p);
        }
    }
}
