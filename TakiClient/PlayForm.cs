using IdentityModel.Client;
using OpenQA.Selenium.Remote;
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
    public partial class PlayForm : Form
    {
        private PictureBox[] pbs = new PictureBox[0];
        private Button[] ColorButtonsArray = new Button[4];
        private Button rematchButton = new Button();
        private Button backToMenuButton = new Button();
        private PictureBox[] topCard = new PictureBox[2];
        private PictureBox deckCards = new PictureBox();
        private bool IsMyTurn = false;
        private Label[,] labelsOfStats = new Label[5, 3];
        private ClientManager clientManager;

        private delegate void SafeSetVisible(bool visible);

        public PlayForm(ClientManager clientManager)
        {
            InitializeComponent();
            this.clientManager = clientManager;
            turnLabel.Text = "";
            topCard[0] = new PictureBox();
            topCard[1] = new PictureBox();
            this.Closing += new CancelEventHandler(PlayForm_Close);
        }

        // Show/hide the Taki PlayForm
        public void SetVisible(bool visible)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SafeSetVisible(SetVisible), visible);
            }
            else
            {
                this.Visible = visible;
            }
        }

        private void PlayForm_Load(object sender, EventArgs e)
        {

        }

        // Player quit game
        private void PlayForm_Close(object sender, EventArgs e)
        {
            clientManager.ClosePlay();
        }

        // Start a new game - initialize the board
        public void StartGame()
        {
            clientManager.SendMessage("Start");
            CreateTableCells();
            topCard[0] = new PictureBox();
            topCard[1] = new PictureBox();
            this.Invoke(new delDisplayCard(DisplayDeckCards), new Card("back", Card.cardValue.Deck));
        }


        // ------------------ Game Handlers --------------------
        public void TwoPlusHandler()
        {
            if (CheckIfHaveTwo())
            {
                IsMyTurn = true;
                this.Invoke(new deleUpdateLabel(UpdateLabel), turnLabel, "Your Turn");
            }
        }


        public void TurnHandler()
        {
            IsMyTurn = true;
            this.Invoke(new deleUpdateLabel(UpdateLabel), turnLabel, "Your Turn");
        }

        public void WinHandler(string winner)
        {
            if (winner == tableOfStats.GetControlFromPosition(0, 1).Text)
            {
                clientManager.SendMessage("NewGame");
            }
            turnLabel.ForeColor = System.Drawing.Color.Gold;
            this.Invoke(new deleUpdateLabel(UpdateLabel), turnLabel, winner + " is the WINNER");
            this.Invoke(new delVisible(RematchButton), true);
            this.Invoke(new delVisible(BackToMenuButton), true);
        }

        public void ChooseColorHandler()
        {
            this.Invoke(new delVisible(ColorButtons), true);
        }

        public void AddNamesHandler(string[] names)
        {
            for (int j = 1; j < tableOfStats.RowCount; j++)
            {
                if (j < names.Length)
                {
                    this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[j, 0], names[j]);
                    this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[j, 1], "8");
                }
                else
                {
                    this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[j, 0], "");
                    this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[j, 1], "");
                }
            }
        }

        public void NumCardsUpdate(string name, string numOfCards)
        {
            for (int j = 1; j < tableOfStats.RowCount; j++)
            {
                if (tableOfStats.GetControlFromPosition(0, j) != null)
                {
                    if (tableOfStats.GetControlFromPosition(0, j).Text == name)
                    {
                        this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[j, 1], numOfCards);
                        break;
                    }
                }
            }
        }
            
        public void TurnUpdate(string name)
        {
            for (int j = 1; j < tableOfStats.RowCount; j++)
            {
                if (tableOfStats.GetControlFromPosition(0, j) != null)
                {
                    if (tableOfStats.GetControlFromPosition(0, j).Text == name)
                    {
                        this.Invoke(new deleUpdateCellColor(ColorRowTurnUpdate), j, Color.LightGreen);
                    }
                    else
                    {
                        this.Invoke(new deleUpdateCellColor(ColorRowTurnUpdate), j, Color.White);
                    }
                }
            }
        }

        public void AddCard(string value, string color)
        {
            this.Invoke(new delDisplayCard(DisplayCard), new Card(value, color));
        }

        public void SetStartingCard(string value, string color)
        {
            this.Invoke(new delDisplayCard(DisplayTopCard), new Card(value, color));
        }

        public void RemoveCard(string value, string color)
        {
            this.Invoke(new delDisplayCard(RemoveCardFromPanel), new Card(value, color));
        }

        public void ClearTable()
        {
            for (int i = 0; i < tableOfStats.RowCount; i++)
            {
                for (int j = 0; j < tableOfStats.ColumnCount; j++)
                {
                    this.Invoke(new delHandleCell(HandleCellInTable), labelsOfStats[i, j], j, i, false);
                }
            }
        }
        public void RematchButton(bool visible)
        {
            rematchButton.Location = new System.Drawing.Point(250, 200);
            rematchButton.Size = new System.Drawing.Size(100, 100);
            rematchButton.Text = "Rematch";
            this.Controls.Add(rematchButton);
            rematchButton.Click += new System.EventHandler(RematchClick);
            rematchButton.Visible = visible;
        }

        public void BackToMenuButton(bool visible)
        {
            backToMenuButton.Location = new System.Drawing.Point(350, 200);
            backToMenuButton.Size = new System.Drawing.Size(100, 100);
            backToMenuButton.Text = "Back to menu";
            this.Controls.Add(backToMenuButton);
            backToMenuButton.Click += new System.EventHandler(BackToMenuClick);
            backToMenuButton.Visible = visible;
        }

        public void HandleCellInTable(Label label, int x, int y, bool create)
        {
            if (create)
                tableOfStats.Controls.Add(label, x, y);
            else
                tableOfStats.Controls.Remove(label);
        }

        public void BackToMenuClick(object sender, EventArgs e)
        {
            ClearTable(); 
            Reset();
            clientManager.GetWaitForm().SetVisibleManagerButtons(false);
            clientManager.ClosePlay();            
        }

        public void Reset()
        {
            turnLabel.Text = "";
            turnLabel.ForeColor = Color.FromArgb(0, 0, 192);
            rematchButton.Visible = false;
            backToMenuButton.Visible = false;
            this.Invoke(new Remove(RemovePbs), topCard[0], false);
            this.Invoke(new Remove(RemovePbs), topCard[1], false);
            topCard = new PictureBox[2];
            this.Invoke(new Remove(RemovePbs), deckCards, false);
            deckCards = new PictureBox();
            IsMyTurn = false;
            ResetCards();
        }

        public void ResetCards()
        {
            for (int j = 0; j < pbs.Length; j++)
                this.Invoke(new Remove(RemovePbs), pbs[j], true);
            pbs = new PictureBox[0];
        }

        public void RematchClick(object sender, EventArgs e)
        {
            ClearTable();
            Reset();
            //userNameLabel.Text = "Waiting for players...";
            clientManager.Rematch();
        }

        public void ColorButtons(bool visible)
        {
            for (int i = 0; i < 4; i++)
            {
                if (visible)
                {
                    ColorButtonsArray[i] = new Button();
                    ColorButtonsArray[i].Location = new System.Drawing.Point(300 + i % 2 * 50, 200 + i / 2 * 50);
                    if (i == 0)
                    {
                        ColorButtonsArray[i].BackColor = Color.Red;
                        ColorButtonsArray[i].Name = "red";
                        ColorButtonsArray[i].Text = "red";
                    }
                    if (i == 1)
                    {
                        ColorButtonsArray[i].BackColor = Color.Blue;
                        ColorButtonsArray[i].Name = "blue";
                        ColorButtonsArray[i].Text = "blue";
                    }
                    if (i == 2)
                    {
                        ColorButtonsArray[i].BackColor = Color.Green;
                        ColorButtonsArray[i].Name = "green";
                        ColorButtonsArray[i].Text = "green";
                    }
                    if (i == 3)
                    {
                        ColorButtonsArray[i].BackColor = Color.Yellow;
                        ColorButtonsArray[i].Name = "yellow";
                        ColorButtonsArray[i].Text = "yellow";
                    }
                    ColorButtonsArray[i].Size = new System.Drawing.Size(50, 50);
                    this.Controls.Add(ColorButtonsArray[i]);
                    ColorButtonsArray[i].Click += new System.EventHandler(ColorClick);
                }
                else
                {
                    ColorButtonsArray[i].Visible = visible;
                    this.Controls.Add(ColorButtonsArray[i]);
                }
            }
        }

        public void ColorClick(object sender, EventArgs e)
        {
            Button b1 = (Button)sender;
            this.Invoke(new delVisible(ColorButtons), false);
            clientManager.SendMessage("ChangeColorSelect_" + b1.Name);
        }

        public bool CheckIfHaveTwo()
        {
            for (int i = 0; i < pbs.Length; i++)
            {
                if (pbs[i].Name.StartsWith("Two"))
                {
                    return true;
                }
            }
            return false;
        }

        private delegate void delHandleCell(Label label, int x, int y, bool create);
        private delegate void delVisible(bool visible);
        private delegate void delDisplayCard(Card card);
        private delegate void deleUpdateLabel(Label label, string text);
        private delegate void deleUpdateCellColor(int num, Color color);
        private delegate void Remove(PictureBox pbs, bool isInPanel);

        private void RemovePbs(PictureBox pbs, bool isInPanel)
        {
            if (isInPanel)
                cardPanel.Controls.Remove(pbs);
            else
                this.Controls.Remove(pbs);
        }

        public void ColorRowTurnUpdate(int row, Color color)
        {
            tableOfStats.GetControlFromPosition(0, row).BackColor = color;
            tableOfStats.GetControlFromPosition(1, row).BackColor = color;
        }

        private void DisplayCard(Card card)
        {
            Array.Resize(ref pbs, pbs.Length + 1);

            pbs[pbs.Length - 1] = new PictureBox();
            pbs[pbs.Length - 1].ImageLocation = card.GetPictureFile();
            pbs[pbs.Length - 1].BorderStyle = BorderStyle.FixedSingle;
            pbs[pbs.Length - 1].SizeMode = PictureBoxSizeMode.StretchImage;
            pbs[pbs.Length - 1].Location = new System.Drawing.Point((pbs.Length - 1) * 120 - cardPanel.HorizontalScroll.Value, 0);
            pbs[pbs.Length - 1].Name = card.GetValue().ToString() + "_" + card.GetColor();
            pbs[pbs.Length - 1].Size = new System.Drawing.Size(100, 200);
            pbs[pbs.Length - 1].TabIndex = 0;
            pbs[pbs.Length - 1].TabStop = false;
            pbs[pbs.Length - 1].Click += new System.EventHandler(Pbs_Click);
            cardPanel.Controls.Add(pbs[pbs.Length - 1]);
        }

        private void DisplayDeckCards(Card card)
        {
            deckCards.ImageLocation = card.GetPictureFile();
            deckCards.SizeMode = PictureBoxSizeMode.StretchImage;
            deckCards.Location = new System.Drawing.Point(600, 000);
            deckCards.Name = card.GetValue().ToString() + "_" + card.GetColor();
            deckCards.Size = new System.Drawing.Size(100, 200);
            deckCards.TabIndex = 0;
            deckCards.TabStop = false;
            this.Controls.Add(deckCards);
            deckCards.Click += new System.EventHandler(DeckCards_Click);
        }

        private void DeckCards_Click(object sender, EventArgs e)
        {
            if (IsMyTurn == true)
                clientManager.SendMessage("GetCard");
            IsMyTurn = false;
            this.Invoke(new deleUpdateLabel(UpdateLabel), turnLabel, "");
        }

        private void Pbs_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            CheckCard(pb);
        }

        public void CheckCard(PictureBox pb)
        {
            if (IsMyTurn == true)
            {
                clientManager.SendMessage("Check_" + pb.Name);
                IsMyTurn = false;
                this.Invoke(new deleUpdateLabel(UpdateLabel), turnLabel, "");
            }
        }

        private void UpdateLabel(Label label, string text)
        {
            label.Text = text;
        }

        private void DisplayTopCard(Card card)
        {
            PictureBox temp = topCard[0];
            bool isNull = true; 
            if (temp.Image != null)
            {
                isNull = false;
            }
            this.Controls.Remove(topCard[0]);
            topCard[0].BorderStyle = BorderStyle.FixedSingle; 
            topCard[0].ImageLocation = card.GetPictureFile();
            topCard[0].SizeMode = PictureBoxSizeMode.StretchImage;
            topCard[0].BringToFront();
            topCard[0].Location = new System.Drawing.Point(300, 000);
            topCard[0].Name = card.GetValue().ToString() + "_" + card.GetColor();
            topCard[0].Size = new System.Drawing.Size(100, 200);
            this.Controls.Add(topCard[0]);
            if (isNull == false)
            {
                this.Controls.Remove(topCard[1]);
                topCard[1].BorderStyle = BorderStyle.FixedSingle;
                topCard[1].Image = temp.Image;
                topCard[1].SizeMode = PictureBoxSizeMode.StretchImage;
                topCard[1].Location = new System.Drawing.Point(280, 000);
                topCard[1].Name = temp.Name; ;
                topCard[1].SendToBack();
                topCard[1].Size = new System.Drawing.Size(100, 200);
                this.Controls.Add(topCard[1]);
            }
        }

        //private void DisplayTopCard(Card card)
        //{

        //    this.Controls.Remove(topCard);
        //    topCard = new PictureBox();
        //    topCard.ImageLocation = card.GetPictureFile();
        //    topCard.SizeMode = PictureBoxSizeMode.StretchImage;
        //    topCard.BringToFront();
        //    topCard.Location = new System.Drawing.Point(300, 000);
        //    topCard.Name = card.GetValue().ToString() + "_" + card.GetColor();
        //    topCard.Size = new System.Drawing.Size(100, 200);
        //    this.Controls.Add(topCard);
        //}

        public void CreateTableCells()
        {
            for (int i = 0; i < tableOfStats.RowCount; i++)
            {
                for (int j = 0; j < tableOfStats.ColumnCount; j++)
                {
                    labelsOfStats[i, j] = new Label();
                    this.Invoke(new delHandleCell(HandleCellInTable), labelsOfStats[i, j], j, i, true);
                }
            }
            this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[0, 0], "User name");
            this.Invoke(new deleUpdateLabel(UpdateLabel), labelsOfStats[0, 1], "Number of cards");
        }

        public void RemoveCardFromPanel(Card card)
        {
            PictureBox[] temp = new PictureBox[pbs.Length - 1];

            // search the card in the array           
            int cardLocation = 0;
            Boolean found = false;
            while (!found)
            {
                string[] info = pbs[cardLocation].Name.Split('_');
                // check if the removed card is the one in the array
                if ((card.GetValue().ToString().Equals(info[0]) && card.GetColor().Equals(info[1])))
                {
                    found = true;
                }
                else
                {
                    cardLocation++;
                }
            }

            // remove the card from the panel
            cardPanel.Controls.Remove(pbs[cardLocation]);

            // shift all cards one location to the left
            for (int i = cardLocation; i < pbs.Length - 1; ++i)
            {
                pbs[i] = pbs[i + 1];
                pbs[i].Location = new System.Drawing.Point(120 * i - cardPanel.HorizontalScroll.Value, 0);
            }

            // resize the array
            Array.Resize(ref pbs, pbs.Length - 1);
        }


        public Point MouseDownLocation { get; private set; }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void PictureBox2_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void PictureBox2_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void PictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;

            //pictureBox2.Invalidate();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            ClearTable();
            Reset();
            clientManager.GetWaitForm().SetVisibleManagerButtons(false);
            clientManager.ClosePlay();
        }
    }
}
