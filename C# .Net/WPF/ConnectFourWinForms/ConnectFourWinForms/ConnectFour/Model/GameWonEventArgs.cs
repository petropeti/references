using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Persistence;

namespace ConnectFour.Model
{
    public class GameWonEventArgs : EventArgs
    {
        public Player Player { get; private set; }
        public int Direction { get; private set; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public GameWonEventArgs(Player player, int direction, (int,int) startCoord)
        {
            Player = player;
            Direction = direction;
            (StartX, StartY) = startCoord;
        }
    }
}
