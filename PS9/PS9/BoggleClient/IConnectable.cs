// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;

namespace BoggleClient
{
    /// <summary>
    /// The interface providing access to the view (MatchmakingForm) 
    /// for the MatchmakerController.
    /// </summary>
    public interface IConnectable
    {
        /// <summary>
        /// Provides a game creation event. The parameters are 
        /// the server name, player name, game duration.
        /// </summary>
        event Action<string, string, int> CreateGameRequest;

        /// <summary>
        /// Provides a user join cancellation event.
        /// </summary>
        event Action CancelJoinRequest;

        /// <summary>
        /// Provides a message for the MatchmakingForm
        /// </summary>
        string Message { set; }

        /// <summary>
        /// Provides the current match making status message.
        /// </summary>
        string Status { set; }

        /// <summary>
        /// Closes this MatchmakingForm and opens the passed game in a BoggleForm.
        /// </summary>
        void DoClose(BoggleGame game);
    }
}
