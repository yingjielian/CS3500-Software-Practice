// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;

namespace BoggleClient
{
    /// <summary>
    /// Controllable interface of the BoggleForm
    /// </summary>
    public interface IBoggleForm
    {
        /// <summary>
        /// Sends the word to be played
        /// </summary>
        event Action<string> PlayWordEvent;

        /// <summary>
        /// Closes the current game and opens the matchmaker. 
        /// </summary>
        event Action OpenMatchMakerEvent;

        /// <summary>
        /// Provides the event for the conclusion of the game. 
        /// </summary>
        event Action ShowResultsEvent;

        /// <summary>
        /// Event fired to indicate we need to refresh the game.
        /// </summary>
        event Action RefreshGameEvent;

        /// <summary>
        /// Sets the label of the users nickname
        /// </summary>
        string UserNickname { set; }
       
        /// <summary>
        /// Sets the label of the opponents username
        /// </summary>
        string OpponentNickname { set; }

        /// <summary>
        /// Sets the label for the users score
        /// </summary>
        string UserScore { set; }

        /// <summary>
        /// Sets the label for the opponent score
        /// </summary>
        string OpponentScore { set; }

        /// <summary>
        /// Sets the lables of all the buttons
        /// </summary>
        string Board { set; }

        /// <summary>
        /// Sets the label for the time left
        /// </summary>
        int TimeLeft { set; }

        /// <summary>
        /// Handles closing a boggle game.
        /// </summary>
        void FinishGame(BoggleGame game);

        /// <summary>
        /// Closes the current Boggle window
        /// </summary>
        void DoClose();

        /// <summary>
        /// Presents the gui.
        /// </summary>
        void Present();
    }
}
