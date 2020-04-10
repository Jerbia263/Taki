using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakiClient
{
    public partial class GamesForm : Form
    {
        private Button[] joinButtons = new Button[0];
        private Label[] labelsId = new Label[0];
        private Label[] labelsAdminName = new Label[0];
        private Label[] labelsNumOfPlayers = new Label[0];
        private Label[] connectedPlayers = new Label[0];
        private ClientManager clientManager;
        LoginForm login;

        private delegate void delListsHandler(string data);
        private delegate void delHandleGameList(string data, string name);
        private delegate void SafeSetVisible(bool visible);

        public GamesForm(ClientManager clientManager)
        {
            this.clientManager = clientManager;
            InitializeComponent();
        }
             
        public void SetVisible(bool visible)
        {
            if (newGameButton.InvokeRequired) 
            { 
                this.Invoke(new SafeSetVisible(SetVisible), visible);
            }
            else
            {
                this.Visible = visible;
            }
        }
    
        public void Start()
        {
            login.SetVisible(false);
        }

        public void Login(string error)
        {
            login.SetVisible(true);
            login.ShowError(error);
        }

        public void RemovePlayerFromList(string player)
        {
            this.Invoke(new delListsHandler(RemoveNameFromList), player);
        }

        public void ShowConnectedPlayer(string name)
        {
            this.Invoke(new delListsHandler(AddNameToList), name);
        }

        public void ShowGame(string game, string adminName)
        {
            this.Invoke(new delHandleGameList(AddGameToList), game, adminName);
        }

        public void RemoveGame(string game)
        {
            this.Invoke(new delListsHandler(RemoveGameFromList), game);

        }
        
        private void UpdateLabel( Label label, string text )
        {
            label.Text = text;
        }

        public void AddNameToList(string name)
        {
            Array.Resize(ref connectedPlayers, connectedPlayers.Length + 1);
            connectedPlayers[connectedPlayers.Length - 1] = new Label();
            connectedPlayers[connectedPlayers.Length - 1].Size = new System.Drawing.Size(50, 20);
            connectedPlayers[connectedPlayers.Length - 1].Location = new System.Drawing.Point(0, (connectedPlayers.Length - 1) * 25);
            connectedPlayers[connectedPlayers.Length - 1].Text = "- " + name;
            connectedPlayers[connectedPlayers.Length - 1].Name = name;
            connectedPlayersPanel.Controls.Add(connectedPlayers[connectedPlayers.Length - 1]);
        }

        public void RemoveNameFromList(string nameToRemove)
        {
            int j = 0;
            bool found = false;
            while (!found)
            {
                string name = connectedPlayers[j].Name;
                if (name == nameToRemove)
                {
                    found = true;
                    connectedPlayersPanel.Controls.Remove(connectedPlayers[j]);
                }
                else
                {
                    j++;
                }
            }
            for (int i = j; i < connectedPlayers.Length - 1; i++)
            {
                connectedPlayers[i] = connectedPlayers[i + 1];
                connectedPlayers[i].Location = new System.Drawing.Point(0, i * 25);
            }
            Array.Resize(ref connectedPlayers, connectedPlayers.Length - 1);
        }

        public void UpdateNumOfPlayers(string id, string numOfPlayers)
        {
            for (int i=0; i<labelsId.Length; i++)
            {
                if (labelsId[i].Text == id)
                {
                    this.Invoke(new delHandleGameList(NumOfPlayersUpdate), i.ToString(), numOfPlayers);
                }
            }
        }

        public void NumOfPlayersUpdate(string location, string numOfPlayers)
        {
            labelsNumOfPlayers[Int32.Parse(location)].Text = numOfPlayers +                                                                                         "/4";
        }

        public void AddGameToList(string numOfGame, string adminName)
        {
            Array.Resize(ref labelsNumOfPlayers, labelsNumOfPlayers.Length + 1);
            labelsNumOfPlayers[labelsNumOfPlayers.Length - 1] = new Label();
            labelsNumOfPlayers[labelsNumOfPlayers.Length - 1].Location = new System.Drawing.Point(200, (labelsNumOfPlayers.Length - 1) * 30);
            labelsNumOfPlayers[labelsNumOfPlayers.Length - 1].Text = "1/4";
            gamesPanel.Controls.Add(labelsNumOfPlayers[labelsNumOfPlayers.Length - 1]);
            Array.Resize(ref labelsAdminName, labelsAdminName.Length + 1);
            labelsAdminName[labelsAdminName.Length - 1] = new Label();
            labelsAdminName[labelsAdminName.Length - 1].Location = new System.Drawing.Point(100, (labelsAdminName.Length - 1) * 30);
            labelsAdminName[labelsAdminName.Length - 1].Text = adminName;
            gamesPanel.Controls.Add(labelsAdminName[labelsAdminName.Length - 1]);
            Array.Resize(ref labelsId, labelsId.Length + 1);
            labelsId[labelsId.Length - 1] = new Label();
            labelsId[labelsId.Length - 1].Location = new System.Drawing.Point(0, (labelsId.Length - 1) * 30);
            labelsId[labelsId.Length - 1].Text = numOfGame;
            gamesPanel.Controls.Add(labelsId[labelsId.Length - 1]);
            Array.Resize(ref joinButtons, joinButtons.Length + 1);
            joinButtons[joinButtons.Length - 1] = new Button();
            joinButtons[joinButtons.Length - 1].Size = new System.Drawing.Size(50, 20);
            joinButtons[joinButtons.Length - 1].Location = new System.Drawing.Point(390, (joinButtons.Length - 1) * 30);
            joinButtons[joinButtons.Length - 1].Text = "join";
            joinButtons[joinButtons.Length - 1].BackColor = Color.LightGreen;
            joinButtons[joinButtons.Length - 1].Name = numOfGame.ToString();
            joinButtons[joinButtons.Length - 1].Click += new System.EventHandler(JoinGame);
            gamesPanel.Controls.Add(joinButtons[joinButtons.Length - 1]);
        }

        public void RemoveGameFromList(string idToRemove)
        {
            int j = 0;
            bool found = false;
            while (!found)
            {
                string name = joinButtons[j].Name;
                if (name == idToRemove)
                {
                    found = true;
                    gamesPanel.Controls.Remove(labelsNumOfPlayers[j]);
                    gamesPanel.Controls.Remove(labelsAdminName[j]);
                    gamesPanel.Controls.Remove(labelsId[j]);
                    gamesPanel.Controls.Remove(joinButtons[j]);
                }
                else
                {
                    j++;
                }
            }
            for (int i=j; i<joinButtons.Length - 1; i++)
            {
                labelsNumOfPlayers[i] = labelsNumOfPlayers[i + 1];
                labelsNumOfPlayers[i].Location = new System.Drawing.Point(200, i * 30);
                labelsAdminName[i] = labelsAdminName[i + 1];
                labelsAdminName[i].Location = new System.Drawing.Point(100, i * 30);
                labelsId[i] = labelsId[i + 1];
                labelsId[i].Location = new System.Drawing.Point(0, i * 30);
                joinButtons[i] = joinButtons[i + 1];
                joinButtons[i].Location = new System.Drawing.Point(390, i * 30);
            }
            Array.Resize(ref labelsNumOfPlayers, labelsNumOfPlayers.Length - 1);
            Array.Resize(ref labelsAdminName, labelsAdminName.Length - 1);
            Array.Resize(ref labelsId, labelsId.Length - 1);
            Array.Resize(ref joinButtons, joinButtons.Length - 1);
        }
                     


        public void JoinGame(object sender, EventArgs e)
        {
            clientManager.ShowWaitingForm();
            //userNameLabel.Text = "Waiting for players...";
            Button temp = (Button)sender;
            clientManager.SendMessage("JoinGame_" + temp.Name);
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            clientManager.ShowWaitingForm();
            clientManager.SendMessage("CreateGame");
        }

        private void TakiClient_Load(object sender, EventArgs e)
        {
            // show the login page
            login = new LoginForm(clientManager);
            login.ShowDialog();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void tableOfStats_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
