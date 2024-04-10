using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BlockDocu.Model
{
    public class BlockDocuModel
    {
        //Private fields
        private bool[,] _gameTable;
        private bool[,] _nextBlock;
        private int _nextBlockType;
        private int _points;
        private Random _random=new Random();

        //Public properties
        public bool this[Int32 x, Int32 y]
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
        public bool NextBlock(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _nextBlock.GetLength(0))
                throw new ArgumentException("Bad column index.", nameof(x));
            if (y < 0 || y >= _nextBlock.GetLength(1))
                throw new ArgumentException("Bad row index.", nameof(y));

            return _nextBlock[x, y];
        }
        public int Points
        {
            get { return _points; }
        }


        //Constructor
        public BlockDocuModel()
        {
            _gameTable = new bool[4, 4];
            _nextBlock = new bool[2, 2];
            _points = 0;

            NewGame();
            NewBlock();
        }
        
        //Private methods
        private void CheckBlock(Int32 x, Int32 y)
        {
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    if (x == 3 || _gameTable[x, y] || _gameTable[x + 1, y])
                        throw new Exception();
                    break;
                case 2:     //Oldalra 2es
                    if (y == 3 || _gameTable[x, y] || _gameTable[x, y + 1])
                        throw new Exception();
                    break;
                case 3:     //Sima L
                    if (x == 3 || y==3 || _gameTable[x, y] || _gameTable[x + 1, y] || _gameTable[x + 1, y + 1])
                        throw new Exception();
                    break;
                case 4:     //Tukrozott L
                    if (x == 3 || y == 3 || _gameTable[x, y] || _gameTable[x, y + 1] || _gameTable[x + 1, y + 1])
                        throw new Exception();
                    break;
            }
        }
        private void PlaceBlock(Int32 x, Int32 y)
        {
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    _gameTable[x,y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x+1,y] = true;
                    OnFieldChanged(x+1, y, true);
                    break;
                case 2:     //Oldalra 2es
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x, y+1] = true;
                    OnFieldChanged(x, y+1, true);
                    break;
                case 3:     //Sima L
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x + 1, y] = true;
                    OnFieldChanged(x+1, y, true);
                    _gameTable[x + 1, y + 1] = true;
                    OnFieldChanged(x+1, y+1, true);
                    break;
                case 4:     //Tukrozott L
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x, y + 1] = true;
                    OnFieldChanged(x, y+1, true);
                    _gameTable[x + 1, y + 1] = true;
                    OnFieldChanged(x+1, y+1, true);
                    break;
            }
        }
        private void CheckFilledLines()
        {
            bool[,] tableCopy = new bool[4,4];  //Azert kell a copy, mert ha egy blokk lerakasaval több sor is kigyűlik,
                                                //azt igy lehet lekezelni
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    tableCopy[i,j]=_gameTable[i, j];
                }
            }
            for (Int32 i = 0; i < 4; i++)
            {
                if (tableCopy[i,0] && tableCopy[i,1] && tableCopy[i,2] && tableCopy[i,3])
                {
                    _gameTable[i,0] = false;
                    _gameTable[i,1] = false;
                    _gameTable[i,2] = false;
                    _gameTable[i,3] = false;
                    OnLineFilled();
                }
            }
            for (Int32 i = 0; i < 4; i++)
            {
                if (tableCopy[0, i] && tableCopy[1, i] && tableCopy[2, i] && tableCopy[3, i])
                {
                    _gameTable[0, i] = false;
                    _gameTable[1, i] = false;
                    _gameTable[2, i] = false;
                    _gameTable[3, i] = false;
                    OnLineFilled();
                }
            }
        }
        private void CheckNextCantBePlaced()
        {
            bool canBePlaced = false;
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 4; j++)
                        {
                            if (!_gameTable[i,j] && !_gameTable[i + 1,j])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 2:     //Oldalra 2es
                    for (Int32 i = 0; i < 4; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 3:     //Sima L
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i + 1, j] && !_gameTable[i + 1, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 4:     //Tukrozott L
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i, j + 1] && !_gameTable[i + 1, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
            }
            if (!canBePlaced)
                OnGameOver(_points);
        }

        //Public methods
        public void NewGame()
        {
            _points = 0;
            OnPointChanged();
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    _gameTable[i, j] = false;
                }
            }
        }
        public void NewBlock()
        {
            int rand = _random.Next(1, 5);
            _nextBlockType = rand;
            switch (rand)
            {
                case 1:     //Lefele 2es
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = false;
                    break;
                case 2:     //Oldalra 2es
                    _nextBlock[0, 0] = false;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = true;
                    break;
                case 3:     //Sima L
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = true;
                    break;
                case 4:     //Tukrozott L
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = true;
                    _nextBlock[1, 0] = false;
                    _nextBlock[1, 1] = true;
                    break;
            }
        }
        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");
            
            CheckBlock(x,y);
            PlaceBlock(x, y);

            _points++;
            OnPointChanged();

            CheckFilledLines();
            NewBlock();
            OnNextBlockChanged();
            CheckNextCantBePlaced();
        }
        //Events
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;
        public event EventHandler? NextBlockChanged;
        public event EventHandler? LineFilled;
        public event EventHandler<int>? GameOver;
        public event EventHandler? PointChanged;


        //Event triggers
        private void OnFieldChanged(Int32 x, Int32 y, bool isBlue)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, isBlue));
        }
        private void OnNextBlockChanged()
        {
            NextBlockChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnLineFilled()
        {
            LineFilled?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameOver(int points)
        {
            GameOver?.Invoke(this, points);
        }
        private void OnPointChanged()
        {
            PointChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
