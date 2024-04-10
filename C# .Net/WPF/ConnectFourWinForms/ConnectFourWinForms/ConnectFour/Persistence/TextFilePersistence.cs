using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public class TextFilePersistence : IPersistence
    {
        public (Player[], Int32, Int32) Load(String path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String[] numbers = (reader.ReadLine() ?? String.Empty).Split();

                    Player[] values = new Player[numbers.Length];
                    for (Int32 i = 0; i < values.Length; i++)
                        values[i] = (Player)Int32.Parse(numbers[i]);
                    Int32 tx = Convert.ToInt32(reader.ReadLine() ?? String.Empty);
                    Int32 to = Convert.ToInt32(reader.ReadLine() ?? String.Empty);

                    return (values, tx, to);
                }
            }
            catch
            {
                throw new DataException("Error occurred during reading.");
            }
        }

        public void Save(String path, Player[] values, Int32 tx, Int32 to)
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
                        writer.Write((Int32)values[i] + " ");
                    }
                    writer.WriteLine((Int32)values[values.Length - 1]);

                    writer.WriteLine(tx);
                    writer.Write(to);

                }
            }
            catch
            {
                throw new DataException("Error occurred during writing.");
            }
        }
    }

}
