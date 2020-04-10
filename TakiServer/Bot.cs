using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TakiServer
{
    class Bot
    {
        private Player player;

        public void HandleMessages(string message) 
        {
            string[] serverMessages = message.Split('*');


            for (int i = 0; i < serverMessages.Length; i++)
            {
                if (serverMessages[i] != "")
                {
                    // each message has elements that are seperated by "_".
                    string[] messageElements = serverMessages[i].Split('_');

                    switch (messageElements[0])
                    {
                        case "Start":
                            {
                                player.GetGame().HandleMessages(player, "Start");
                                break;
                            }
                        case "TwoPlusMode":
                            {
                                Thread.Sleep(2000);
                                TwoPlusHandler();
                                break;
                            }
                        case "Turn":
                            {
                                Thread.Sleep(2000);
                                Turn();
                                break;
                            }
                        case "ChooseColor":
                            {
                                Thread.Sleep(2000);
                                ChooseColorHandler();
                                break;
                            }
                        case "Win":
                            {
                                if (messageElements[1] == player.GetName())
                                    SendToGame("NewGame");
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        public void ChooseColorHandler()
        {
            Card[] cards = player.GetCards();
            int[] colorArray = new int[4];
            for (int i=0; i< cards.Length; i++)
            {
                if (cards[i].GetColor() == "red")
                {
                    colorArray[0]++;
                }
                if (cards[i].GetColor() == "green")
                {
                    colorArray[1]++;
                }
                if (cards[i].GetColor() == "yellow")
                {
                    colorArray[2]++;
                }
                if (cards[i].GetColor() == "blue")
                {
                    colorArray[3]++;
                }
            }

            int maxLocation = 0;
            int maxColor = colorArray[0];
            for (int i=0; i< colorArray.Length; i++)
            {
                if (colorArray[i] > maxColor)
                {
                    maxLocation = i;
                    maxColor = colorArray[0];
                }
            }

            if (maxLocation == 0)
            {
                SendToGame("ChangeColorSelect_red");
            }
            if (maxLocation == 1)
            {
                SendToGame("ChangeColorSelect_green");
            }
            if (maxLocation == 2)
            {
                SendToGame("ChangeColorSelect_yellow");
            }
            if (maxLocation == 3)
            {
                SendToGame("ChangeColorSelect_blue");
            }
        }

        public void TwoPlusHandler()
        {
            Card[] cards = player.GetCards();
            for (int i=0; i<cards.Length; i++)
            {
                if (cards[i].GetValue() == Card.cardValue.Two)
                {
                    SendToGame("Check_" + cards[i].GetValue().ToString() + "_" + cards[i].GetColor());
                    return;
                }
            }
            SendToGame("GetCard");
        }

        public void SetPlayer(Player player)
        {
            this.player = player; 
        }

        public Card WhosValuer(Card cardToPut, Card cardToCheck)
        {
            int valueOfCardToPut = GetValue(cardToPut);
            int valueOfCardToCheck = GetValue(cardToCheck);

            if (valueOfCardToPut >= valueOfCardToCheck)
            {
                return cardToPut;
            }
            else
            {
                return cardToCheck;
            }
        }

        public int GetValue(Card card)
        {
            int value;
            if (card.GetValue() == Card.cardValue.Taki)
            {
                value = 8;
            }

            else if (card.GetValue() == Card.cardValue.Two)
            {
                value = 7;
            }

            else if (card.GetValue() == Card.cardValue.Plus)
            {
                value = 6;
            }

            else if (card.GetValue() == Card.cardValue.Stop)
            {
                value = 5;
            }

            else if (card.GetValue() == Card.cardValue.ChangeDirection)
            {
                value = 4;
            }

            else if (card.GetValue() == Card.cardValue.CrazyCard)
            {
                value = 1;
            }

            else if (card.GetValue() == Card.cardValue.ChangeColor)
            {
                value = 2;
            }

            else
            {
                value = 3;
            }

            return value;
        }

        public void Turn()
        {
            Card topCard = player.GetGame().GetTopCard();
            Card[] cards = player.GetCards();

            bool foundCardToPut = false;
            Card bestCardToPut = null;

            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].GetColor() == topCard.GetColor() || cards[i].GetValue() == topCard.GetValue() || cards[i].GetColor() == "colorful" || topCard.GetValue() == Card.cardValue.CrazyCard|| cards[i].GetColor() == "gray")
                {
                    if (player.GetGame().GetTakiMode() && cards[i].GetColor() != topCard.GetColor())
                    {
                        foundCardToPut = false;
                    }
                    else if (topCard.GetValue() == Card.cardValue.CrazyCard && cards[i].GetColor() != topCard.GetColor())
                    {
                        foundCardToPut = false;
                    }
                    else if (bestCardToPut == null)
                    {
                        foundCardToPut = true;
                        bestCardToPut = cards[i];
                    }
                    else
                    {
                        foundCardToPut = true;
                        bestCardToPut = WhosValuer(bestCardToPut, cards[i]);
                    }
                }
            }

            if (foundCardToPut)
            {
                SendToGame("Check_" + bestCardToPut.GetValue().ToString() + "_" + bestCardToPut.GetColor());
            }
            else
            {
                SendToGame("GetCard");
            }
        }

        public void SendToGame(string message)
        {
            player.GetGame().HandleMessages(player, message);
        }
    }
}
