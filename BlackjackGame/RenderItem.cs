using GameHandler;
using System;
using System.Media;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BlackjackGame
{
    public static class RenderItem
    {
        public static int CardHeight = 120;
        public static int CardWidth = 80;

        public static void Card(Card card, Grid grid, bool hidden = false)
        {
            string resource;
            if (!hidden)
                resource = "Resources/CardImages/" + ((int)card.Face).ToString() + "_of_" + card.Suit.ToString() + ".png";
            else
                resource = "Resources/CardImages/card_back.png";
            BitmapImage temp = new BitmapImage(new Uri(
                string.Format(
                    "pack://application:,,,/{0};component/{1}"
                    , Assembly.GetExecutingAssembly().GetName().Name
                    , resource
                )
            ));
            Image image = new Image
            {
                Source = temp,
                Height = CardHeight,
                Tag = hidden? null :card
            };
            if (hidden)
                Card(card, grid);

            grid.Children.Add(image);

            if (!hidden)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CardWidth / 4) });
            }
                

            Grid.SetColumn(image, (grid.ColumnDefinitions.Count - 4));

            //hard coding to 4 for now, later can make this value dynamic by comparing grid width to card width vs number of children if we want to use the space better
            Grid.SetColumnSpan(image, 4);

            //set image to lowest row (a row will be added when doubling down)
            Grid.SetRow(image, grid.RowDefinitions.Count - 1);
        }

        public static void InitGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CardWidth/4)});
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CardWidth/4)});
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CardWidth/4)});
            grid.RowDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition());
        }

        internal static void RevealHiddenCard(Grid grid)
        {
            foreach(Image item in grid.Children)
            {
                if (item.Tag == null)
                {
                    grid.Children.Remove(item);
                    break;
                }
                    
            }
        }

        public static void GameOver()
        {
            //draw winner message on the center of the table
        }
    }
}
