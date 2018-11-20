using System;
using System.Collections.Generic;
using System.Text;

namespace GameHandler
{
    interface IPlayer
    {
        int EvaluateHand(List<Card> cards);
    }
}
