using System;
using System.Collections.Generic;
using System.Text;

namespace TakiServer
{
    class Pile
    {
        private Card topCard;

        public Pile(Card c)
        {
            topCard = c;
        }

        public void SetTopCard(Card c)
        { 
            topCard = c;
        }

        public Card GetTopCard()
        {
            return topCard;
        }
    }
}
