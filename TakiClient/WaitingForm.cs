using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakiClient
{
    public partial class WaitingForm : Form
    {
        private ClientManager clientManager;
        private Label[] namesArray = new Label[0];

        private delegate void SafeSetVisible(bool visible);
        private delegate void delRemoveNames();
        private delegate void delAddNameToTable(string name);

        public WaitingForm(ClientManager clientManager)
        {
            this.clientManager = clientManager;
            InitializeComponent();
        }

        public void SetVisible(bool visible)
        {
            if (label1.InvokeRequired)
            {
                this.Invoke(new SafeSetVisible(SetVisible), visible);
            }
            else
            {
                this.Visible = visible;
            }
        }

        public void AddPlayer(string name)
        {
            this.Invoke(new delAddNameToTable(AddPlayerToList), name);
        }

        public void RemoveNames()
        {
            this.Invoke(new delRemoveNames(RemoveAllFromList));
        }

        public void AddPlayerToList(string name)
        {
            Array.Resize(ref namesArray, namesArray.Length + 1);
            namesArray[namesArray.Length - 1] = new Label();
            namesArray[namesArray.Length - 1].Text = name;
            tablePlayers.Controls.Add(namesArray[namesArray.Length - 1], 0, namesArray.Length);
        }

        public void RemoveAllFromList()
        {
            for (int i = 1; i < tablePlayers.RowCount; i++)
            {
                if (tablePlayers.GetControlFromPosition(0, i) != null)
                    tablePlayers.Controls.Remove(namesArray[i - 1]);
            }
            Array.Resize(ref namesArray, 0);
        }

        public void SetVisibleManagerButtons(bool visible)
        {
            this.Invoke(new SafeSetVisible(SetVisibleManagerBtns), visible);
        }

        public void SetVisibleManagerBtns(bool visible)
        {
            AddBot.Visible = visible;
            startGameButton.Visible = visible;
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {

        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            clientManager.SendMessage("StartGame");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            SetVisibleManagerButtons(false);
            clientManager.CloseWait();
        }

        private void AddBot_Click(object sender, EventArgs e)
        {
            clientManager.SendMessage("AddBot");
        }
    }
}
