namespace TakiClient
{
    partial class WaitingForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.startGameButton = new System.Windows.Forms.Button();
            this.tablePlayers = new System.Windows.Forms.TableLayoutPanel();
            this.labelPlayers = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AddBot = new System.Windows.Forms.Button();
            this.tablePlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(425, 171);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // startGameButton
            // 
            this.startGameButton.Location = new System.Drawing.Point(344, 171);
            this.startGameButton.Name = "startGameButton";
            this.startGameButton.Size = new System.Drawing.Size(75, 23);
            this.startGameButton.TabIndex = 2;
            this.startGameButton.Text = "Start Game";
            this.startGameButton.UseVisualStyleBackColor = true;
            this.startGameButton.Click += new System.EventHandler(this.startGameButton_Click);
            // 
            // tablePlayers
            // 
            this.tablePlayers.ColumnCount = 1;
            this.tablePlayers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePlayers.Controls.Add(this.labelPlayers, 0, 0);
            this.tablePlayers.Location = new System.Drawing.Point(57, 54);
            this.tablePlayers.Name = "tablePlayers";
            this.tablePlayers.RowCount = 5;
            this.tablePlayers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePlayers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePlayers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePlayers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePlayers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePlayers.Size = new System.Drawing.Size(200, 99);
            this.tablePlayers.TabIndex = 3;
            // 
            // labelPlayers
            // 
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.Location = new System.Drawing.Point(3, 0);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(44, 13);
            this.labelPlayers.TabIndex = 0;
            this.labelPlayers.Text = "Players:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // AddBot
            // 
            this.AddBot.Location = new System.Drawing.Point(263, 171);
            this.AddBot.Name = "AddBot";
            this.AddBot.Size = new System.Drawing.Size(75, 23);
            this.AddBot.TabIndex = 5;
            this.AddBot.Text = "Add Bot";
            this.AddBot.UseVisualStyleBackColor = true;
            this.AddBot.Click += new System.EventHandler(this.AddBot_Click);
            // 
            // WaitingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 220);
            this.ControlBox = false;
            this.Controls.Add(this.AddBot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tablePlayers);
            this.Controls.Add(this.startGameButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "WaitingForm";
            this.Text = "Waiting for players";
            this.Load += new System.EventHandler(this.WaitingForm_Load);
            this.tablePlayers.ResumeLayout(false);
            this.tablePlayers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button startGameButton;
        private System.Windows.Forms.TableLayoutPanel tablePlayers;
        private System.Windows.Forms.Label labelPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddBot;
    }
}