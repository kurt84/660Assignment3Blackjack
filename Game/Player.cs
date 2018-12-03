using System;
using System.Collections.Generic;
using System.Text;
using static GameHandler.CardType;
using static GameHandler.Events;

namespace GameHandler
{
    public class Player : IPlayer
    {
        public int i;
        public List<Card> Hand { get; private set; }
        public List<Card> Hand2 { get; private set; }
        public List<Card> Hand3 { get; private set; }
        public List<Card> Hand4 { get; private set; }
        public List<Card> CurrentHand { get; private set; }
        public int CurrentBet { get; set; }
        public int Bank { get;  set; }
        public Boolean CheckTurn { get; private set; }
        public string Name { get; private set; }
        private PlayerCardEvent playerNotify;
        public Player(PlayerCardEvent playerEvent, int bank, string name)
        {
            playerNotify = playerEvent;
            i = 1; 
            this.Bank = bank;
            this.Name = name;
            this.Hand = new List<Card>();
            this.Hand2 = new List<Card>();
            this.Hand3 = new List<Card>();
            this.Hand4 = new List<Card>();
            this.CurrentHand = new List<Card> ();
            this.CurrentHand = this.Hand;
            this.CheckTurn = false;
            CurrentBet = 0;
        }
        //Subtracts amount from total, returns new total
        public int MakeBet(int amount)
        {
             
            //Worry about error handling later - should Player even do this?
            if (amount > Bank)
                ;//throw new Exception("Insufficient funds");
            CurrentBet += amount;
            Bank -= amount;
            return Bank;
        }
        //Adds payout to total, returns new total
        public int ReceivePayout(int amount)
        {
            Bank += amount;
            return Bank;
        }


        public int CurrentBetPay(int amount)
        {
            return amount;
        }
        //Adds card to hand, returns new total
        public List<Card> Hit(Card card)
        {
            CurrentHand.Add(card);
            if(Hand4 != null)
            {
                Hand4.Add(card);
            }
            playerNotify(card, i);
            return CurrentHand;
        }
        public List<Card> Stand()
        {
            // assuming default is null!!!!
            if(Hand4 != null)
            {
                i = 1; 
                if (Hand4.Count >= 2)
                {
                    CurrentHand = Hand4;
                    return CurrentHand;
                }
            }

            
            return CurrentHand;
        }

        public List<Card> DoubleDown(Card card, int amount)
        {
            MakeBet(amount);
            CurrentHand.Add(card);
            playerNotify(card, i);
            return CurrentHand; 
        }

        public int Surrender(int amount)
        {
            return ReceivePayout(amount); 
        }

        public void Split(List<Card> cards)
        {
            //very cludgy probably won't work
            if (i == 1)
            {
                // to check if cards are the same and to handle splitting tens
                if (cards[1].Face == cards[0].Face || ((int)cards[1].Face >= 10 && (int)cards[0].Face >= 10))
                {
                    Hand2.Add(Hand[1]);
                    Hand.RemoveAt(1);
                    CurrentHand = Hand; 
                    i++;
                }
            }

            if (i == 2)
            {
                // to check if cards are the same and to handle splitting tens
                if (cards[1].Face == cards[0].Face || ((int)cards[1].Face >= 10 && (int)cards[0].Face >= 10))
                {
                    Hand3.Add(Hand[1]);
                    Hand2.RemoveAt(1);
                    CurrentHand = Hand2;
                    i++;
                }
            }

            if (i == 3)
            {
                // to check if cards are the same and to handle splitting tens
                if (cards[1].Face == cards[0].Face || ((int)cards[1].Face >= 10 && (int)cards[0].Face >= 10))
                {
                    Hand4.Add(Hand[1]);
                    Hand3.RemoveAt(1);
                    CurrentHand = Hand3;
                    i++;
                }
            }
            
        }

        public int Insurance (int amount)
        {
            MakeBet(amount);
            int pay = amount * 2;
            return CurrentBetPay(amount);
        }

        private Boolean EndTurn()
        {
            CheckTurn = true;
            return CheckTurn;
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
