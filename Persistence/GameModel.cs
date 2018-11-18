using GameHandler;
using System;
using System.Data.Entity;

namespace Persistence
{
    public class GameModel : DbContext
    {
        public GameModel() : base("name=PlayerModel")
        {

        }
        public DbSet<PlayerModel> Players { get; set; }
    }
}
