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
    public partial class LoginForm : Form
    {
        private ClientManager clientManager;

        private delegate void SafeShowError(string error);
        private delegate void SafeSetVisible(bool visible);
     


        public LoginForm(ClientManager clientManager)
        {
            this.clientManager = clientManager;
            InitializeComponent();
        }

        public void SetVisible(bool visible)
        {
            if (LoginMessage.InvokeRequired)
            {
                this.Invoke(new SafeSetVisible(SetVisible), visible);
            }
            else
            {
                this.Visible = visible;
            }
        }

        private void MyName_Click(object sender, EventArgs e)
        {

        }
     
        private void Connect_Click(object sender, EventArgs e)
        {

            if (userNameField.Text == "" )
            {
                ShowError("Please specify user name");
            }
            else if (userNameField.Text.Contains("_") || userNameField.Text.Contains("*")) 
            {
                ShowError("User name cannot container '_' or '*' sign");
            }
            else if (serverNameField.Text == "")
            {
                ShowError("Please specify server address");
            }
            else
            {                
                clientManager.Connect(serverNameField.Text, userNameField.Text);
            }               
        }

        // Display connection error message
        public void ShowError( string error )
        {            
            if (LoginMessage.InvokeRequired)
            {
                this.Invoke(new SafeShowError(ShowError), error);
            }
            else
            {
                LoginMessage.Text = error;
            }
        }

        private void userNameField_TextChanged(object sender, EventArgs e)
        {
            LoginMessage.Text = "";
        }

        private void serverNameField_TextChanged(object sender, EventArgs e)
        {
            LoginMessage.Text = "";
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            LoginMessage.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
