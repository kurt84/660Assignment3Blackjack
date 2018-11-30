using System;
using System.Collections.Generic;
using static GameHandler.CardType;

namespace GameHandler
{
    public class Dealer :IPlayer
    {
        private List<Card> Deck { get; set; }
        private int Score { get; set; }

        public Dealer(int numDecks = 1)
        {
            Deck = new List<Card>();
            for (int i = 0; i < numDecks; i++) {
                foreach(Suit suit in Suit.GetValues(typeof(Suit)))
                {
                    foreach (Face face in Face.GetValues(typeof(Face))) {
                        Deck.Add(new Card(suit, face));
                    }
                }
            }
        }

        public void Shuffle(int numberOfTimes = 1)
        {
            var r = new Random();
            for (int n = 0; n < numberOfTimes; n++) {
                for (int i = 0; i < Deck.Count; i++)
                {
                    int index = r.Next(Deck.Count);
                    Card temp = Deck[index];
                    Deck[index] = Deck[i];
                    Deck[i] = temp;
                }
            }
        }
        //removes a card from the deck and returns it
        public Card Draw()
        {
            Card next = Deck[0];
            Deck.RemoveAt(0);
            return next;
        }

        //this scores the list of cards passed in
        public int EvaluateHand(List<Card> cards)
        {
            int total = 0;
            bool ace = false;
            foreach (Card card in cards)
            {
                if (card.Face == Face.Ace)
                    ace = true;

                //enum values start at Ace = 1, Two = 2, and so on
                if ((int)card.Face >= 10)
                    total += 10;
                //add the integer value of the enum
                else
                    total += (int)card.Face;
            }
            //turn 
            if (ace && total <= 11)
                total += 10;
            return Score = total;
        }
    }
}
