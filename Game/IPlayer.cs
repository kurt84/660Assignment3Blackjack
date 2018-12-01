using System;
using System.Collections.Generic;
using System.Text;

namespace GameHandler
{
    public interface IPlayer
    {
        int EvaluateHand(List<Card> cards);
    }
}
