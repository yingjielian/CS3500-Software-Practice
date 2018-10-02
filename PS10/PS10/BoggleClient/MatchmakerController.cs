// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient
{
    /// <summary>
    /// Provides a Controller for the Matchmaker form. The model and view do not 
    /// know about the state in the Controller. The Controller accesses the 
    /// MatchmakingForm (View) via an interface (IConnectable).
    /// </summary>
    class MatchmakerController
    {
        /// <summary>
        /// Contains the window being controlled, accessing view via 
        /// the interface.
        /// </summary>
        private IConnectable window;

        /// <summary>
        /// Contains the backing Matchmaker data structure (model).
        /// </summary>
        private BoggleApi api;

        /// <summary>
        /// Creates a controller which connects a GUI to a matchmaker. 
        /// Also registers the event handlers.
        /// </summary>
        /// 
        /// <param name="window">The currently active Matchmaker window. </param>
        public MatchmakerController(IConnectable window)
        {
            this.window = window;
            window.CreateGameRequest += HandleGameRequest;
            window.CancelJoinRequest += HandleCancelRequest;
        }

        /// <summary>
        /// It handles the request by the user to create a new game. If the game was 
        /// created, waits for the other player to join. Once joined, terminates the 
        /// Matchmaking process and begins the Boggle game.
        /// </summary>
        /// 
        /// <param name="serverName">Name of the server</param>
        /// <param name="userName">The player name</param>
        /// <param name="timeLimit">Requested time limit for the game</param>
        private void HandleGameRequest(string serverName, string userName, int timeLimit)
        {
            HandleStatusUpdate("Starting matchmaking...");
            // Create a new Matchmaker and allow it to send back updates along the way
            api = new BoggleApi(serverName, userName, timeLimit);
            api.StatusUpdate += HandleStatusUpdate;

            // Initiate the game request
            int statusCode = api.CreateRequest();

            // If the game was "Created", wait for the other player to join. Once the 
            // game is "Accepted", terminate the currently active Matchmaking window and 
            // begin the boggle game.
            if((api.HasCreatedGame && statusCode == 201 && !api.cancel) || 
                (api.HasCreatedGame && statusCode == 202 && !api.cancel))
            {
                dynamic game = api.GetGameRequest();
                if (statusCode == 202 || game.GameState == "pending")
                {
                    // Keep refreshing the game waiting for another player to join
                    while (!api.cancel && game.GameState != "active")
                    {
                        game = api.GetGameRequest();
                    }
                }
                // Don't open the game if we are to cancel.
                if(!api.cancel)
                {
                    BoggleGame newGame = new BoggleGame(game, api.UserName);
                    window.DoClose(newGame);
                }
            }
        }

        /// <summary>
        /// Handles the event of cancelling a pending quest to join 
        /// the game. Updates the status on the GUI appropriately.
        /// </summary>
        private void HandleCancelRequest()
        {
            if( api != null )
            {
                HandleStatusUpdate("Cancelling game...");
                api.cancel = true;
                int statusCode = api.CancelRequest();

                // 200 == OK
                if(statusCode == 200)
                {
                    HandleStatusUpdate("Cancelled game. <idle>");
                }
                else if(statusCode == 403)
                {   // 403, user is not in a pending game
                    HandleStatusUpdate("Already cancelled. <idle>");
                }
            }
        }

        /// <summary>
        /// Helper method to set the status label in the GUI.
        /// </summary>
        /// <param name="updateMessage">The text of the label</param>
        private void HandleStatusUpdate(string updateMessage)
        {
            window.Status = updateMessage;
        }
    }
}
