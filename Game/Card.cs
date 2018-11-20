using System;
using System.Collections.Generic;
using System.Text;
using static GameHandler.CardType;

namespace GameHandler
{
    public class Card
    {
        public Suit Suit { get; private set;}
        public Face Face { get; private set;}
        
        public Card(Suit suit, Face face)
        {
            this.Suit = suit;
            this.Face = face;
        }
    }
}
