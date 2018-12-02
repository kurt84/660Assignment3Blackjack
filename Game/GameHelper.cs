using System;
using System.Collections.Generic;
using System.Text;
using static GameHandler.Events;

namespace GameHandler
{
    public class GameHelper
    {
        private Dealer d;
        private Player p;
        private int deck = 1;
        private int roll = 100;
        private String name = "";
        private Boolean sCheck;
        private Boolean sCheck2;
        private Boolean InsureCheck;
        //public bool canInsurance = false;
        //public bool canSurrender = false;
        //public bool canDouble = false;
        //public bool canSplit = false;
        private int finalCount;
        //public Boolean Win { get; private set; }
        //private Boolean CheckWinner = false;
        public GameHelper(DealerCardEvent dealer, PlayerCardEvent player)
        {
            //Win = false;
            sCheck = false;
            sCheck2 = true;
            InsureCheck = false;
            finalCount = 0;
            d = new Dealer(dealer, deck);
            p = new Player(player, roll, name);
        }
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
                p.CurrentBet = 0;
                p.ReceivePayout(p.CurrentBet);
                EndGame();
                return dealer;
            }
            if ((player.EvaluateHand(playerCards) > dealer.EvaluateHand(dealerCards) ||
                 dealer.EvaluateHand(dealerCards) > 21) &&
                 dealer.EvaluateHand(dealerCards) <= 21)
            {
                //CheckWinner = true;
                p.ReceivePayout(p.CurrentBet * 2);
                finalCount = p.ReceivePayout(p.CurrentBet * 2);
                EndGame();
                return player;
            }

            return null;
        }
        
        //---Checks to see if the player has busted------------------------------------
        public Boolean BustCheck(IPlayer player, List<Card> playerCards)
        {
            return player.EvaluateHand(playerCards) > 21;
        }

        
        // initial bet to be made to get game started
        public void InitialBet(int amount)
        {

            p.MakeBet(amount);
            // in case there is no deal button we could just call this method and run the deal method.
            Deal();
        }

        public void MakeInsuranceBet(int amount)
        {
            if (amount <= (p.CurrentBet / 2))
            {
                if (DealerBlackJack())
                {
                    finalCount = p.Insurance(amount);
                    p.Insurance(amount);
                }
                
            }
        }

        // Beginning deal method for beginning of hand. 
        // A bet will need to be made before this method can run as a bet needs to be made for the game can begin.
        public int Deal()
        {
            // shuffle
            d.Shuffle(1);
            
            // add card to player's hand, to dealer's, to player's, and to dealer's to make a two card hand for each
            p.Hit(d.Draw());
            d.Hit(d.Draw());
            p.Hit(d.Draw());
            d.Hit(d.Draw());

            // check dealers hand for insurance 
            // code for checking if insurance should be offered
            if ((int)d.DealerHand[0].Face == 1)
            {
                OfferInsurance();
                if (InsureCheck)
                {
                    if (DealerBlackJack())
                    {
                        EndGame();
                    }
                }
            }

            

            // write above code is incomplete

            // dealer will 'tell player how much they have' 
            return d.EvaluateHand(p.Hand);



        }

        private Boolean DealerBlackJack()
        {
            if (d.EvaluateHand(d.DealerHand) == 21)
            {
                finalCount = p.CurrentBet;
                return true;
            }
            else
            {
                return false; 
            }
           
        }

        public int Hit()
        {
            sCheck2 = false;
            p.Hit(d.Draw());
            if(BustCheck(d, p.CurrentHand) == true)
            {
                CalculateWinner(d, p, d.DealerHand, p.CurrentHand);
                return d.EvaluateHand(p.CurrentHand);
            }
            return d.EvaluateHand(p.CurrentHand);
        }
        // overloaded method so a card can be drawn on a second hand after split
        public int Hit(List<Card> cards)
        {

            p.Hit(d.Draw());
            
            return d.EvaluateHand(p.CurrentHand);
        }
        // standing always forces the dealer to complete his hand
        public void Stand()
        {
            sCheck2 = false;

            //if (p.Hand2 == null || p.Hand3 == null || p.Hand4 == null)
            //{
            //    d.EvaluateHand(p.Stand());
            //    EndTurn();
            //}

            //if (p.Hand2 != null && p.Hand3 != null && p.Hand4 != null)
            //{
            //    d.EvaluateHand(p.Stand());
            //    EndTurn();
            //}
            EndTurn();

            
        }
        // checks to see if player is better less than or equal to original bet.
        private Boolean CheckDoubleDown(int amount)
        {
            if (amount > p.CurrentBet)
            {
                return false;
            }
            else
            {
                return true; 
            }
        }
        // if player makes correct bet then he is forced to stand 
        // otherwise he still has the same hand
        public void DoubleDown(int amount)
        {
            sCheck2 = false;
            if (CheckDoubleDown(amount))
            {
                p.DoubleDown(d.Draw(), amount);
                Stand();
            }
            else
            {
                d.EvaluateHand(p.CurrentHand);
            }
        }

        public void Split(int amount)
        {
            sCheck2 = false;
            if (amount == p.CurrentBet)
            {
                p.MakeBet(amount);
                p.Split(p.CurrentHand);
            }

            else
            {
                //bet is required
            }
             
            

            
        }

        public String OfferInsurance()
        {
            InsureCheck = true;
            return "Dealer has an Ace showing would you like insurance";
            
        }

        public void Surrender()
        {
            if (sCheck2)
            {
                sCheck = true;
                p.ReceivePayout(p.CurrentBet / 2);
                finalCount = p.ReceivePayout(p.CurrentBet / 2);
                EndGame();
            }
        }


        public void EndTurn()
        {
            DealerHit(d.DealerHand);

        }

        

        private void DealerHit(List<Card> cards)
        {
            

            while (d.EvaluateHand(d.DealerHand) < 17)
            {
                d.Hit(d.Draw());
            }
            
             
            if (BustCheck(d, d.DealerHand) == true)
            {
                    CalculateWinner(d, p, d.DealerHand, p.Hand);

                if (p.Hand2 != null || p.Hand3 != null || p.Hand4 != null)
                {
                    CalculateWinner(d, p, d.DealerHand, p.Hand2);
                    CalculateWinner(d, p, d.DealerHand, p.Hand3);
                    CalculateWinner(d, p, d.DealerHand, p.Hand4);
                }
            }
            

            else
            {
                CalculateWinner(d, p, d.DealerHand, p.Hand);

                if (p.Hand2 != null || p.Hand3 != null || p.Hand4 != null)
                {
                    CalculateWinner(d, p, d.DealerHand, p.Hand2);
                    CalculateWinner(d, p, d.DealerHand, p.Hand3);
                    CalculateWinner(d, p, d.DealerHand, p.Hand4);
                }

            }

            
        }
        // For message in the GUI
        public String EndGame()
        {
            
            

            if (sCheck)
            {
                return "You lose due to Surrender " + finalCount + " returned";
            }
            if (finalCount > 0)
            {
                return "You Won " + finalCount;
            }

            if(DealerBlackJack())
            {
                // this is hard coded likely won't work as player should be able to choose amount
                
                return "You kept your bet " + finalCount + " returned.";

            }
            else
            {
                return "You lose";
            }
            

        }

    }
}
