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
using System.Windows.Threading;

namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        public static string selectedPlayer;
        private bool canDoubleDown = false;
        private bool hideFirstTurnFunctions = false;
        private bool betMade = false;
        private Dictionary<string, int> players;
        private GameHelper gameHelper;
        public MainWindow()
        {
            InitializeComponent();
            var win = new Welcome();
            win.ShowDialog();
            players = File.Load();
            Reset();

        }

        private void Hit_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.Hit();
            canDoubleDown = false;
            hideFirstTurnFunctions = true;
            if (gameHelper.GameOver)
                EndPlayerTurn();
            SetButtons();
        }
        private void Stand_Button(object sender, RoutedEventArgs e)
        {
            RenderItem.RevealHiddenCard(dealerGrid);
            gameHelper.Stand();
            hideFirstTurnFunctions = true;
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
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.25) };
                int amount = Int32.Parse(betAmount.Text);
                if (!gameHelper.InitialBet(amount))
                {
                    DisplayMessage("You do not have enough money");
                    return;
                }
                betMade = true;
                betAmount.Text = "";
                currentBetAmount.Content = "Bet: " + amount;
                canDoubleDown = true;
                hideFirstTurnFunctions = false;
                ToggleBetDouble();

                timer.Start();
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    EndPlayerTurn();
                };
                UpdateBank();
                if (gameHelper.GameOver)
                {
                    EndPlayerTurn();
                    hideFirstTurnFunctions = true;
                }
                    
                SetButtons();
            }
            else
                DisplayMessage("You must enter an amount.");
            EndPlayerTurn();
        }
        private void Double_Button(object sender, RoutedEventArgs e)
        {
            if (betAmount.Text.Length != 0)
            {
                int amount = Int32.Parse(betAmount.Text);
                if (!gameHelper.DoubleDown(amount))
                {
                    DisplayMessage("You do not have enough money");
                    return;
                }
                betAmount.Text = "";
                currentBetAmount.Content += " + " + amount;
                canDoubleDown = false;
                ToggleBetDouble();
                hideFirstTurnFunctions = true;
                betGrid.Visibility = Visibility.Hidden;
                UpdateBank();
                SetButtons();
                EndPlayerTurn();
            }
            else
                DisplayMessage("You must enter an amount.");
        }

        private void Split_Button(object sender, RoutedEventArgs e)
        {
            hideFirstTurnFunctions = false;
            gameHelper.Split(0);
            SetButtons();
        }
        private void Insurance_Button(object sender, RoutedEventArgs e)
        {
            gameHelper.MakeInsuranceBet();
            gameHelper.canInsurance = false;
            SetButtons();
        }
        private void EndPlayerTurn()
        {
            if (gameHelper.GameOver)
            {
                betMade = false;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                RenderItem.RevealHiddenCard(dealerGrid);
                dealerText.Text = gameHelper.EndGame();
                File.Save(players);
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    if (gameHelper.GameOver)
                        NewHand();
                };
                if (gameHelper.GetBank() <= 0)
                {
                    System.Windows.MessageBox.Show("You must borrow from the bank to continue.", "Out Of Money", System.Windows.MessageBoxButton.OK);
                    gameHelper.Borrow();
                    UpdateBank();
                }
            }
        }
        //Prepares to play a new hand
        private void NewHand()
        {
            betMade = false;
            RenderItem.InitGrid(playerGrid);
            RenderItem.InitGrid(dealerGrid);
            canDoubleDown = false;
            betGrid.Visibility = Visibility.Visible;

            ToggleBetDouble();
            UpdateBank();
            currentBetAmount.Content = "";
            gameHelper.OnNewHand();
            File.Save(players);
            SetButtons();
        }
        public void Reset() {
            gameHelper = new GameHelper(
                new DealerCardEvent((Card card, bool hidden) => { RenderItem.Card(card, dealerGrid, hidden); return card; }),
                new PlayerCardEvent((Card card, int hands) => { RenderItem.Card(card, playerGrid); return card; }),
                players[selectedPlayer]
            );
            NewHand();
            SetButtons();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
        public void SetButtons()
        {
            if (gameHelper.canInsurance && betMade)
            {
                dealerText.Text = gameHelper.OfferInsurance();
            }
            dealerText.Text = gameHelper.EndGame();

            if (hideFirstTurnFunctions || !betMade)
            {
                surrenderButton.Visibility = Visibility.Hidden;
            }
            else
            {
                surrenderButton.Visibility = Visibility.Visible;
            }
            if (betMade)
            {
                hitButton.Visibility = Visibility.Visible;
                standButton.Visibility = Visibility.Visible;
            }
            else
            {
                hitButton.Visibility = Visibility.Hidden;
                standButton.Visibility = Visibility.Hidden;
            }
        }
        public void DisplayMessage(string message)
        {
            dealerText.Text = message;
        }

        private void TableClicked(object sender, MouseButtonEventArgs e)
        {
            if (gameHelper.GameOver)
                NewHand();
        }
        private void UpdateBank()
        {
            int amount = gameHelper.GetBank();
            players[selectedPlayer] = amount;
            currentBank.Content = selectedPlayer + ": " + amount;
        }
    }
}
