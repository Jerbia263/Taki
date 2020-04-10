namespace TakiClient
{
    partial class GamesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GamesForm));
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            this.labelConnectdPlayers = new System.Windows.Forms.Label();
            this.connectedPlayersPanel = new System.Windows.Forms.Panel();
            this.gamesPanel = new System.Windows.Forms.Panel();
            this.activeGames = new System.Windows.Forms.Label();
            this.newGameButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelConnectdPlayers
            // 
            this.labelConnectdPlayers.AutoSize = true;
            this.labelConnectdPlayers.Location = new System.Drawing.Point(522, 168);
            this.labelConnectdPlayers.Name = "labelConnectdPlayers";
            this.labelConnectdPlayers.Size = new System.Drawing.Size(99, 13);
            this.labelConnectdPlayers.TabIndex = 0;
            this.labelConnectdPlayers.Text = "Connected Players:";
            this.labelConnectdPlayers.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // connectedPlayersPanel
            // 
            this.connectedPlayersPanel.BackColor = System.Drawing.Color.White;
            this.connectedPlayersPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.connectedPlayersPanel.Location = new System.Drawing.Point(525, 184);
            this.connectedPlayersPanel.Name = "connectedPlayersPanel";
            this.connectedPlayersPanel.Size = new System.Drawing.Size(167, 276);
            this.connectedPlayersPanel.TabIndex = 21;
            // 
            // gamesPanel
            // 
            this.gamesPanel.AutoScroll = true;
            this.gamesPanel.BackColor = System.Drawing.Color.White;
            this.gamesPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gamesPanel.Location = new System.Drawing.Point(29, 184);
            this.gamesPanel.Name = "gamesPanel";
            this.gamesPanel.Size = new System.Drawing.Size(445, 276);
            this.gamesPanel.TabIndex = 0;
            // 
            // activeGames
            // 
            this.activeGames.AutoSize = true;
            this.activeGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeGames.Location = new System.Drawing.Point(26, 139);
            this.activeGames.Name = "activeGames";
            this.activeGames.Size = new System.Drawing.Size(89, 13);
            this.activeGames.TabIndex = 23;
            this.activeGames.Text = "Active Games:";
            this.activeGames.Click += new System.EventHandler(this.label1_Click_2);
            // 
            // newGameButton
            // 
            this.newGameButton.Location = new System.Drawing.Point(379, 478);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(95, 37);
            this.newGameButton.TabIndex = 24;
            this.newGameButton.Text = "New Game";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.NewGame_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(31, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(267, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(636, 536);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Alon Jerbi © 2020";
            this.label1.Click += new System.EventHandler(this.label1_Click_3);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Game ID:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Number Of Players:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Admin";
            this.label3.Click += new System.EventHandler(this.label3_Click_1);
            // 
            // GamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(730, 558);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.newGameButton);
            this.Controls.Add(this.activeGames);
            this.Controls.Add(this.gamesPanel);
            this.Controls.Add(this.connectedPlayersPanel);
            this.Controls.Add(this.labelConnectdPlayers);
            this.Name = "GamesForm";
            this.Text = "טאקי";
            this.Load += new System.EventHandler(this.TakiClient_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Diagnostics.PerformanceCounter performanceCounter1;
        private System.Windows.Forms.Label labelConnectdPlayers;
        private System.Windows.Forms.Panel connectedPlayersPanel;
        private System.Windows.Forms.Panel gamesPanel;
        private System.Windows.Forms.Label activeGames;
        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}

