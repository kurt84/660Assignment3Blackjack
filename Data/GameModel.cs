using GameHandler;
using System;
using System.Data.Entity;

namespace Data
{
    public class GameModel : DbContext
    {
        public GameModel() : base("name=PlayerModel")
        {

        }
        public DbSet<PlayerModel> Players { get; set; }
    }
}
