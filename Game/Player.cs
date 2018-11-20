﻿using System;
using System.Collections.Generic;
using System.Text;
using static GameHandler.CardType;

namespace GameHandler
{
    public class Player : IPlayer
    {
        public List<Card> Hand { get; private set; }
        public int Bank { get; private set; }
        public string Name { get; private set; }
        public Player(int bank, string name)
        {
            this.Bank = bank;
            this.Name = name;
            this.Hand = new List<Card>();
        }
        //Subtracts amount from total, returns new total
        public int MakeBet(int amount)
        {
            //Worry about error handling later - should Player even do this?
            if (amount > Bank)
                ;//throw new Exception("Insufficient funds");
            Bank -= amount;
            return Bank;
        }
        //Adds payout to total, returns new total
        public int ReceivePayout(int amount)
        {
            Bank += amount;
            return Bank;
        }
        //Adds card to hand, returns new total
        public List<Card> Hit(Card card)
        {
            Hand.Add(card);
            return Hand;
        }
        public List<Card> Stand()
        {
            return Hand;
        }

        public int EvaluateHand(List<Card> cards)
        {
            int total = 0;
            bool ace = false;
            foreach (Card card in Hand)
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
            return total;
        }
    }
}
