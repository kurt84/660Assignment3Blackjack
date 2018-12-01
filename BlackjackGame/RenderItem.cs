﻿using GameHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlackjackGame
{
    public static class RenderItem
    {
        public static int CardHeight = 120;
        public static int CardWidth = 80;
        public static void Card(Card card, Grid grid, string res = null)
        {
            string resource = res;
            if(res == null)
                resource = "Resources/CardImages/" + ((int)card.Face).ToString() + "_of_" + card.Suit.ToString() + ".png";
            BitmapImage temp = new BitmapImage(new Uri(
            string.Format(
            "pack://application:,,,/{0};component/{1}"
            , Assembly.GetExecutingAssembly().GetName().Name
            , resource
        )
                ));
            Image image = new Image {
                Source = temp,
                Height = CardHeight
            };

            grid.Children.Add(image);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CardWidth/4) });
            Grid.SetColumn(image, (grid.ColumnDefinitions.Count - 4));

            //hard coding to 4 for now, later can make this value dynamic by comparing grid width to card width vs number of children if we want to use the space better
            Grid.SetColumnSpan(image, 4);

            //set image to lowest row (a row will be added when doubling down)
            Grid.SetRow(image, grid.RowDefinitions.Count - 1);
        }
        public static void RevealHiddenCard(Grid grid)
        {
            //remove card back image that is covering the hidden card
            //can iterate and compare children using linq function
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
        public static void GameOver()
        {
            //draw winner message on the center of the table
        }
    }
}