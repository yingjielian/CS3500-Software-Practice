using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 3.29.2018
/// </summary>
namespace Boggle
{
    /// <summary>
    /// Class that holds all the data related to games.  This will be stored in a dicationary and searchable by the relevant GameID.
    /// </summary>
    [DataContract]
    public class GameStatus
    {
        [DataMember]
        public string GameState { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string TimeLimit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string TimeLeft { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public UserInfo Player1 { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public UserInfo Player2 { get; set; }

        public DateTime StartGameTime;

        public BoggleBoard RelevantBoard { get; set; }
    }

    /// <summary>
    /// Class used to store relevant information about the player.  Referenced by the GameStatus class.
    /// </summary>
    [DataContract]
    public class UserInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public string Nickname { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Score { get; set; }

        public string UserToken { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<WordScore> WordsPlayed { get; set; }

        public List<WordScore> personalList { get; set; }

    }

    /// <summary>
    /// Class that is used to return data results from the server to the clients.  Primarily only one property will have data added per instance, and then it will be
    /// returned to the client.
    /// </summary>
    [DataContract]
    public class TokenScoreGameIDReturn
    {
        [DataMember(EmitDefaultValue = false)]
        public string UserToken { get; set; }

        [DataMember(EmitDefaultValue = false)]

        public string GameID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Score { get; set; }
    }

    /// <summary>
    /// Class that is used to take the data from the user for when they are joining a game or creating one
    /// </summary>
    public class GameJoin
    {
        public string UserToken { get; set; }
        public string TimeLimit { get; set; }

    }

    /// <summary>
    /// Class that is used to take data from the user when they are submitting a word to the server for a game.
    /// </summary>
    public class UserGame
    {
        public string Word { get; set; }
        public string UserToken { get; set; }
    }

    /// <summary>
    /// Class that is used for a list so it can be attached to a specific user.
    /// </summary>
    public class WordScore
    {
        public string Word { get; set; }

        public int Score { get; set; }
    }

}