using System;
using System.Collections.Generic;
using System.Text;

namespace GameHandler
{
    public class Events
    {
        public delegate Card DealerCardEvent(Card card, bool hidden);
        public delegate Card PlayerCardEvent(Card card, int hands);
    }
}
