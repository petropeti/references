using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public bool IsAlive { get; private set; }
        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }
        public FieldChangedEventArgs(Int32 x, Int32 y, bool isAlive)
        {
            IsAlive = IsAlive;
            X = x;
            Y = y;
        }
    }
}
