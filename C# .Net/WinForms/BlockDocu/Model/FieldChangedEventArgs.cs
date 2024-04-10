using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockDocu.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }
        public bool IsBlue { get; private set; }
        public FieldChangedEventArgs(Int32 x, Int32 y, bool isBlue)
        {
            X = x;
            Y = y;
            IsBlue = isBlue;
        }
    }
}
