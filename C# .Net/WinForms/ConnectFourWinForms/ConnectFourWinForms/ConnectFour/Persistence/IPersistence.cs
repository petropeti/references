using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public interface IPersistence
    {
        (Player[],Int32,Int32) Load(String path);

        void Save(String path, Player[] values, Int32 tx, Int32 to);
    }
}
