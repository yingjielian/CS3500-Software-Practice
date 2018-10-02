using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 3.15.2018
/// </summary>
namespace PS8
{
    /// <summary>
    /// This is GUI class for the Boggle Game 
    /// </summary>
    public partial class BoggleGame : Form
    {
        public BoggleGame()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Create the boggle game
        /// </summary>
        /// <param name="client"></param>
        /// <param name="gameId"></param>
        /// <param name="user"></param>
        /// <param name="form"></param>
        public BoggleGame(HttpClient client, string gameId, string user, Match form)
        {
            InitializeComponent();
            closed = false;
            this.client = client;
            this.gameId = gameId;
            this.user = user;
            this.form = form;

            HttpResponseMessage response = client.GetAsync("/BoggleService.svc/games/" + gameId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic token = JsonConvert.DeserializeObject(result);

            Player1.Text = (string)token.Player1.Nickname + " :";
            Player2.Text = (string)token.Player2.Nickname + " :";

            //populate board
            board = (string)token.Board;
            int i = 0;
            foreach (Button s in Buttons.Controls)
            {
                s.Text = board[i++] + "";
                if (s.Text == "Q")
                {
                    s.Text = "QU";
                }
            }
            UpdateBoard();
            time = new Timer();
            time.Interval = 1000;
            time.Tick += new EventHandler(TimerTick);
            time.Start();

            FormClosing += form.CancelButton_Click;
            FormClosing += Closed;
            form.Hide();

        }

        /// <summary>
        /// Client that communicates with server
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Timer for client
        /// </summary>
        private Timer time;

        /// <summary>
        /// Start form for GUI
        /// </summary>
        private Form form;

        /// <summary>
        /// Unique game identifier
        /// </summary>
        private string gameId;

        /// <summary>
        /// Nick name of user
        /// </summary>
        private string user;

        /// <summary>
        /// board string
        /// </summary>
        private string board;

        /// <summary>
        /// True when board is closed. False otherwise.
        /// </summary>
        private Boolean closed;

        /// <summary>
        /// Handler for timer
        /// </summary>
        private void TimerTick(object sender, EventArgs e)
        {
            UpdateBoard();
        }


        /// <summary>
        /// A closed method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closed(object sender, EventArgs e)
        {
            closed = true;
        }
        /// <summary>
        /// This method is used to updated your score when you finish the game.
        /// </summary>
        private void UpdateBoard()
        {
            HttpResponseMessage response = client.GetAsync("/BoggleService.svc/games/" + gameId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic token = JsonConvert.DeserializeObject(result);

            Score1.Text = (string)token.Player1.Score;
            Score2.Text = (string)token.Player2.Score;
            TimeLeft.Text = ((int)token.TimeLeft) / 60 + ":" + ((int)token.TimeLeft) % 60 / 10 + "" + ((int)token.TimeLeft) % 60 % 10;

            if (token.GameState == "completed")
            {
                time.Stop();
                EndGame(token);
            }
        }

        /// <summary>
        /// This is the method for enter the word, it will throw to response.
        /// </summary>
        /// <param name="word"></param>
        private void PlayWord(string word)
        {
            string json = "{ \"UserToken\":\"" + user + "\"," + "\"Word\":\"" + word + "\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync("/BoggleService.svc/games/" + gameId, content).Result;
            TextEntry.Text = "";
        }


        /// <summary>
        /// This a end game result message for when you finish the game, it 
        /// will show result to you.
        /// </summary>
        /// <param name="token"></param>
        private void EndGame(dynamic token)
        {
            if (closed)
            {
                return;
            }
            //Block all buttons
            foreach (Button s in Buttons.Controls)
            {
                s.Enabled = false;
            }
            TextEntry.Enabled = false;
            //Get scores and determine winner
            int s1, s2;
            int.TryParse(Score1.Text, out s1);
            int.TryParse(Score2.Text, out s2);
            string winner = Player1.Text;
            if (s1 < s2)
            {
                winner = Player2.Text;
            }
            //message construction
            string message = "GAME OVER! " + winner + " is the winner!\n";
            message += "\n=======PLAYER1=======\n";
            message += "\n\t  " + Player1.Text + "  :  " + Score1.Text;
            message += "\n========WORDS========\n";
            foreach (dynamic pair in token.Player1.WordsPlayed)
            {
                message += (string)pair.Word + " : ";
                message += (string)pair.Score + "\n";
            }
            message += "\n=======PLAYER2=======\n";
            message += "\n\t  " + Player2.Text + "  :  " + Score2.Text;
            message += "\n========WORDS========\n";
            foreach (dynamic pair in token.Player2.WordsPlayed)
            {
                message += (string)pair.Word + " : ";
                message += (string)pair.Score + "\n";
            }
            //display scores and words
            MessageBox.Show(message);
        }

        private void BoggleG(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This is Text entry to let you press REturn(enter) to enter 
        /// The word into the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEntryClick(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            PlayWord(TextEntry.Text);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder helpmessage = new StringBuilder();
            helpmessage.AppendLine("Enter your answer in the word textbox, and press Return(Enter)");
            helpmessage.AppendLine("If you don't know how to play, you can google the Boggle's rule");
            helpmessage.AppendLine("Have fun!");
            String help = helpmessage.ToString();
            MessageBox.Show(help);
        }
    }
}

