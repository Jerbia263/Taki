using System;
using System.Collections.Generic;
using System.Text;

namespace TakiServer
{
    class ServerManager
    {
        private Player[] usersArray;
        private Game[] gameArray;
        private int gameCount = 0;
        private DataBase dataBase;

        public ServerManager()
        {
            usersArray = new Player[0];
            gameArray = new Game[0];
            dataBase = new DataBase();
        }

        public void Addplayer(Player player)
        {
            Array.Resize(ref usersArray, usersArray.Length + 1);
            usersArray[usersArray.Length - 1] = player;
            ShowAvailableGames(player);
        }

        public void HandleMessages(string message, Player player)
        {
            string[] messageArray = message.Split('_');

            switch (messageArray[0])
            {
                case "Name":
                    {
                        if (!IsNameExists(messageArray[1]))
                        {
                            player.SendMessage("*NameCheck_OK_" + messageArray[1]);
                            //dataBase.SetUser(messageArray[1]);
                            player.SetName(messageArray[1]);
                        }
                        else
                        {
                            player.SendMessage("*NameCheck_NOTOK_" + messageArray[1]);
                        }
                        break;
                    }
                case "GetConnectedPlayers":
                    {
                        ShowConnectedPlayers(player);
                        BroadCast("*ShowConnectedPlayer_" + player.GetName());
                        break;
                    }
                case "CreateGame":
                    {
                        CreateNewGame(player);
                        break;
                    }
                case "JoinGame":
                    {
                        JoinCurrentGame(player, Int32.Parse(messageArray[1]));
                        break;
                    }
                case "Disconnect":
                    {
                        DisconnectPlayer(player);
                        BroadCast("*RemovePlayerFromList_" + player.GetName());
                        break;
                    }
            }
        }

        public void JoinCurrentGame(Player player, int numOfGame)
        {
            gameArray[numOfGame].AddPlayer(player);
        }

        public void RematchGame(Game game)
        {
            int id = game.GetId();
            Player manager = game.GetManager();
            gameArray[id] = new Game(this, id, false, manager);
        }

        public void CreateNewGame(Player player)
        {
            gameCount++;
            Array.Resize(ref gameArray, gameCount);
            gameArray[gameCount - 1] = new Game(this, gameCount - 1, true, player);
            gameArray[gameCount - 1].AddPlayer(player);
            BroadCast("*ShowGame_" + gameArray[gameCount - 1].GetId() + "_" + player.GetName());
        }

        public void ShowAvailableGames(Player player)
        {
            for (int i=0; i<gameArray.Length; i++)
            {
                if (!gameArray[i].GetisStarted())
                {
                    player.SendMessage("*ShowGame_" + gameArray[i].GetId() + "_" + gameArray[i].GetManager().GetName());
                }
            }
        }

        public void ShowConnectedPlayers(Player player)
        {
            for (int i = 0; i < usersArray.Length; i++)
            {
                if (usersArray[i].GetName() != player.GetName())
                    player.SendMessage("*ShowConnectedPlayer_" + usersArray[i].GetName());
            }
        }

        public void DisconnectPlayer(Player player)
        {
            bool found = false;
            for (int i = 0; i < usersArray.Length - 1; i++)
            {
                if (usersArray[i].GetName() == player.GetName())
                    found = true;
                if (!found)
                    usersArray[i] = usersArray[i];
                if (found)
                    usersArray[i] = usersArray[i + 1];
            }
            Array.Resize(ref usersArray, usersArray.Length - 1);
        }

        public void RemoveGameFromLists(int id)
        {
            BroadCast("*RemoveGame_" + id);
        }

        public bool IsNameExists(string name) 
        {
            for (int i = 0; i < usersArray.Length; i++)
            {
                if (usersArray[i].GetName() == name)
                    return true;
            }
            return false;
        }

        public void BroadCast(string message)
        {
            for (int i = 0; i < usersArray.Length; i++)
            {
                usersArray[i].SendMessage(message);
            }
        }
    }
}
