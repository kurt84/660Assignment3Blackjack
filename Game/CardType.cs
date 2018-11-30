using System;
using System.Collections.Generic;
using System.Text;

namespace GameHandler
{
    public static class CardType
    {
        public enum Suit { Spades, Clubs, Hearts, Diamonds };
        public enum Face { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    }
}
