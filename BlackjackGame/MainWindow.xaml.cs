using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        private int numDecks = 5;
        private Dealer dealer;
        public MainWindow()
        {
            InitializeComponent();
            Reset();
            var win = new Welcome();
            win.Show();
            var p = DummyDb.Load("Kurt");
        }

        private void hitButton_Click(object sender, RoutedEventArgs e)
        {
            RenderItem.Card(dealer.Draw(),playerGrid);
        }

        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset() {
            dealer = new Dealer(numDecks);
            dealer.Shuffle();
            RenderItem.InitGrid(playerGrid);
            RenderItem.InitGrid(dealerGrid);
        }
    }
}
