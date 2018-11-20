using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web;
using GameHandler;

namespace Data
{
    public class PlayerModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Bank { get; set; }
        public PlayerModel(int bank, string name)
        {
            Bank = bank;
            Name = name;
        }
        public PlayerModel(Player player)
        {
            this.Bank = player.Bank;
            this.Name = player.Name;
        }
        public Player AsPlayer()
        {
            return new Player(Bank, Name);
        }
    }
}
