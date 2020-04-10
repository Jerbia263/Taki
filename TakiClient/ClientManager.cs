using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TakiClient
{
    public class ClientManager
    {
        private TcpClient client = new TcpClient();        
        private const int PORT_NUMBER = 1500;
        private byte[] receivedData;        
        private GamesForm gamesForm;
        private PlayForm  playForm;
        private WaitingForm waitForm;
                
        public ClientManager()
        {
            //loginForm  = new LoginForm();
            //loginForm.SetClientManager(this);

            gamesForm  = new GamesForm(this);            
            gamesForm.Show();            

            playForm   = new PlayForm(this);            
            playForm.Show();
            playForm.Visible = false;
            waitForm   = new WaitingForm(this);
            waitForm.Show();
            waitForm.Visible = false;
            waitForm.SetVisibleManagerButtons(false);
        }

        public GamesForm GetGamesForm()
        {
            return gamesForm;
        }
        
        // Connect to Taki Server. Return "true" if successful
        public void Connect(string serverAddress, string userName)
        {          

            try
            {
                if (!client.Connected)
                {
                    client.Connect(serverAddress, PORT_NUMBER);
                    receivedData = new byte[client.ReceiveBufferSize];

                    // read from server (async)
                    client.GetStream().BeginRead(receivedData,
                                                  0,
                                                  System.Convert.ToInt32(client.ReceiveBufferSize),
                                                  ReceiveMessage,
                                                  null);
                }
            
                // send user name
                SendMessage("Name_" + userName);

            }
            catch (Exception ex)
            {
                gamesForm.Login("Failed connecting to server");
            }
        }       
        
        public void SendMessage(string message)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                // send message to the server
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // send the text
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Receieve message from Taki server
        private void ReceiveMessage(IAsyncResult asynchronousResult)
        {
            try
            {
                int bytesRead;

                // read the data from the server
                bytesRead = client.GetStream().EndRead(asynchronousResult);

                string recievedMessage = System.Text.Encoding.ASCII.GetString(receivedData, 0, bytesRead);
                HandleMessages(recievedMessage);
                client.GetStream().BeginRead(receivedData,
                                         0,
                                         System.Convert.ToInt32(client.ReceiveBufferSize),
                                         ReceiveMessage,
                                         null);
            }
            catch (Exception ex)
            {
                // ignor the error... fired when the user logs off
                Console.WriteLine(ex.Message);
            }
        }

        // Process a received message
        public void HandleMessages(string recievedMessage)
        {
            // The server sends list of messages, seperated by "*"
            // We split the messages and handle them one by one
            string[] serverMessages = recievedMessage.Split('*');


            for (int i = 0; i < serverMessages.Length; i++)
            {
                if (serverMessages[i] != "")
                {
                    // each message has elements that are seperated by "_".
                    string[] messageElements = serverMessages[i].Split('_');

                    switch (messageElements[0])
                    {                       
                        case "NameCheck":
                            {
                                if (messageElements[1] == "OK")
                                {
                                    // name is unique - get all connected users
                                    SendMessage("GetConnectedPlayers");
                                    gamesForm.Start();                                    
                                }
                                else
                                {
                                    gamesForm.Login("User is not unique, please choose another name");                                    
                                }                                
                                break;
                            }
                        case "Start":
                            {
                                waitForm.RemoveNames();
                                waitForm.SetVisible(false);
                                gamesForm.SetVisible(false);
                                playForm.SetVisible(true);
                                playForm.StartGame();                             
                                break;
                            }
                        case "TwoPlusMode":
                            {
                                playForm.TwoPlusHandler();
                                break;
                            }
                        case "CrazyCardMode":
                            {
                                playForm.ResetCards();
                                break;
                            }
                        case "Turn":
                            {
                                playForm.TurnHandler();
                                break;
                            }
                        case "ChooseColor":
                            {
                                playForm.ChooseColorHandler();
                                break;
                            }
                        case "Win":
                            {
                                playForm.WinHandler(messageElements[1]);
                                break;
                            }
                        case "InGameNames":
                            {
                                playForm.AddNamesHandler(messageElements);                                                                    
                                break;
                            }
                        case "LeftTheWait":
                            {
                                waitForm.RemoveNames();
                                SendMessage("GetNames");
                                break;
                            }
                        case "Names":
                            {
                                waitForm.RemoveNames();
                                for (int j=1; j<messageElements.Length; j++)
                                {
                                    waitForm.AddPlayer(messageElements[j]);
                                }
                                break;
                            }
                        // Server is updating the number of cards each player has
                        case "NumCardsUpdate":
                            {
                                playForm.NumCardsUpdate(messageElements[1], messageElements[2]);                                                                
                                break;
                            }
                        case "LeftTheGame":
                            {
                                SendMessage("GetInGameNames");
                                break;
                            }
                        case "RemovePlayerFromList":
                            {
                                gamesForm.RemovePlayerFromList(messageElements[1]);                                
                                break;
                            }
                        case "ShowConnectedPlayer":
                            {
                                gamesForm.ShowConnectedPlayer(messageElements[1]);
                                break;
                            }
                        case "ShowGame":
                            {
                                gamesForm.ShowGame(messageElements[1], messageElements[2]);
                                break;
                            }
                        case "Reset":
                            {
                                playForm.Reset();
                                break;
                            }                        
                        case "RemoveGame":
                            {
                                gamesForm.RemoveGame(messageElements[1]);
                                break;
                            }
                        case "TurnUpdate":
                            {
                                playForm.TurnUpdate(messageElements[1]);
                                break;
                            }
                        case "AddCard":
                            {
                                playForm.AddCard(messageElements[1], messageElements[2]);
                                break;
                            }
                        case "StartingCard":
                            {
                                playForm.SetStartingCard(messageElements[1], messageElements[2]);
                                break;
                            }
                        case "Remove":
                            {
                                playForm.RemoveCard(messageElements[1], messageElements[2]);
                                break;
                            }
                        case "Manager":
                            {
                                waitForm.SetVisibleManagerButtons(true);
                                break;
                            }
                        case "CancelGame":
                            {
                                CloseWait();
                                break;
                            }
                        case "NumOfPlayersUpdate":
                            {
                                gamesForm.UpdateNumOfPlayers(messageElements[1], messageElements[2]);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        public void ClosePlay()
        {
            // Signal server that player quit game
            SendMessage("EndGame");
            // Show the games board
            gamesForm.SetVisible(true);
            playForm.SetVisible(false);
        }

        public WaitingForm GetWaitForm()
        {
            return waitForm;
        }

        public void CloseWait()
        {
            // Signal server that player quit game
            SendMessage("Cancel");
            // Show the games board
            gamesForm.SetVisible(true);
            waitForm.SetVisible(false);
        }

        public void Rematch()
        {
            SendMessage("Rematch");
            ShowWaitingForm();
        }

        public void ShowWaitingForm()
        {            
            playForm.SetVisible(false);
            gamesForm.SetVisible(false);
            waitForm.SetVisible(true);            
        }
    }
}
