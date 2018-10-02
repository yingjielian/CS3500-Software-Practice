// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Text.RegularExpressions;

namespace BoggleClient
{
    public partial class BoggleForm : Form, IBoggleForm
    {
        /// <summary>
        /// Fired when a word is played by the user. The parameter is 
        /// the word being played.
        /// </summary>
        public event Action<string> PlayWordEvent;
        
        /// <summary>
        /// An event that closes this window and opens a 
        /// MatchmakingForm window to join a new game.
        /// </summary>
        public event Action OpenMatchMakerEvent;

        /// <summary>
        /// Provides the event for the conclusion of the game. 
        /// </summary>
        public event Action ShowResultsEvent;

        /// <summary>
        /// Fired each second to refresh the display.
        /// </summary>
        public event Action RefreshGameEvent;

        /// <summary>
        /// Sets the first player name label. Does not need to be thread safe 
        /// becuase it is only set exactly once.
        /// </summary>
        public string UserNickname
        {
            set { player1NameLabel.Text = value; }
        }

        /// <summary>
        /// Sets the second player name label. Does not need to be thread 
        /// safe becuase it is only set exactly once.
        /// </summary>
        public string OpponentNickname
        {
            set { player2NameLabel.Text = value; }
        }

        /// <summary>
        /// Sets the first player's score. Thread safe.
        /// </summary>
        public string UserScore
        {
            set
            {
                if (player1ScoreLabel.InvokeRequired)
                {
                    Invoke((Action)(() =>
                    {
                        player1ScoreLabel.Text = value;
                    }));
                }
                else
                {
                    player1ScoreLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// Sets the second player's score. Thread safe.
        /// </summary>
        public string OpponentScore
        {
            set
            {
                if (player2ScoreLabel.InvokeRequired)
                {
                    Invoke((Action)(() =>
                    {
                        player2ScoreLabel.Text = value;
                    }));
                }
                else
                {
                    player2ScoreLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// Contains the current game board
        /// </summary>
        public string Board
        {
            set { setButtons(value); }
        }

        /// <summary>
        /// Indicates whether the results pane was showed or not.
        /// </summary>
        private bool showedResults;

        /// <summary>
        ///  Sets the text of each button to the corresponding letter 
        ///  from the current board state received from the server. 
        ///  Replaces char Q with QU.
        /// </summary>
        /// <param name="value">string containing the current board 
        /// state.</param>
        private void setButtons(string value)
        {
            Button[] buttons = new Button[16];
            buttons[0] = button1;
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button5;
            buttons[5] = button6;
            buttons[6] = button7;
            buttons[7] = button8;
            buttons[8] = button9;
            buttons[9] = button10;
            buttons[10] = button11;
            buttons[11] = button12;
            buttons[12] = button13;
            buttons[13] = button14;
            buttons[14] = button15;
            buttons[15] = button16;

            for(int i = 0; i < buttons.Length; i++)
            {
                Button b = buttons[i];

                if (b.InvokeRequired)
                {
                    Invoke((Action)(() =>
                    {
                        b.Text = value[i].ToString();
                        if (b.Text == "Q")
                        {
                            b.Text = "QU";
                        }
                    }));
                }
                else
                {
                    b.Text = value[i].ToString();
                    if (b.Text == "Q")
                    {
                        b.Text = "QU";
                    }
                }
            }
        }

        /// <summary>
        /// Represents the time left in the current game.
        /// </summary>
        public int TimeLeft
        {
            set
            {
                if (player2ScoreLabel.InvokeRequired)
                {
                    Invoke((Action)(() =>
                    {
                        timerLabel.Text = value.ToString();
                    }));
                }
                else
                {
                    timerLabel.Text = value.ToString();
                }
                if (timer == null)
                {
                    timer = new System.Timers.Timer(1000);
                    timer.Elapsed += HandleTimerElapsedEvent;
                    timer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Countdown game timer
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Initalizes this form.
        /// </summary>
        public BoggleForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;    
        }

        /// <summary>
        /// Provides the countdown timer which runs every 1 second and 
        /// updates the timer label. If the countdown timer runs out, closes 
        /// the current Boggle game window and displays the results. Thread safe.
        /// </summary>
        /// <param name="sender">The object which fired the event</param>
        /// <param name="e">Default elapsed event arguments</param>
        private void HandleTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            int timeLeft = 1;
            if(timerLabel.InvokeRequired)
            {
                Invoke((Action)(() =>
                {
                    timeLeft = Int32.Parse(timerLabel.Text);
                    timeLeft--;
                    TimeLeft = timeLeft;
                }));
            }
            else
            {
                Int32.TryParse(timerLabel.Text, out timeLeft);
                timeLeft--;
                TimeLeft = timeLeft;
            }

            // Coutndown timer has run out
            if (timeLeft <= 0)
            {
                if (ShowResultsEvent != null && !showedResults)
                {
                    showedResults = true;
                    ShowResultsEvent();
                }
            }
            else
            {
                if(RefreshGameEvent != null)
                {
                    RefreshGameEvent();
                }
            }
        }

        /// <summary>
        /// Prompts the user with information on how to play the game.
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Default event arguments</param>
        private void HelpMenuItemClicked(object sender, EventArgs e)
        {
            string text = "To use this Boggle client, enter a word in the Word Play text box "
                + "located at the bottom of the screen and press enter to play it. A sound will be "
                + "played to notify you that the word was successfully played. Work those brain cells "
                + "until the time runs out. At any moment, you can monitor yours and your opponent's "
                + "score on top of the screen, along with the countdown timer displaying the time left. "
                + "You can also leave the game at any moment by clicking the Leave button at the bottom "
                + "right corner of the screen or you can navigate to File -> Close to quit this game "
                + "and return back to player match-making window. \n\nHowever, if you manage to carry on "
                + "until the timer runs out, you will be greeted by a results window showing a list of "
                + "all the words that you and your opponent played along with the individual scores. "
                + "At the bottom, you can also find the total score and at the top of the window, you can "
                + "see who won the game. Once you are done analyzing the results, you can go back to the "
                + "match-making window to start a new game by: \n\n(1) Clicking X button or \n(2) Pressing the "
                + "yellow moral-boosting button at the bottom of the results window. \n\nThe match-making "
                + "window allows you to join a boogle server where the boogle game is being hosted. You should "
                + "also provide the time limit and preferred nickname to begin playing the game. Once an "
                + "opponent is found, the game will begin. You can cancel the request to join a game any "
                + "time by clicking cancel buttom. \n\nTo quit the game entirely, "
                + "just close the match-making window by click X.";
            MessageBox.Show(text, "Gameplay Walthrough");
        }

        /// <summary>
        /// Leaves the current game and opens the Matchmaker. 
        /// Fired when the leave game button is clicked and when the 
        /// close button in the file menu is clicked. </summary>
        /// 
        /// <param name="sender">The object/form which fired this event</param>
        /// <param name="e">Default event arguments</param>
        private void leaveGameButton_Click(object sender, EventArgs e)
        {
            if(OpenMatchMakerEvent != null)
            {
                showedResults = true;
                OpenMatchMakerEvent();
            }
        }

        /// <summary>
        /// Closes this form.
        /// </summary>
        public void FinishGame(BoggleGame game)
        {
            ResultsForm gui = new ResultsForm();
            ResultsController resultsController = new ResultsController(gui, game);
            Invoke((Action)(() =>
            {
                Hide();
                gui.Present();
            }));
        }

        /// <summary>
        /// Closes the current Boggle window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// shows this form.
        /// </summary>
        public void Present()
        {
            Show();
        }

        /// <summary>
        /// Fired when the word play textbox changes and the return key is pressed. 
        /// Fires an event which communicates with the server through the Boggle API 
        /// requesting to play the entered word.
        /// </summary>
        /// <param name="sender">The object/form that fired this event</param>
        /// <param name="e">Arguments containing the key pressed. </param>
        private void wordTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(PlayWordEvent != null)
                {
                    string word = wordPlayTextBox.Text;
                    // Process the play word on a seperate thread to keep the GUI responsive 
                    Task task = new Task(() => PlayWordEvent(word));
                    task.Start();
                    wordPlayTextBox.Text = "";
                }
            }
        }
    }
}
