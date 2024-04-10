using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public Player Player { get; private set; }
        public Int32 PieceNumber { get; private set; }
        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }
        public FieldChangedEventArgs(Int32 x, Int32 y, Player player, Int32 pieceNumber)
        {
            Player = player;
            X = x;
            Y = y;
            PieceNumber = pieceNumber;
        }
    }
}
