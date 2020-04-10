namespace TakiClient
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.serverNameField = new System.Windows.Forms.TextBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.userNameField = new System.Windows.Forms.TextBox();
            this.Connect = new System.Windows.Forms.Button();
            this.LoginMessage = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(204, 157);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(41, 13);
            this.serverNameLabel.TabIndex = 24;
            this.serverNameLabel.Text = "Server:";
            // 
            // serverNameField
            // 
            this.serverNameField.Location = new System.Drawing.Point(267, 154);
            this.serverNameField.Name = "serverNameField";
            this.serverNameField.Size = new System.Drawing.Size(156, 20);
            this.serverNameField.TabIndex = 23;
            this.serverNameField.Text = "127.0.0.1";
            this.serverNameField.TextChanged += new System.EventHandler(this.serverNameField_TextChanged);
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(204, 130);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(61, 13);
            this.userNameLabel.TabIndex = 22;
            this.userNameLabel.Text = "User name:";
            // 
            // userNameField
            // 
            this.userNameField.Location = new System.Drawing.Point(267, 127);
            this.userNameField.Name = "userNameField";
            this.userNameField.Size = new System.Drawing.Size(156, 20);
            this.userNameField.TabIndex = 21;
            this.userNameField.TextChanged += new System.EventHandler(this.userNameField_TextChanged);
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(267, 180);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 20;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // LoginMessage
            // 
            this.LoginMessage.AutoSize = true;
            this.LoginMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LoginMessage.Location = new System.Drawing.Point(210, 98);
            this.LoginMessage.Name = "LoginMessage";
            this.LoginMessage.Size = new System.Drawing.Size(35, 13);
            this.LoginMessage.TabIndex = 25;
            this.LoginMessage.Text = "label1";
            this.LoginMessage.Click += new System.EventHandler(this.MyName_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(348, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 326);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LoginMessage);
            this.Controls.Add(this.serverNameLabel);
            this.Controls.Add(this.serverNameField);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.userNameField);
            this.Controls.Add(this.Connect);
            this.Name = "LoginForm";
            this.Text = "Connect to Taki Server";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.TextBox serverNameField;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.TextBox userNameField;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Label LoginMessage;
        private System.Windows.Forms.Button button1;
    }
}