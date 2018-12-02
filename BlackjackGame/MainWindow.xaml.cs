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
using GameHandler;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using static GameHandler.Events;
using System.Text.RegularExpressions;

namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        private bool doublingDown = false;
        private GameHelper gameHelper;
        private const int BETPLACEHOLDER = 5;
        public MainWindow()
        {
            InitializeComponent();
            Reset();
            var win = new Welcome();
            win.Show();
            //ar p = DummyDb.Load("Kurt");
        }

        private void Hit_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Hit();
            //SetButtons();
        }
        private void Stand_Button(object sender, RoutedEventArgs e)
        {
            RenderItem.RevealHiddenCard(dealerGrid);
            gameHelper.Stand();
            //SetButtons();
        }
        private void Surrender_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Surrender();
            HandleDealer();
            //SetButtons();
        }
        private void Bet_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.InitialBet(amount);
            //SetButtons();
        }
        private void Double_Button(object sender, RoutedEventArgs e)
        {
            doublingDown = true;
            betGrid.Visibility = Visibility.Visible;
            
            int amount = BETPLACEHOLDER;
            gameHelper.DoubleDown(amount);
            //SetButtons();
        }
        private void Split_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.Split(amount);
            //SetButtons();
        }
        private void Insurance_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.MakeInsuranceBet(amount);
            //SetButtons();
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
            ToggleBetDouble();
            doubleButton.Visibility = Visibility.Visible;
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
            //SetButtons();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DealMeIn(object sender, RoutedEventArgs e)
        {
            ToggleBetDouble();
            //betGrid.Visibility = Visibility.Hidden;
            currentBetAmount.Content = betAmount.Text;
            gameHelper.Deal();
            //SetButtons();
        }
        public void ToggleBetDouble()
        {
            if (submitBet.Visibility == Visibility.Hidden)
            {
                submitBet.Visibility = Visibility.Visible;
                submitDouble.Visibility = Visibility.Hidden;
            }
            else
            {
                submitBet.Visibility = Visibility.Hidden;
                submitDouble.Visibility = Visibility.Visible;
            }
        }
        //public void HideExtraFunctions()
        //{
        //    if (!doublingDown)
        //    {
        //        doubleButton.Visibility = Visibility.Hidden;
        //        splitButton.Visibility = Visibility.Hidden;
        //    }
        //    surrenderButton.Visibility = Visibility.Hidden;
        //    insuranceButton.Visibility = Visibility.Hidden;
        //}
        //public void ShowExtraFunctions()
        //{
        //    doubleButton.Visibility = Visibility.Visible;
        //    surrenderButton.Visibility = Visibility.Visible;
        //    splitButton.Visibility = Visibility.Visible;
        //    insuranceButton.Visibility = Visibility.Visible;
        //}
        public void SetButtons()
        {
            //if (gameHelper.canDouble)
            //    doubleButton.Visibility = Visibility.Visible;
            //else
            //    doubleButton.Visibility = Visibility.Hidden;

            //if (gameHelper.canSurrender)
            //    surrenderButton.Visibility = Visibility.Visible;
            //else
            //    surrenderButton.Visibility = Visibility.Hidden;

            //if (gameHelper.canSplit)
            //    splitButton.Visibility = Visibility.Visible;
            //else
            //    splitButton.Visibility = Visibility.Hidden;

            //if (gameHelper.canInsurance)
            //    insuranceButton.Visibility = Visibility.Visible;
            //else
            //    insuranceButton.Visibility = Visibility.Hidden;
        }
    }
}
