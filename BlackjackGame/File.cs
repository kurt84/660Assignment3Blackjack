using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackGame
{
    public static class File
    {
        public static void Save(Dictionary<string,int> players)
        {
            var j = JsonConvert.SerializeObject(players, Formatting.Indented);
            System.IO.File.WriteAllText(@"./players.txt", j);
        }
        public static Dictionary<string,int> Load()
        {
            try
            {
                var j = System.IO.File.ReadAllText(@"./players.txt");
                Dictionary<string, int> temp = JsonConvert.DeserializeObject<Dictionary<string, int>>(j);
                return temp;
            } catch(Exception e)
            {
                return new Dictionary<string, int>();
            }

        }
        public delegate string PlayerCardEvent(string player);

    }
}
