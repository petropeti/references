using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Model
{
    public class GameModel
    {
        //Private fields

        private bool[,] _gameTable;

        //Properties
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

        //Constructor
        public GameModel()
        {
            _gameTable = new bool[12, 12];
        }

        //Public methods
        public void NewGame()
        {
            for (Int32 i = 0; i < 12; i++)
            {
                for (Int32 j = 0; j < 12; j++)
                {
                    _gameTable[i, j] = false;
                }
            }

            OnGameStarted();
        }
        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");

            _gameTable[x, y] = !_gameTable[x, y];
            OnFieldChanged(x, y, _gameTable[x,y]);


        }
        public void AdvanceTime()
        {
            Simulate();
        }

        //Private methods
        private void Simulate()
        {
            bool[,] newTable = new bool[12, 12];
            for (Int32 i = 0; i < 12; i++)
            {
                for (Int32 j = 0; j < 12; j++)
                {
                    newTable[i, j] = false;
                }
            }
            for (Int32 x = 0; x < 12; x++)
            {
                for (Int32 y = 0; y < 12; y++)
                {
                    if (_gameTable[x,y])    //Élő
                    {
                        int count = 0;
                        for (Int32 i = x - 1; i <= x + 1; i++)
                        {
                            for (Int32 j = y - 1; j <= y + 1; j++)
                            {
                                if (i >= 0 && i <= 11 && j >= 0 && j <= 11 && !(x == i && y == j) && _gameTable[i, j])
                                {
                                    count++;
                                }
                            }
                        }
                        if (count==2 || count==3)
                        {
                            newTable[x, y] = true;
                        }
                        else
                        {
                            newTable[x, y] = false;
                        }
                    }
                    else                     //Halott
                    {
                        int count = 0;
                        for (Int32 i = x-1; i <= x+1; i++)
                        {
                            for (Int32 j = y-1; j <= y+1; j++)
                            {
                                if (i>=0 && i<=11 && j>=0 && j<=11 && !(x==i && y==j) && _gameTable[i,j])
                                {
                                    count++;
                                }
                            }
                        }
                        if (count == 3)
                        {
                            newTable[x, y] = true;
                        }
                    }
                }
            }
            for (Int32 i = 0; i < 12; i++)
            {
                for (Int32 j = 0; j < 12; j++)
                {
                    _gameTable[i,j]=newTable[i, j];
                    OnFieldChanged(i, j, _gameTable[i, j]);
                }
            }

        }

        //Events
        public event EventHandler? GameStarted;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;

        //Event triggers
        private void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
        private void OnFieldChanged(Int32 x, Int32 y, bool isAlive)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, isAlive));
        }
       
        

    }
}
