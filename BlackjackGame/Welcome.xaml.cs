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
using System.Windows.Shapes;

namespace BlackjackGame
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        private Dictionary<string, int> p;
        public Welcome()
        {
            InitializeComponent();
            p = File.Load();
            if(p.Count > 0)
                playerSelectCombo.ItemsSource = p.Keys.ToList();
            else
            {
                playerSelectCombo.Visibility = Visibility.Hidden;
                chooseExistingPlayer.Visibility = Visibility.Hidden;
                chooseLabel.Visibility = Visibility.Hidden;
                orLabel.Visibility = Visibility.Hidden;
            }

        }

        private void chooseNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (newPlayerEntry.Text.Length != 0 && !p.ContainsKey(newPlayerEntry.Text))
            {
                string name = newPlayerEntry.Text;
                p.Add(name, 100);
                MainWindow.selectedPlayer = name;
                File.Save(p);
                this.Close();
            }
        }

        private void chooseExistingPlayer_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.selectedPlayer = playerSelectCombo.SelectedValue as string;
            this.Close();
        }
    }
}
