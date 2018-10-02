using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 4.06.2018
/// </summary>
namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Registers a new user.
        /// If Nickname is null, or is empty when trimmed, responds with status 403 (Forbidden).
        /// Otherwise, creates a new user with a unique UserToken and the trimmed Nickname. 
        /// The returned UserToken should be used to identify the user in subsequent requests. 
        /// Responds with status 201 (Created).
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate = "users")]
        Token Register(UserInfo user);

        /// <summary>
        /// Joins a game/Creates a new game
        /// If UserToken is invalid, TimeLimit  5, or TimeLimit  120, responds with status 403 (Forbidden).
        /// Otherwise, if UserToken is already a player in the pending game, responds with status 409 (Conflict).
        /// Otherwise, if there is already one player in the pending game, adds UserToken as the second player. 
        /// The pending game becomes active and a new pending game with no players is created. 
        /// The active game's time limit is the integer average of the time limits requested by the two players. 
        /// Returns the new active game's GameID (which should be the same as the old pending game's GameID). 
        /// Responds with status 201 (Created).
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate = "games")]
        GameId JoinGame(PostingGame game);

        /// <summary>
        /// Cancels the users join request.
        /// If UserToken is invalid or is not a player in the pending game, responds with status 403 (Forbidden).
        /// Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
        /// </summary>
        /// <param name="token"></param>
        [WebInvoke(Method = "PUT", UriTemplate = "games")]
        void CancelJoin(Token token);

        /// <summary>
        /// Play a word in a game.
        /// If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid, or 
        /// if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden).
        /// Otherwise, if the game state is anything other than "active", responds with response code 409 (Conflict).
        /// Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID. 
        /// Returns the score for Word in the context of the game (e.g. if Word has been played before the score is zero). 
        /// Responds with status 200 (OK). Note: The word is not case sensitive.
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "games/{GameID}")]
        WordScore PlayWord(string GameID, PlayedWord word);

        /// <summary>
        /// Get game status information.
        /// If GameID is invalid, responds with status 403 (Forbidden).
        /// Otherwise, returns information about the game named by GameID as illustrated below. 
        /// Note that the information returned depends on whether "Brief=yes" was included as a 
        /// parameter as well as on the state of the game. 
        /// Responds with status code 200 (OK). 
        /// Note: The Board and Words are not case sensitive.
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "games/{GameID}?Brief={Option}")]
        Status Gamestatus(string GameID, string Option);

        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();
    }
}
