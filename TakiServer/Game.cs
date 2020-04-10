using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TakiServer
{
    class Game
    {
        private Player[] clientArray = new Player[0];
        private ServerManager serverManager;

        public Player manager;
        
        public Deck deck;
        public Pile pile;

        public int turn = -1;
        
        private bool TakiMode = false;
        public int TwoPlusCounter = 0;
        private int id;
        private int numOfPlayers = 0;
        private int MAX_PLAYERS = 4;
        private int INITIAL_CARD_COUNT = 8;
        private bool isStarted = false;
        private bool isFirstMatch;
        private bool direction = true;
        private int numOfBots = 0;

        public Game(ServerManager serverManager, int id, bool isFirstGame, Player manager)
        {
            if (!isFirstGame)
            {
                isStarted = true;
            }
            this.manager = manager;
            isFirstMatch = isFirstGame;
            this.serverManager = serverManager;
            this.id = id;
            deck = new Deck();
            pile = new Pile(deck.RemoveCard());
        }

       
        public void AddPlayer(Player player)
        {
            Array.Resize(ref clientArray, clientArray.Length + 1);
            clientArray[numOfPlayers] = player;

            numOfPlayers++;

            if (!CheckName(player.GetName()) && player.GetName() != null)
            {
                DisconnectPlayer(player);
                return;
            }

            serverManager.BroadCast("NumOfPlayersUpdate_" + GetId() + "_" + numOfPlayers);

            if (manager == null)
            {
                manager = player;
            }

            if (player.GetName()  == manager.GetName())
            {
                player.SendMessage("Manager");
            }

            player.SetGame(this);
            BroadCast("*Names" + Names(clientArray[0].GetName()));

            if (numOfPlayers == MAX_PLAYERS )
            {
                // begin the game
                StartGame();
            }

        }

        public int GetNumOfPlayers()
        {
            return numOfPlayers;
        }

        public void StartGame()
        {
            if (numOfPlayers == 1)
            {
                return;
            }
            SetCardsToAll();
            BroadCast("*Start");
            isStarted = true;
            MAX_PLAYERS = numOfPlayers;
            if (isFirstMatch)
            {
                serverManager.RemoveGameFromLists(id);
            }
        }

        public void SetCardsToAll()
        {
            for(int i=0; i<clientArray.Length; i++)
            {
                // get 8 cards for player
                Card[] cards = new Card[INITIAL_CARD_COUNT];
                for (int j = 0; j < INITIAL_CARD_COUNT; j++)
                {
                    Card c = deck.RemoveCard();
                    cards[j] = c;
                }
                clientArray[i].SetInitialCards(cards);
            }
        }

        public void DisconnectPlayer(Player player)
        {
            numOfPlayers--;
            bool found = false;
            for (int i=0; i<numOfPlayers; i++)
            {
                if (clientArray[i].GetName() == player.GetName())
                    found = true;
                if (!found)
                    clientArray[i] = clientArray[i];
                if (found)
                    clientArray[i] = clientArray[i + 1];
            }
            Array.Resize(ref clientArray, numOfPlayers);
            if (numOfPlayers == 0 && !isStarted)
            {
                if (isFirstMatch)
                {
                    isStarted = true;
                    serverManager.RemoveGameFromLists(id);
                }
            }
        }

        public void HandleMessages(Player player, string message)
        {
            string[] messageArray = message.Split('_');

            switch (messageArray[0]) 
            {
                case "StartGame":
                    {
                        StartGame();
                        break;
                    }
                case "Rematch":
                    {
                        serverManager.JoinCurrentGame(player, this.GetId());
                        break;
                    }
                case "Start":
                    {
                        // send the top card
                        Card topCard = pile.GetTopCard();
                        string startingCard = "*StartingCard_" + topCard.GetValue().ToString() + "_" + topCard.GetColor();
                        player.SendMessage(startingCard);

                        // send initial cards
                        player.SendInitialCards();

                        // send player names
                        player.SendMessage("*InGameNames" + Names(player.GetName()));

                        // if palayer is first in the array - let him start
                        if (clientArray[0].GetName() == player.GetName())
                        {
                            Turn(1, player);
                        }
                        else
                        {
                            player.SendMessage("*TurnUpdate_" + clientArray[0].GetName());
                        }
                            
                        break;
                    }
                case "GetCard":
                    {
                        if (TwoPlusCounter == 0)
                        {
                            Card card = deck.RemoveCard();
                            player.AddCard(card);
                            TakiMode = false;
                            Turn(1, player);
                        }
                        else
                        {
                            for (int i = 0; i < TwoPlusCounter; i++)
                            {
                                Card card = deck.RemoveCard();
                                player.AddCard(card);
                            }
                            TwoPlusCounter = 0;
                            Turn(1, player);
                        }
                        break;
                    }
                case "Check":
                    {
                        CheckAndRemove(pile.GetTopCard(), messageArray[2], messageArray[1], player);
                        break;
                    }
                case "ChangeColorSelect":
                    {
                        BroadCast("*StartingCard_ChangeColor_" + messageArray[1]);
                        pile.SetTopCard(new Card(messageArray[1], Card.cardValue.ChangeColor));
                        Turn(1, player);
                        break;
                    }
                case "GetNames":
                    {
                        player.SendMessage("*Names" + Names(player.GetName()));
                        break;
                    }
                case "GetInGameNames":
                    {
                        player.SendMessage("InGameNames" + Names(player.GetName()));
                        break;
                    }
                case "NewGame":
                    {
                        isFirstMatch = false;
                        serverManager.RematchGame(this);
                        break;
                    }
                case "EndGame":
                    {
                        DisconnectPlayer(player);
                        BroadCast("LeftTheGame");
                        player.SetGame(null);
                        break;
                    }
                case "Cancel":
                    {
                        DisconnectPlayer(player);
                        if (player.GetName() == manager.GetName())
                        {
                            DisconnectAll();
                        }
                        BroadCast("LeftTheWait");
                        player.SetGame(null);
                        break;
                    }
                case "AddBot":
                    {
                        Bot bot = new Bot();
                        Player newBot = new Player(bot);
                        bot.SetPlayer(newBot);
                        numOfBots++;
                        newBot.SetName("BOT" + numOfBots);
                        AddPlayer(newBot);
                        break;
                    }
                default:
                    break;

            }           
        }

        public void DisconnectAll()
        {
            for (int i=0; i<clientArray.Length; i++)
            {
                if (clientArray[i].GetName() != manager.GetName())
                {
                    clientArray[i].SendMessage("*CancelGame");
                }
            }
        }
        public Player GetManager()
        {
            return manager;
        }

        public string Names(string name)
        {
            string namesOfPlayers = "";
            bool found = false;
            for (int i = 0; i < clientArray.Length; i++)
            {
                if (clientArray[i].GetName() == name)
                    found = true;
                if (found)
                    namesOfPlayers = namesOfPlayers + "_" + clientArray[i].GetName();
            }
            int j = 0;
            while (clientArray[j].GetName() != name)
            {
                namesOfPlayers = namesOfPlayers + "_" + clientArray[j].GetName();
                j++;
            }
            return namesOfPlayers;
        }

        public bool GetisStarted()
        {
            return isStarted;
        }

        public int GetId()
        {
            return id;
        }

        // Check the card that player wants to put in pile, if OK - let player put it
        public void CheckAndRemove(Card topPileCard, string cardColorToPut, string cardTypeToPut, Player player)
        {
            bool cardMoveOK = false;

            Card cardToPut = new Card(cardTypeToPut, cardColorToPut);

            // if there is a +2 - you can only put +2
            if (TwoPlusCounter != 0)
            {
                if (cardToPut.GetValue() == Card.cardValue.Two)
                {
                    // this is OK
                    cardMoveOK = true;
                }
            }
            else if (cardToPut.GetColor() == "colorful")
            {
                if (cardTypeToPut == "ChangeColor")
                    // it is OK always
                    cardMoveOK = true;
                else
                {
                    pile.SetTopCard(new Card(cardTypeToPut, topPileCard.GetColor()));
                    player.RemoveCard(cardToPut);
                    BroadCast("*StartingCard_" + cardTypeToPut + "_" + topPileCard.GetColor());
                    TurnHandler(pile.GetTopCard().GetValue(), pile.GetTopCard().GetColor(), player);
                }
                    
            }
            else if (cardToPut.GetColor() == "gray")
            {
                pile.SetTopCard(new Card(cardTypeToPut, topPileCard.GetColor()));
                player.RemoveCard(cardToPut);
                BroadCast("*StartingCard_" + cardTypeToPut + "_" + cardColorToPut);
                TurnHandler(pile.GetTopCard().GetValue(), pile.GetTopCard().GetColor(), player);
            }
            // simplest case - same color or same value
            else if (topPileCard.GetColor() == cardToPut.GetColor() || topPileCard.GetValue() == cardToPut.GetValue())
            {
                if (TakiMode)
                {
                    if (topPileCard.GetColor() != cardToPut.GetColor())
                    {
                        cardMoveOK = false;
                    }
                    else
                    {
                        cardMoveOK = true;
                    }
                }
                else
                {
                    // this is OK
                    cardMoveOK = true;
                }
            }
            else
            {
                // this is not OK
                cardMoveOK = false;
            }

            if (cardMoveOK == true)
            {
                pile.SetTopCard(cardToPut);
                player.RemoveCard(cardToPut);
                BroadCast("*StartingCard_" + cardTypeToPut + "_" + cardColorToPut);
                TurnHandler(pile.GetTopCard().GetValue(), pile.GetTopCard().GetColor(), player);
            }
            else
            {
                Turn(0, player);
            }
        }

        public void TurnHandler(Card.cardValue c, string color, Player player)
        {
            if (c == Card.cardValue.Stop)
            {
                if (CheckWin(player))
                {
                    return;
                }
                if (TakiMode && FindColorInCards(color, player))
                {
                    Turn(0, player);
                }
                else
                {
                    TakiMode = false;
                    Turn(2, player);
                }
            }
            else if (c == Card.cardValue.Plus)
            {
                if (TakiMode && FindColorInCards(color, player))
                {
                    Turn(0, player);
                }
                else
                {
                    TakiMode = false;
                    Turn(0, player);
                }
            }
            else if (c == Card.cardValue.ChangeDirection)
            {
                if (CheckWin(player))
                {
                    return;
                }
                if (TakiMode && FindColorInCards(color, player))
                {
                    Turn(0, player);
                }
                else
                {
                    TakiMode = false;
                    if (direction == true)
                    {
                        direction = false; 
                    }
                    else
                    {
                        direction = true;
                    }
                    Turn(1, player);
                }
            }
            else if (c == Card.cardValue.ChangeColor)
            {
                if (CheckWin(player))
                {
                    return;
                }
                BroadCast("*NumCardsUpdate_" + player.GetName() + "_" + player.GetCards().Length);
                player.SendMessage("*ChooseColor");
            }
            else if (c == Card.cardValue.Two)
            {
                if (CheckWin(player))
                {
                    return;
                }
                if (TakiMode && FindColorInCards(color, player))
                {
                    Turn(0, player);
                }
                else
                {
                    TwoPlusCounter = TwoPlusCounter + 2;
                    TakiMode = false;
                    Turn(1, player);
                }
            }
            else if (c == Card.cardValue.CrazyCard)
            {
                if (CheckWin(player))
                {
                    return;
                }
                else
                {
                    TakiMode = false;
                    BroadCast("*CrazyCardMode");
                    CrazyCardAction(player);
                    Turn(1, player);
                }
            }
            else if (c == Card.cardValue.Taki)
            {
                if (CheckWin(player))
                {
                    return;
                }
                if (FindColorInCards(color, player))
                {
                    TakiMode = true;
                    Turn(0, player);
                }
                else
                {
                    Turn(1, player);
                }
            }
            else if (TakiMode)
            {
                if (CheckWin(player))
                {
                    return;
                }
                if (FindColorInCards(color, player))
                {
                    Turn(0, player);
                }
                else
                {
                    TakiMode = false;
                    Turn(1, player);
                }
            }
            else
            {
                if (CheckWin(player))
                {
                    return;
                }
                Turn(1, player);
            }
        }

        public bool GetTakiMode()
        {
            return TakiMode;
        }

        public void CrazyCardAction(Player player)
        {
            if (direction == true)
            {
                Card[] cards = clientArray[numOfPlayers - 1].GetCards();
                for (int i = 0; i < clientArray.Length - 1; i++)
                {
                    clientArray[i + 1].SetInitialCards(clientArray[i].GetCards());
                }
                clientArray[0].SetInitialCards(cards);
                for (int i = 0; i < clientArray.Length; i++)
                {
                    clientArray[i].SendInitialCards();
                }
            }

            if (direction == false)
            {
                Card[] cards = clientArray[0].GetCards();
                for (int i = 0; i < clientArray.Length - 1; i++)
                {
                    clientArray[i].SetInitialCards(clientArray[i + 1].GetCards());
                }
                clientArray[numOfPlayers - 1].SetInitialCards(cards);
                for (int i = 0; i < clientArray.Length; i++)
                {
                    clientArray[i].SendInitialCards();
                }
            }

            for(int i=0; i<clientArray.Length; i++)
            {
                BroadCast("*NumCardsUpdate_" + clientArray[i].GetName() + "_" + clientArray[i].GetCards().Length);
            }
        }

        public bool FindColorInCards(string color, Player player)
        {
            for (int i = 0; i < player.GetCards().Length; i++)
            {
                if (player.GetCards()[i].GetColor() == color)
                {
                    return true;
                }
            }
            return false;
        }

        public void Rematch(Player player)
        {
            AddPlayer(player);
        }

        public bool CheckName(string name)
        {
            for(int i=0; i<clientArray.Length -1; i++)
            {
                if (clientArray[i].GetName() != null)
                {
                    if (clientArray[i].GetName() == name)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllNamesOk()
        {
            for (int i = 0; i < clientArray.Length; i++)
            {
                if (clientArray[i].GetName() == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckWin( Player player )
        {
            if (player.GetCards().Length >= 0)
            {
                BroadCast("*Win_" + player.GetName());
                return true;
            }
            return false;
        }

        public void Turn(int turnsToTransfer, Player player)
        {
            if (direction == false)
            {
                turn = turn - turnsToTransfer;
                if (turn < 0)
                    turn = turn + numOfPlayers;
            }
            else
            {
                turn = turn + turnsToTransfer;
                if (turn >= numOfPlayers)
                    turn = turn - numOfPlayers;
            }

            BroadCast("*TurnUpdate_" + clientArray[turn].GetName());
            BroadCast("*NumCardsUpdate_" + player.GetName() + "_" + player.GetCards().Length);

            if (TwoPlusCounter != 0)
            {
                clientArray[turn].SendMessage("*TwoPlusMode");
            }
            else
            {
                clientArray[turn].SendMessage("*Turn");
            }
        }

        public Card GetTopCard()
        {
            return pile.GetTopCard();
        }

        // Send message to everyone
        public void BroadCast(string message)
        {
            for (int i=0; i<clientArray.Length; i++)
            {
                clientArray[i].SendMessage(message); 
            }
        }
    }
}

