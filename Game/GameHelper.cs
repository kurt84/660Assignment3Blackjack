﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameHandler
{
    public class GameHelper
    {
        //---Evaluates the scores to determine the winner-------------------------------
        public IPlayer CalculateWinner(Dealer dealer, 
                                       Player player,
                                       List<Card> dealerCards,
                                       List<Card> playerCards)
        {
            if ((dealer.EvaluateHand(dealerCards) > player.EvaluateHand(playerCards) || 
                 player.EvaluateHand(playerCards) > 21) &&
                 dealer.EvaluateHand(dealerCards) <= 21)
            {
                return dealer;
            }
            if ((player.EvaluateHand(playerCards) > dealer.EvaluateHand(dealerCards) ||
                 dealer.EvaluateHand(dealerCards) > 21) &&
                 dealer.EvaluateHand(dealerCards) <= 21)
            {
                return player;
            }

            return null;
        }

        //---Checks to see if the player has busted------------------------------------
        public Boolean BustCheck(Player player, List<Card> playerCards)
        {
            return player.EvaluateHand(playerCards) > 21;
        }

        public Card Hit()
        {
            return new Card(CardType.Suit.Clubs, CardType.Face.Ace);
        }

        public void Stand()
        {

        }

        public int DoubleDown()
        {
            return 1;
        }

        public void Surrender()
        {

        }

        public void Split()
        {

        }

        public void Insurance()
        {

        }

        public void Bet()
        {

        }


    }
}