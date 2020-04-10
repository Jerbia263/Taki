using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TakiServer
{
    class Player
    {
        private TcpClient client;
        private string _clientIP;
        private string _clientNick;
        private Game game;
        private ServerManager serverManager;

        private bool inGame = false;

        private byte[] data;

        private Card[] playerCards;

        private int INITIAL_CARD_COUNT = 8;

        private Bot bot;

        public Player(Bot bot)
        {
            this.bot = bot;
        }

        public Player(TcpClient client, ServerManager serverManager)
        {
            this.client   = client;
            this.serverManager = serverManager;
            playerCards = new Card[INITIAL_CARD_COUNT];
            _clientIP = client.Client.RemoteEndPoint.ToString();

            data = new byte[client.ReceiveBufferSize];

            
            client.GetStream().BeginRead(data,
                                            0,
                                            System.Convert.ToInt32(client.ReceiveBufferSize),
                                            ReceiveMessage,
                                            null);
        }

        public void SendMessage(string message)
        {
            if (bot != null)
            {
                bot.HandleMessages(message);
                return; 
            }
            try
            {
                System.Net.Sockets.NetworkStream ns;

                lock (client.GetStream())
                {
                    ns = client.GetStream();
                }

                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(message);
                ns.Write(bytesToSend, 0, bytesToSend.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ReceiveMessage(IAsyncResult ar)
        {
            int bytesRead;
            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(ar);
                }
                string messageReceived = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                if (inGame)
                {
                    game.HandleMessages(this, messageReceived);
                }
                else
                {
                    serverManager.HandleMessages(messageReceived, this);
                }
                lock (client.GetStream())
                {
                    client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (inGame)
                {
                    game.DisconnectPlayer(this);
                    game.BroadCast("*LeftTheGame");
                }
                else
                {
                    serverManager.DisconnectPlayer(this);
                    serverManager.BroadCast("*RemovePlayerFromList_" + _clientNick);
                }
            }
        }

        // save initial cards
        public void SetInitialCards(Card[] cards)
        {
            playerCards = new Card[cards.Length];
            for (int i=0; i<cards.Length; ++i)
            {
                playerCards[i] = cards[i];
            }
        }

        public void SendInitialCards()
        {
            for (int i = 0; i < playerCards.Length; ++i)
            {
                SendMessage("*AddCard_" + playerCards[i].GetValue().ToString() + "_" + playerCards[i].GetColor());
            }

        }

        public TcpClient GetTcpClient()
        {
            return client;
        }

        public void AddCard(Card card)
        {
            if (game.deck.isEmpty())
            {
                Deck temp = new Deck();
                game.deck = temp;
            }
            Array.Resize(ref playerCards, playerCards.Length + 1);
            playerCards[playerCards.Length - 1] = card;
            SendMessage("*AddCard_" + card.GetValue().ToString() + "_" + card.GetColor());
        }

        public void RemoveCard(Card card)
        {
            bool found = false;
            int i = 0;

            while (!found && i< playerCards.Length)
            {
                if (card.GetColor() == playerCards[i].GetColor() && card.GetValue() == playerCards[i].GetValue())
                {
                    playerCards[i] = playerCards[playerCards.Length - 1];
                    Array.Resize(ref playerCards, playerCards.Length - 1);
                    found = true;
                    SendMessage("*Remove_" + card.GetValue().ToString() + "_" + card.GetColor());
                }
                i++;
            }
        }

        public void SetGame(Game game)
        {
            this.game = game;
            if (game == null)
            {
                this.inGame = false;
            }
            else
            {
                this.inGame = true;
            }
        }

        public Game GetGame()
        {
            return game;
        }

        public Card[] GetCards()
        {
            return playerCards;
        }

        public string GetName()
        {
            return _clientNick;
        }

        public void SetName(string name)
        {
            _clientNick = name;
        }
    }
}
