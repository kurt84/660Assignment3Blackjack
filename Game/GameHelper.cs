using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public bool canInsurance = false;
        public bool canSurrender = true;
        public bool canDouble = true;
        public bool canSplit = false;
        public bool GameOver = false;
        public bool dealBust = false;
        public bool dub = false;
        private int finalCount;
        private int shuffle;
        public bool Win = false;
        public bool bust = false;
        
        //private Boolean CheckWinner = false;
        public GameHelper(DealerCardEvent dealer, PlayerCardEvent player)
        {
            //Win = false;
            shuffle = 0;
            sCheck = false;
            sCheck2 = true;
            InsureCheck = false;
            finalCount = 0;
            d = new Dealer(dealer, deck);
            p = new Player(player, roll, name);
        }

        public void OnNewHand()
        {
            GameOver = false;
            canInsurance = false;
            canSurrender = true;
            canDouble = true;
            canSplit = false;
            if (d.CardsRemaining <= deck * .4)
                d.Reshuffle(deck);
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
                //p.ReceivePayout(p.CurrentBet);
               
                    finalCount = 0;
                
                return dealer;
            }
            if ((player.EvaluateHand(playerCards) > dealer.EvaluateHand(dealerCards) ||
                 dealer.EvaluateHand(dealerCards) > 21) &&
                 player.EvaluateHand(dealerCards) <= 21)
            {
                //CheckWinner = true;
                
                //finalCount = p.CurrentBet * 2;
                

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
        public bool InitialBet(int amount)
        {
            if (amount > p.GetBank())
                return false;
            shuffle++;
            if (shuffle == 5)
            {
                d.Shuffle(1);
                shuffle = 0;
            }
            d.DealerHand = new List<Card>();
            p.Hand = new List<Card>();
            p.CurrentHand = p.Hand;
            dub = false;
            GameOver = false;
            dealBust = false;
            Win = false;
            p.CurrentBet = 0;
            finalCount = 0;
            p.MakeBet(amount);
            //p.Bank = p.Bank - amount;
            // in case there is no deal button we could just call this method and run the deal method.
            Deal();
            return true;
        }

        public void MakeInsuranceBet()
        {
           
                if (DealerBlackJack())
                {
                    finalCount = p.Insurance(p.CurrentBet / 2);
                    p.Insurance(p.CurrentBet / 2);
                }
                
            
        }

        // Beginning deal method for beginning of hand. 
        // A bet will need to be made before this method can run as a bet needs to be made for the game can begin.
        public async void Deal()
        {
            // shuffle
            d.Shuffle(1);

            // add card to player's hand, to dealer's, to player's, and to dealer's to make a two card hand for each
            await Task.Delay(500);
            p.Hit(d.Draw());
            await Task.Delay(500);
            d.Hit(d.Draw());
            await Task.Delay(500);
            p.Hit(d.Draw());
            await Task.Delay(500);
            d.Hit(d.Draw());

            // check dealers hand for insurance 
            // code for checking if insurance should be offered
            if ((int)d.DealerHand[1].Face == 1)
            {
                canInsurance = true;
                
                //if (InsureCheck)
                //{
                //    if (DealerBlackJack())
                //    {
                //        EndGame();
                //    }
                //}
            }

            if(d.EvaluateHand(p.CurrentHand) == 21)
            {
                GameOver = true;
                Win = true;
                finalCount = (p.CurrentBet * 2) + (p.CurrentBet / 2);
                EndTurn();
                //p.ReceivePayout(finalCount);
            }
        }

        private Boolean DealerBlackJack()
        {
            if (d.EvaluateHand(d.DealerHand) == 21 && d.DealerHand.Count == 2)
            {
                finalCount = p.CurrentBet;
                GameOver = true;
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
                bust = true;
                GameOver = true;
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
            d.EvaluateHand(p.Stand());

            
        }
        // checks to see if player is better less than or equal to original bet.
        private Boolean CheckDoubleDown(int amount)
        {
            if (amount > p.CurrentBet || amount > p.GetBank())
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
        public bool DoubleDown(int amount)
        {
            sCheck2 = false;
            if (CheckDoubleDown(amount))
            {
                p.DoubleDown(d.Draw(), amount);
                dub = true;
                Stand();
                return true;
            }
            else
            {
                //d.EvaluateHand(p.CurrentHand);
                return false;
            }
        }

        public void Split(int amount)
        {
            sCheck2 = false;
            if (amount == p.CurrentBet)
            {
                //p.MakeBet(amount);
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
            GameOver = true;
            sCheck = true;
        }

        public int GetBank()
        {
            
            return p.GetBank();
        }
        

        public void EndTurn()
        {
            DealerHit(d.DealerHand);

        }

        

        private void DealerHit(List<Card> cards)
        {

            if (Win == false)
            {
                while (d.EvaluateHand(d.DealerHand) < 17)
                {
                    d.Hit(d.Draw());
                }
            }

            GameOver = true;

            if (BustCheck(d, d.DealerHand) == true)
            {
                    dealBust = true;
                    CalculateWinner(d, p, d.DealerHand, p.Hand);
                    
                    

                //if (p.Hand2 != null || p.Hand3 != null || p.Hand4 != null)
                //{
                //    CalculateWinner(d, p, d.DealerHand, p.Hand2);
                //    CalculateWinner(d, p, d.DealerHand, p.Hand3);
                //    CalculateWinner(d, p, d.DealerHand, p.Hand4);
                    
                //    EndGame();
                //}
            }

            else if (d.EvaluateHand(d.DealerHand) != d.EvaluateHand(p.CurrentHand))
                CalculateWinner(d, p, d.DealerHand, p.Hand);
            
        }



        // For message in the GUI
        public String EndGame()
        {

            if (((dub == true) && dealBust == true && GameOver == true) ||
                ((dub == true) && (d.EvaluateHand(p.CurrentHand) > d.EvaluateHand(d.DealerHand)) && (GameOver == true)))
            {
                p.ReceivePayout((p.CurrentBet * 2)/2);
                return "You Won a Double Down " + p.CurrentBet * 2;
            }

            if (sCheck)
            {
                p.ReceivePayout((p.CurrentBet/2));
                return "You lose due to Surrender " + p.CurrentBet / 2 + " returned";
            }
            if ((d.EvaluateHand(p.CurrentHand) == d.EvaluateHand(d.DealerHand)) && GameOver == true)
            {

                p.ReceivePayout(p.CurrentBet/2);
                return "Push";
            }

            if ((d.EvaluateHand(p.CurrentHand) == 21) && (GameOver == true) && (p.CurrentHand.Count == 2))
            {

                p.ReceivePayout(finalCount);
                return "BLACKJACK " + finalCount;
            }
            if ((d.EvaluateHand(p.CurrentHand) > 21) && (GameOver == true))
            {
                return "Player Bust";
            }

            if (dealBust == true && GameOver == true)
            {
                finalCount = p.CurrentBet * 2;
                p.ReceivePayout(finalCount/2);
                return "Dealer Busted You Win " + finalCount;
            }

            if ((d.EvaluateHand(p.CurrentHand) > d.EvaluateHand(d.DealerHand)) && (GameOver == true))
            {
                finalCount = p.CurrentBet * 2;
                p.ReceivePayout(finalCount/2);
                return "You Won " + finalCount;
            }

            if (DealerBlackJack() && (GameOver == true))
            {


                return "Dealer BlackJack you lose";

            }
            if ((d.EvaluateHand(d.DealerHand) > d.EvaluateHand(p.CurrentHand)) && (GameOver == true))
            {
                return "You lose";
            }

            else
            {
                return "";
            }





        }

    }
}
