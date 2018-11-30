using GameHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public static class DummyDb
    {
        private static Dictionary<string, int> Players = new Dictionary<string, int>{
            { "Kurt" , 500 },
            { "Clayton", 500 },
            { "Noah", 500},
            { "James", 500 }
        };
        public static void Save(Player p)
        {
            if(!Players.Any(x => x.Key == p.Name))
                Players.Add(p.Name, 100);
        }
        public static Player Load(string name)
        {

            List<string> names = new List<string>{ "Kurt", "Clayton", "James", "Noah" };
            //using (GameModel context = new GameModel())
            //{
            //    //return player with name if it exists, otherwise make a new player
            //    return (context.Players.FirstOrDefault(x => x.Name == name) ?? (new PlayerModel(100, name))).AsPlayer();
            //}

            return (new PlayerModel(names.Any(x => x == name)? 1000 : 100, name)).AsPlayer();
        }
    }
}
