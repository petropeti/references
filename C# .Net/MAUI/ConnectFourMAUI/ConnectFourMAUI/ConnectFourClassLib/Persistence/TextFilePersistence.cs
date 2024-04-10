using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConnectFourClassLib.Persistence
{
    public class TextFilePersistence : IPersistence
    {
        public async Task<(Player[], Int32, Int32)> LoadAsync(String path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String[] numbers = (await reader.ReadLineAsync() ?? String.Empty).Split();
                    if (numbers.Count(num => (Convert.ToInt32(num) != 0 && Convert.ToInt32(num) != 1 && Convert.ToInt32(num) != 2)) != 0)
                        throw new DataException("Error occurred during reading: Unknown player included.");

                    Player[] values = new Player[numbers.Length];
                    for (Int32 i = 0; i < values.Length; i++)
                        values[i] = (Player)Int32.Parse(numbers[i]);
                    Int32 tx = Convert.ToInt32(reader.ReadLine() ?? String.Empty);
                    Int32 to = Convert.ToInt32(reader.ReadLine() ?? String.Empty);

                    return (values, tx, to);
                }
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }
        }

        public async Task SaveAsync(String path, Player[] values, Int32 tx, Int32 to)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (Int32 i = 0; i < values.Length - 1; i++)
                    {
                        await writer.WriteAsync(((Int32)values[i] + " ").ToString());
                    }
                    await writer.WriteLineAsync(((Int32)values[values.Length - 1]).ToString());

                    await writer.WriteLineAsync(tx.ToString());
                    await writer.WriteAsync(to.ToString());

                }
            }
            catch
            {
                throw new DataException("Error occurred during reading");
            }
        }
    }

}
