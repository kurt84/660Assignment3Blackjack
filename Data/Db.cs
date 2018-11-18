using GameHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public static class Db
    {
        public static void Save(Player p)
        {
            var player = new PlayerModel(p);
            using (GameModel context = new GameModel())
            {
                context.Players.Add(player);
                context.SaveChanges();
            }
        }
        public static Player Load(string name)
        {
            using (GameModel context = new GameModel())
            {
                //return player with name if it exists, otherwise make a new player
                return (context.Players.FirstOrDefault(x => x.Name == name) ?? (new PlayerModel(100, name))).AsPlayer();
            }

            
        }
    }
}
