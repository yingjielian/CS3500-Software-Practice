// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// Provides a controller which interfaces between the 
    /// model and the view of the BoggleClient.
    /// </summary>
    public class BoggleController
    {
        /// <summary>
        /// The interface used by this Controller to access or 
        /// manipulate the state of the GUI.
        /// </summary>
        private IBoggleForm window;
        
        /// <summary>
        /// The actual game object of the boggle game (Model).
        /// </summary>
        private BoggleGame game;
        
        /// <summary>
        /// The name of the home player
        /// </summary>
        private string homePlayerName;

        /// <summary>
        /// Constructs a game with a form and a game
        /// </summary>
        /// <param name="window">The interface for the boggle game</param>
        /// <param name="game">The game which corresponds to the GUI</param>
        public BoggleController(IBoggleForm window, BoggleGame game)
        {
            this.window = window;
            this.window.PlayWordEvent += HandlePlayWordEvent;
            this.window.OpenMatchMakerEvent += HandleOpenMatchmakerEvent;
            this.window.ShowResultsEvent += HandleResults;
            this.window.RefreshGameEvent += HandleRefreshGameEvent;
            this.game = game;
            homePlayerName = game.HomePlayer.Nickname;
            // Initialize the UI.
            UpdateUI();
        }

        /// <summary>
        /// Feteches the words played, and opens them in a dialog box.
        /// </summary>
        private void HandleResults()
        {
            BoggleApi api = new BoggleApi(BoggleApi.ServerName, "", 0);
            game.ComputeFinalWordPlayed(api.GetGameRequest());
            window.FinishGame(game);
        }

        /// <summary>
        /// Should play the word in the game, fired when a user plays a word
        /// </summary>
        /// <param name="word">The word to be played</param>
        /// <returns>Returns true if the word was successfully played.</returns>
        public void HandlePlayWordEvent(string word)
        {
            game.PlayWord(word);
            UpdateUI();
        }

        /// <summary>
        /// Closes this game and opens the matchmaker to allow the user 
        /// to play a new game.
        /// </summary>
        public void HandleOpenMatchmakerEvent()
        {
            window.DoClose();
            
            MatchmakingForm matchmakingWindow = new MatchmakingForm();
            MatchmakerController controller = new MatchmakerController(matchmakingWindow);
            matchmakingWindow.Show();
        }

        /// <summary>
        /// Is called every second while the game is active and 
        /// refreshes the UI.
        /// </summary>
        public void HandleRefreshGameEvent()
        {
            dynamic response = game.RefreshGame();
            game = new BoggleGame(response, homePlayerName);
            UpdateUI();
        }

        /// <summary>
        /// Based on the contents of the game sets the labels for 
        /// the GUI through properties.
        /// </summary>
        private void UpdateUI()
        {
            // Only update the UI if the game is either "active" or "completed"
            if(game.GameState != "pending")
            {
                window.UserNickname = game.HomePlayer.Nickname;
                window.OpponentNickname = game.OpponentPlayer.Nickname;
                window.UserScore = game.HomePlayer.Score.ToString();
                window.OpponentScore = game.OpponentPlayer.Score.ToString();
                window.Board = game.Board;
                window.TimeLeft = game.TimeLeft;
            }
        }
    }
}
