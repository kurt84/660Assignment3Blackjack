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
using static GameHandler.Events;

namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        private GameHelper gameHelper;
        private const int BETPLACEHOLDER = 5;
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
            gameHelper.Hit();
        }
        private void Stand_Button(object sender, RoutedEventArgs e)
        {
            HandleDealer();
        }
        private void Surrender_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Surrender();
            HandleDealer();
        }
        private void Bet_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.InitialBet(amount);
        }
        private void Double_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.DoubleDown(amount);
        }
        private void Split_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.Split(amount);
        }
        private void Insurance_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.MakeInsuranceBet(amount);
        }

        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        private void HandleDealer()
        {

            //ADD CODE TO REVEAL DEALERS CARD
            RenderItem.RevealHiddenCard(dealerGrid);
            //while (!gameHelper.DealerStands)
            //{
            //    Card card = gameHelper.DealerHit();
            //    if (card != null)
            //        RenderItem.Card(card, dealerGrid);
            //}
            //FOR TESTING ONLY - REAL CODE IS ABOVE
            //for (int i = 0; i < 2; i++)
            //{
            //    RenderItem.Card(dealer.Draw(), dealerGrid);
            //}
            //EndHand();

        }
        private void StartHand()
        {
            gameHelper.Deal();

        }
        private void EndHand()
        {
            //RenderItem.GameOver(gameHelper.GameOverMessage);
            //Wait for a few seconds
            NewHand();
        }
        //Prepares to play a new hand
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
            gameHelper = new GameHelper(
                new DealerCardEvent((Card card, bool hidden) => { RenderItem.Card(card, dealerGrid, hidden); return card; }),
                new PlayerCardEvent((Card card, int hands) => { /*DO SOMETHING WITH HANDS VARIABLE*/RenderItem.Card(card, playerGrid); return card; })
            );
            NewHand();
        }
    }
}
