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
        private bool canDoubleDown = false;
        private GameHelper gameHelper;
        private const int BETPLACEHOLDER = 5;
        public MainWindow()
        {
            InitializeComponent();
            Reset();
            var win = new Welcome();
            win.Show();
        }

        private void Hit_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Hit();
            if (gameHelper.GameOver)
                EndPlayerTurn();
            SetButtons();
        }
        private void Stand_Button(object sender, RoutedEventArgs e)
        {
            RenderItem.RevealHiddenCard(dealerGrid);
            gameHelper.Stand();
            EndPlayerTurn();
            SetButtons();
        }
        private void Surrender_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Surrender();
            EndPlayerTurn();
            SetButtons();
        }
        private void Bet_Button(object sender, RoutedEventArgs e)
        {
            if (betAmount.Text.Length != 0)
            {
                int amount = Int32.Parse(betAmount.Text);
                betAmount.Text = "";
                canDoubleDown = true;
                ToggleBetDouble();
                gameHelper.InitialBet(amount);
                currentBetAmount.Content = betAmount.Text;
                currentBank.Content = "Bank: " + gameHelper.GetBank();
                SetButtons();
            }
            else
                DisplayMessage("You must enter an amount.");
        }
        private void Double_Button(object sender, RoutedEventArgs e)
        {
            if (betAmount.Text.Length != 0)
            {
                int amount = Int32.Parse(betAmount.Text);
                betAmount.Text = "";
                gameHelper.DoubleDown(amount);
                canDoubleDown = false;
                ToggleBetDouble();
                betGrid.Visibility = Visibility.Hidden;
                SetButtons();
                EndPlayerTurn();
            }
            else
                DisplayMessage("You must enter an amount.");
        }
        private void Split_Button(object sender, RoutedEventArgs e)
        {
            int amount = BETPLACEHOLDER;
            gameHelper.Split(amount);
            SetButtons();
        }
        private void Insurance_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.MakeInsuranceBet();
            gameHelper.canInsurance = false;
            SetButtons();
        }

        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            //Reset();
        }
        private void EndPlayerTurn()
        {
            RenderItem.RevealHiddenCard(dealerGrid);
            if (gameHelper.GameOver)
            {
                dealerText.Text = gameHelper.EndGame();
            }
            //Wait for a few seconds
            NewHand();
        }
        //Prepares to play a new hand
        private void NewHand()
        {
            ToggleBetDouble();
            doubleButton.Visibility = Visibility.Hidden;
            RenderItem.InitGrid(playerGrid);
            RenderItem.InitGrid(dealerGrid);
            gameHelper.canInsurance = false;
            gameHelper.canSurrender = true;
            gameHelper.canDouble = true;
            gameHelper.canSplit = false;
            canDoubleDown = false;
            betGrid.Visibility = Visibility.Visible;

            currentBank.Content = "Bank: " + gameHelper.GetBank();
            currentBetAmount.Content = "";
            SetButtons();
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
            SetButtons();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void DoubleDownBet(object sender, RoutedEventArgs e)
        {

        }
        public void ToggleBetDouble()
        {
            if (!canDoubleDown)
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
        public void HideExtraFunctions()
        {
            if (!canDoubleDown)
            {
                doubleButton.Visibility = Visibility.Hidden;
                splitButton.Visibility = Visibility.Hidden;
            }
            surrenderButton.Visibility = Visibility.Hidden;
            insuranceButton.Visibility = Visibility.Hidden;
        }
        public void ShowExtraFunctions()
        {
            doubleButton.Visibility = Visibility.Visible;
            surrenderButton.Visibility = Visibility.Visible;
            splitButton.Visibility = Visibility.Visible;
            insuranceButton.Visibility = Visibility.Visible;
        }
        public void SetButtons()
        {
            if (gameHelper.canInsurance)
            {
                dealerText.Text = gameHelper.OfferInsurance();
            }
            dealerText.Text = gameHelper.EndGame();
            //if (gameHelper.canDouble)
            //    doubleButton.Visibility = Visibility.Visible;
            //else
            //    doubleButton.Visibility = Visibility.Hidden;

            if (gameHelper.canSurrender)
                surrenderButton.Visibility = Visibility.Visible;
            else
                surrenderButton.Visibility = Visibility.Hidden;

            if (gameHelper.canSplit)
                splitButton.Visibility = Visibility.Visible;
            else
                splitButton.Visibility = Visibility.Hidden;

            if (gameHelper.canInsurance)
                insuranceButton.Visibility = Visibility.Visible;
            else
                insuranceButton.Visibility = Visibility.Hidden;
        }
        public void DisplayMessage(string message)
        {
            dealerText.Text = message;
        }
    }
}
