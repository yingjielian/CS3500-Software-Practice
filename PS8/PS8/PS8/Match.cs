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
    /// This is GUI class for match making.
    /// </summary>
    public partial class Match : Form
    {
        public Match()
        {
            InitializeComponent();
        }

        private HttpClient client;


        private Timer timer;

        /// <summary>
        /// A unique identifying token
        /// </summary>
        private string usertoken;

        /// <summary>
        /// Nickname, game ID
        /// </summary>                
        private string gameId;


        private BoggleGame board;


        private bool Cancel { get; set; }


        /// <summary>
        /// Creates the players in the game
        /// </summary>
        private void CreateUser()
        {
            HttpContent content = new StringContent("{ \"Nickname\":\"" + UserText.Text + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("/BoggleService.svc/users", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                dynamic token = JsonConvert.DeserializeObject(result);
                usertoken = token.UserToken;
            }
            else
            {
                MessageBox.Show("Please enter a Nickname");
            }


        }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        private void CreateGame()
        {
            if (Cancel)
            {
                return;
            }

            int time;

            if (!int.TryParse(TimeText.Text, out time))
            {
                MessageBox.Show("Please enter a Game Duration.");
                return;
            }

            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + time + "}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("/BoggleService.svc/games", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string responsecode = response.StatusCode.ToString();
                if (!responsecode.Equals("Conflict"))
                {
                    string result = response.Content.ReadAsStringAsync().Result;

                    dynamic token = JsonConvert.DeserializeObject(result);
                    gameId = (string)token.GameID;
                    timer = new Timer();
                    timer.Interval = 1000;
                    timer.Tick += new EventHandler(StartGame);
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("Player is already to request to play the boggle");
                }
            }
            else if (time > 120 || time < 5)
            {
                MessageBox.Show("Enter a time bewteen 5s to 120s");
            }
        }
        /// <summary>
        /// Request to start a new game.
        /// </summary>
        private void StartGame(object sender, EventArgs e)
        {
            HttpResponseMessage response = client.GetAsync("/BoggleService.svc/games/" + gameId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if (result.Contains("active"))
            {
                timer.Stop();
                board = new BoggleGame(client, gameId, usertoken, this);
                board.Show();
            }


        }

        /// <summary>
        /// Fires when Play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            playbutton.Enabled = false;
            Cancel = false;
            CancelButton.Enabled = true;
            client = new HttpClient();
            if (ServerText.Text == "")
            {
                MessageBox.Show("Please enter the server name to request a game");
                playbutton.Enabled = true;
                return;
            }
            client.BaseAddress = new Uri(ServerText.Text);

            if (usertoken == null)
            {
                CreateUser();
            }


            //create game if usertoken is returned
            if (usertoken != null | !Cancel)
            {
                CreateGame();

            }
        }


        /// <summary>
        /// Fires when cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelButton_Click(object sender, EventArgs e)
        {
            Cancel = true;
            CancelButton.Enabled = false;
            playbutton.Enabled = true;
            if (timer != null)
            {
                timer.Stop();
            }
            if (client != null && usertoken != null)
            {
                string json = "{ \"UserToken\":\"" + usertoken + "}";
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.PutAsync("/BoggleService.svc/games/", content);

            }

            if (board != null)
            {
                board.Hide();
                board = null;
            }

            usertoken = null;
            this.Show();

        }


        private void StartForm_Load(object sender, EventArgs e)
        {
            //Not implemented
        }


        /// <summary>
        /// Displays the help menu message box when help is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenu_Click(object sender, EventArgs e)
        {
            StringBuilder helpmessage = new StringBuilder();
            helpmessage.AppendLine("This is a Boggle client to let play with other person ");
            helpmessage.AppendLine("Enter a server name, your nickname, and set up a duration bewteen 5 to 120");
            helpmessage.AppendLine("Then click play to request a game, or click cancel to cancel the request");
            helpmessage.AppendLine("Have fun for the Boggle!");
            String help = helpmessage.ToString();
            MessageBox.Show(help);
        }
    }
}

