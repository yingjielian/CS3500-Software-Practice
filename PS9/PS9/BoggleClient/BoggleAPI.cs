// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace BoggleClient
{
    /// <summary>
    /// Represents the model behind the Matchmaker responsible 
    /// for interacting with the server through the BoggleAPI. 
    /// Handles making requests to join a player into a game and 
    /// receiving responses from the server.
    /// </summary>
    class BoggleApi
    {
        /// <summary>
        /// Contains the name of the boggle sever
        /// </summary>
        public static string ServerName { private set; get; }

        /// <summary>
        /// Contains the name of the player requesting to join a game.
        /// </summary>
        public string UserName { private set; get; }
       
        /// <summary>
        /// Contains the duration of the game in seconds
        /// </summary>
        private int gameDuration;

        /// <summary>
        /// Indicates the user's act of cancelling the request to join a game.
        /// </summary>
        public bool cancel;

        /// <summary>
        /// Contains the user token responded by the server after 
        /// making the request
        /// </summary>
        public static string UserToken { private set; get; }

        /// <summary>
        /// Contains the gameID of the game being joined (sent by the server).
        /// </summary>
        public static string GameId;

        /// <summary>
        /// An event which is to be fired when the progress or status of the 
        /// match making needs to be changed. It takes in a status message as 
        /// a string.
        /// </summary>
        public event Action<string> StatusUpdate;

        /// <summary>
        /// To track whether or not the game was created based on the server 
        /// response.
        /// </summary>
        public bool HasCreatedGame { private set; get; }

        /// <summary>
        /// Creates a matchmaker and initliazes its state so that it can request 
        /// to join a game. </summary>
        /// 
        /// <param name="serverName">The name of the boggle sever</param>
        /// <param name="userName">The player name</param>
        /// <param name="gameDuration">The duration of the game</param>
        public BoggleApi(string serverName, string userName, int gameDuration)
        {
            ServerName = serverName;
            this.UserName = userName;
            this.gameDuration = gameDuration;
        }

        /// <summary>
        /// Creates an HttpClient for communicating with the boggle API. 
        /// Also updates the status when the user enters a bad URL.
        /// </summary>
        /// <returns>An HttpClient pointed toward the boggle sever</returns>
        private HttpClient CreateClient()
        {
            HttpClient client = null;
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(ServerName);
            }
            catch (Exception)
            {
                StatusUpdate("Bad URL, Idle");
                cancel = true;
            }
            return client;
        }

        /// <summary>
        /// Creates a game request with the info passed in the Matchmaker constructor 
        /// and returns the status code received as a response from the server. It first 
        /// creates a new user request, followed by a request to join a game.
        /// </summary>
        /// 
        /// <returns>
        /// 403 if the provided timelimit is bad or if the nickname was invalid (Forbidden). 
        /// 409 if the user is already in a pending game (Conflict). 
        /// 201 if a new active game was created (game ready to start now). 
        /// 202 if the player joined a game (still need a player to become active).
        /// </returns>
        public int CreateRequest()
        {
            cancel = false;
            int statusCode = -1;

            // Make a Create User request to the server and return the bad status code 
            // if an invalid nickname was entered.
            statusCode = createUserRequest(UserName);
            if(statusCode == 403)       
            {
                return statusCode;
            }
         
            // Send a Join Game request to the server with the appropriate parameters 
            // and update the status. Return the status code received from the server 
            // after joining the game.
            statusCode = joinGameRequest(UserToken, gameDuration);
            if(statusCode == 201 & !cancel)
            {
                StatusUpdate("Game created, starting game");
            }
            else if(statusCode == 202 & !cancel)
            {
                StatusUpdate("Game created, awaiting other player...");
            } else if(statusCode == 403)
            {
                StatusUpdate("Invalid time limit, Idle");
                
            }
            return statusCode;      
        }

        /// <summary>
        /// Cancels the current request to join a game. 
        /// </summary>
        /// 
        /// <returns>
        /// 200 if the player was removed from the game (OK).
        /// 403 if player not in a pending game or invalid (Forbidden).
        /// </returns>
        public int CancelRequest()
        {
            cancel = true;
            return cancelJoinRequest(UserToken);
        }

        /// <summary>
        /// A helper method that POSTS a request to the server via the 
        /// BoggleAPI to create a new player. It passes the Nickname of the 
        /// player to be joined as a Json paramter in the body of the request.
        /// Also returns the status code sent by the server. </summary>
        /// 
        /// <param name="userName">The nickname of the player wanting to join 
        /// a game</param>
        /// 
        /// <returns>
        /// 403 if the Nickname was invalid (Forbidden).
        /// 201 if the user was successfully created (OK).
        /// 0 if the user clicked Cancel button while the request was being 
        /// made.
        /// </returns>
        private int createUserRequest(string userName)
        {
            // If the user did not cancel the match making request, 
            // proceed to make the request.
            if (!cancel)
            {
                using (HttpClient client = CreateClient())
                {
                    // Set the POST data
                    dynamic data = new ExpandoObject();
                    data.Nickname = userName;

                    // Serialize parameter object into the body of the request
                    StringContent content = new StringContent(JsonConvert.SerializeObject(data), 
                                                               Encoding.UTF8, "application/json");

                    // Try getting a response from the POST request while handling exceptional 
                    // conditions.
                    HttpResponseMessage response = null;
                    try
                    {
                        response = client.PostAsync("/BoggleService.svc/users", content).Result;
                    } catch(Exception)
                    {
                        StatusUpdate("Bad URL, Idle");
                        cancel = true;
                        return 403;
                    }

                    // If the user creation was successful, store the user token. Else, 
                    // if failed, cancel the request. Returns the status code either way.
                    if (response.IsSuccessStatusCode)
                    {
                        String result = response.Content.ReadAsStringAsync().Result;
                        dynamic newUser = JsonConvert.DeserializeObject(result);

                        UserToken = newUser.UserToken;
                        // Don't need to ensure the cancel was sent becuase we are not creating a game.
                        if(!cancel)
                        {
                            StatusUpdate("Created user");
                        }
                        return (int)response.StatusCode;
                    }
                    else
                    {
                        StatusUpdate(response.ReasonPhrase);
                        CancelRequest();
                        return (int)response.StatusCode;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// A helper method that POSTS a request to the server via the 
        /// BoggleAPI to join a game. It passes the user token and the game 
        /// duration as a Json paramter in the body of the request.
        /// Also returns the status code received from the server. </summary>
        /// 
        /// <param name="userToken">Current player identification token</param>
        /// <param name="gameDuration">The duration of the game entered by the user</param>
        /// 
        /// <returns>
        /// 403 if the User token or the time limit was invalid (Forbidden).
        /// 409 if the player was already in a pending game (Conflict).
        /// 201 if a new active game was created (Created). 
        /// 202 if the player joined a game (still need a player to become active).
        /// 0 if the user clicked Cancel button while the request was being 
        /// made.
        /// </returns>
        private int joinGameRequest(string userToken, int gameDuration)
        {
            // If the user did not cancel the match making request, 
            // proceed to make the request.
            if (!cancel)
            {
                using (HttpClient client = CreateClient())
                {
                    // Set the POSt data
                    dynamic data = new ExpandoObject();
                    data.UserToken = userToken;
                    data.TimeLimit = gameDuration;

                    // Serialize parameter object into body of the request
                    StringContent content = new StringContent(JsonConvert.SerializeObject(data), 
                                                                Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("/BoggleService.svc/games", 
                                                                        content).Result;

                    // If the game was successfully created (or accepted), store the game ID. Else, 
                    // if failed, cancel the request. Returns the status code either way.
                    if (response.IsSuccessStatusCode)
                    {
                        String result = response.Content.ReadAsStringAsync().Result;
                        dynamic newGame = JsonConvert.DeserializeObject(result);

                        GameId = newGame.GameID;
                        HasCreatedGame = true;
                        return (int)response.StatusCode;
                    }
                    else
                    {
                        StatusUpdate(response.ReasonPhrase);
                        CancelRequest();
                        return (int)response.StatusCode;
                    }
                } 
            }
            return 0;
        }

        /// <summary>
        /// Gets the status information of this game (identified by the game id).
        /// </summary>
        /// 
        /// <returns>
        /// If successful, the dynamic ExpandoObject containing the game status 
        /// information as received from the server.
        /// Else, an empty ExpandoObject.
        /// </returns>
        public dynamic GetGameRequest()
        {
            // If the user did not cancel the match making request, 
            // proceed to make the request.
            if (!cancel)
            {
                using (HttpClient client = CreateClient())
                {
                    string requestParameter = "BoggleService.svc/games/" + GameId;
                    HttpResponseMessage response = client.GetAsync(requestParameter).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        String result = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject(result);
                    }
                }
            }
            return new ExpandoObject();
        }

        /// <summary>
        /// A helper method that sends a request to cancel the pending request to 
        /// join a game to the server via the BoggleAPI. It passes the user token 
        /// as a Json paramter in the body of the request.
        /// 
        /// Also returns the status code received from the server. </summary>
        /// 
        /// <param name="userToken">The token identifying the current player</param>
        /// <returns>
        /// 403 if invalid user token (Forbidden).
        /// 200 if cancellation was successful (OK).
        /// </returns>
        private int cancelJoinRequest(string userToken)
        {
            if(UserToken == null || GameId == null)
            {
                return 403;
            }

            using (HttpClient client = CreateClient())
            {
                // Set the data and serialize parameter object into the body of the request
                dynamic data = new ExpandoObject();
                data.UserToken = userToken;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), 
                                                            Encoding.UTF8, "application/json");

                // Try getting a response from the PUT request while handling exceptional 
                // conditions.
                HttpResponseMessage response = null;
                try
                {
                    response = client.PutAsync("/BoggleService.svc/games", content).Result;
                }
                catch(Exception)
                {
                    // Reset the state of this application and return the status code received.
                    userToken = "";
                    GameId = "";
                    HasCreatedGame = false;
                    return 403;
                }
                return (int)response.StatusCode;
            }
        }

        /// <summary>
        /// Makes a PUT request to play the passed word.
        /// </summary>
        /// <param name="word">The word to be played</param>
        /// <returns>The score for the word</returns>
        public int PlayWord(string word)
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.UserToken = UserToken;
                data.Word = word;
                string url = String.Format("BoggleService.svc/games/{0}", GameId);
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), 
                                                            Encoding.UTF8, "application/json");

                // Try getting a response from the PUT request and return the 
                // status code received.
                HttpResponseMessage response = client.PutAsync(url, content).Result;
                if(response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic scoreObject = JsonConvert.DeserializeObject(result);
                    string score = scoreObject.Score;
                    return Int32.Parse(score);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
