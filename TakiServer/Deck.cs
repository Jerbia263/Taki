using System;
using System.Collections.Generic;
using System.Text;

namespace TakiServer
{
    class Deck
    {
        private Card[] cards;
        private int numOfTop = -1;
        
        public Deck()
        {
            this.cards = new Card[55];
            int j = 0;
            for (Card.cardValue i = Card.cardValue.One; i <= Card.cardValue.Taki; i++)
            {                
                if (i == Card.cardValue.Taki)
                {
                    cards[j] = new Card("green", i);
                    cards[j+1] = new Card("blue", i);
                    cards[j+2] = new Card("red", i);
                    cards[j+3] = new Card("yellow", i);
                    cards[j+4] = new Card("colorful", i);
                    j = j + 5;
                }
                else if (i == Card.cardValue.CrazyCard)
                {
                    cards[j] = new Card("gray", i);
                    j++;
                }
                else if (i == Card.cardValue.ChangeColor)
                {
                    cards[j] = new Card("colorful", i);
                    j ++;
                }
                else
                {
                    cards[j] = new Card("green", i);
                    cards[j+1] = new Card("blue", i);
                    cards[j+2] = new Card("red", i);
                    cards[j+3] = new Card("yellow", i);
                    j = j + 4;
                }
            }
            MixCards();
        }

        public void MixCards()
        {
            Random rnd = new Random();
            for (int i=0; i<cards.Length; i++)
            {
                if (i == 0)
                {
                    int num = RandomStartingCard();
                    Card temp = cards[i];
                    cards[i] = cards[num];
                    cards[num] = temp;
                }
                else
                {
                    int num = rnd.Next(1, 49);
                    Card temp = cards[i];
                    cards[i] = cards[num];
                    cards[num] = temp;
                }
            }
        }

        public Card RemoveCard ()
        {
            numOfTop ++;
            return cards[numOfTop];
        }

        public bool isEmpty()
        {
            return (cards.Length -1  - numOfTop == 0);
        }

        public int RandomStartingCard()
        {
            Random rnd = new Random();
            while (true)
            {
                int num = rnd.Next(0, 35);
                if (num == 4 || num == 5 || num == 6 || num == 7)
                {
                    //keep randomizing
                }
                else
                {
                    return num;
                }
            }
        }
    }
}
