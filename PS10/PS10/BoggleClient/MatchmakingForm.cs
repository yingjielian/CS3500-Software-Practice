// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// Partial class containing methods and events for manipulating 
    /// the MatchmakingForm window. Implements the IConnectable interface.
    /// </summary>
    public partial class MatchmakingForm : Form, IConnectable
    {
        /// <summary>
        /// To track closing the form by clicking X button
        /// </summary>
        private bool closing;

        /// <summary>
        /// Provides the event when the user decides to join a new 
        /// game. The parameters are server name, player name, 
        /// and the time limit.
        /// </summary>
        public event Action<string, string, int> CreateGameRequest;

        /// <summary>
        /// Provides the event when the user decides to cancel the 
        /// current request to join a game.
        /// </summary>
        public event Action CancelJoinRequest;

        /// <summary>
        /// Displays a MessageBox with the assigned text.
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// Sets the text of the Status label. Thread safe, 
        /// </summary>
        public string Status
        {
            set {  Invoke((Action)(() => { statusLabel.Text = value; })); }
        }

        /// <summary>
        /// Deafult constructor that initializes this form.
        /// </summary>
        public MatchmakingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fires an event when the Find Game button is clicked. 
        /// If the time limit or the user name is invalid, notifies the 
        /// user.
        /// </summary>
        /// <param name="sender">The object on which this event was invoked</param>
        /// <param name="e">Default event arguments</param>
        private void findGameButtonClicked(object sender, EventArgs e)
        {
            int timeLimit;
            if(!Int32.TryParse(gameDurationTextBox.Text, out timeLimit))
            {
                Message = "Please enter a valid time limit";
                return;
            }
            if(string.IsNullOrWhiteSpace(playerNameTextBox.Text))
            {
                Message = "Please enter a valid player name";
                return;
            }

            // Start the progress bar and disable/enable buttons
            progressBar.MarqueeAnimationSpeed = 30;
            findGameButton.Enabled = false;
            cancelButton.Enabled = true;

            if(CreateGameRequest != null)
            {
                // Lanuch game request on a seperate thread to keep GUI responsive.
                Task task = new Task(() => CreateGameRequest(serverNameTextBox.Text, playerNameTextBox.Text, timeLimit));
                task.Start();
            }
        }

        /// <summary>
        /// Fires the CancelJoinRequest event when the Cancel button 
        /// is clicked. 
        /// </summary>
        /// <param name="sender">The object on which this event was invoked</param>
        /// <param name="e">Default event arguments</param>
        private void cancelButtonClicked(object sender, EventArgs e)
        {

            progressBar.MarqueeAnimationSpeed = 0;
            findGameButton.Enabled = true;
            cancelButton.Enabled = false;
            if(CancelJoinRequest != null)
            {
                CancelJoinRequest();
            }
        }

        /// <summary>
        /// Hides this form, resets its components and opens a new 
        /// Boggle game window. Thread Safe.
        /// </summary>
        /// <param name="game">Contains the state of the current game</param>
        public void DoClose(BoggleGame game)
        {
            Invoke((Action)(() =>
            {
                BoggleForm gui = new BoggleForm();
                BoggleController gameController = new BoggleController(gui, game);
                gui.Present();
                Hide();
                ResetUI();
            }));
        }

        /// <summary>
        /// Resets all the components in this form.
        /// </summary>
        public void ResetUI()
        {
            serverNameTextBox.Text = "";
            playerNameTextBox.Text = "";
            gameDurationTextBox.Text = "";
            progressBar.MarqueeAnimationSpeed = 0;
            statusLabel.Text = "Idle";
            cancelButton.Enabled = false;
            findGameButton.Enabled = true;
        }

        /// <summary>
        /// Deals with the act of clicking the X button to close 
        /// this Matchmaking window. When clicked, quits this 
        /// entire application.
        /// </summary>
        private void MatchmakingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!closing)
            {
                Application.Exit();
            }
            closing = true;           
        }
    }
}
