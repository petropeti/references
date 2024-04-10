using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourClassLib.Persistence
{
    public interface IPersistence
    {
        Task<(Player[],Int32,Int32)> LoadAsync(String path);

        Task SaveAsync(String path, Player[] values, Int32 tx, Int32 to);
    }
}
