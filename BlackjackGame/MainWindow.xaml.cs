using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data;
using GameHandler;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        private int numDecks = 5;
        private Dealer dealer;
        private GameHelper gameHelper;
        public MainWindow()
        {
            InitializeComponent();
            Reset();
            var win = new Welcome();
            win.Show();
            var p = DummyDb.Load("Kurt");
        }

        private void Hit_Button(object sender, RoutedEventArgs e)
        {
            RenderItem.Card(dealer.Draw(),playerGrid);
        }
        private void Stand_Button(object sender, RoutedEventArgs e)
        {
            //RenderItem.Card(dealer.Draw(), dealerGrid);
            HandleDealer();
        }
        private void Surrender_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Surrender();
            HandleDealer();
        }
        private void Bet_Button(object sender, RoutedEventArgs e)
        {
            int amount = 5; //this will be read from a text field later
            //gameHelper.MakeInitialBet(amount);
        }
        private void Double_Button(object sender, RoutedEventArgs e)
        {
            
        }
        private void Split_Button(object sender, RoutedEventArgs e)
        {
            
        }
        private void Insurance_Button(object sender, RoutedEventArgs e)
        {
            
        }

        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        private void HandleDealer()
        {

            //ADD CODE TO REVEAL DEALERS CARD
            //RenderItem.HiddenCard()
            //while (!gameHelper.DealerStands)
            //{
            //    Card card = gameHelper.DealerHit();
            //    if (card != null)
            //        RenderItem.Card(card, dealerGrid);
            //}
            //FOR TESTING ONLY - REAL CODE IS ABOVE
            for (int i = 0; i < 2; i++)
            {
                RenderItem.Card(dealer.Draw(), dealerGrid);
            }
            //EndHand();

        }
        private void EndHand()
        {
            //RenderItem.GameOver(gameHelper.GameOverMessage);
            //Wait for a few seconds
            NewHand();
        }
        private void NewHand()
        {
            RenderItem.InitGrid(playerGrid);
            RenderItem.InitGrid(dealerGrid);
            //gameHelper.Bet(value);   \  can we combine these functions?
            //gameHelper.NewHand();    /
            //Calls to render item for player cards
            //Calls to render item for dealers card and hidden card
        }
        public void Reset() {
            dealer = new Dealer(numDecks);
            dealer.Shuffle();
            gameHelper = new GameHelper();
            NewHand();
        }
    }
}
