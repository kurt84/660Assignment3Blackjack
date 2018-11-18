using GameHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public static class DummyDb
    {
        public static void Save(Player p)
        {
            return;
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
