using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 4.24.2018
/// </summary>
namespace Boggle
{
    /// <summary>
    /// Contains the username of the player
    /// </summary>
    public class UserInfo
    {
        public string Nickname { get; set; }
    }

    /// <summary>
    /// Contains the usertoken
    /// </summary>
    public class Token
    {
        public string UserToken { get; set; }
    }

    /// <summary>
    /// Contains the usertoken of the user and
    /// the desired timelimit
    /// </summary>
    public class JoiningAGame
    {
        public string UserToken { get; set; }

        public string TimeLimit { get; set; }
    }

    /// <summary>
    /// Contains the GameId of a game.
    /// </summary>
    [DataContract]
    public class TheGameID
    {
        [DataMember]
        public string GameID { get; set; }

        [IgnoreDataMember]
        public string TimeLimit { get; set; }
    }

    /// <summary>
    /// Contains a pending game
    /// </summary>
    [DataContract]
    public class PendingGame
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Player1Token { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Player2Token { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GameId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TimeLimit { get; set; }
    }


    /// <summary>
    /// Contains the score of the word played.
    /// </summary>
    public class TheScore
    {
        public int Score { get; set; }
    }

    /// <summary>
    /// Contains the word to be played.
    /// </summary>
    public class PlayWord
    {
        public string UserToken { get; set; }

        public string Word { get; set; }
    }

    /// <summary>
    /// Represents a word that has already been played.
    /// </summary>
    public class AlreadyPlayedWord
    {
        public string Word { get; set; }

        public int Score { get; set; }
    }

    /// <summary>
    /// Contains the statistics of any given game.
    /// </summary>
    [DataContract]
    public class GameStatus
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        [DataMember]
        public int TimeLeft { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public FirstPlayer Player1;

        [DataMember(EmitDefaultValue = false)]
        public SecondPlayer Player2;

        [IgnoreDataMember]
        public DateTime datetime { get; set; }

        [IgnoreDataMember]
        public List<AlreadyPlayedWord> Player1Words = new List<AlreadyPlayedWord>();

        [IgnoreDataMember]
        public List<AlreadyPlayedWord> Player2Words = new List<AlreadyPlayedWord>();
    }

    [DataContract]
    public class FirstPlayer
    {
        [DataMember(EmitDefaultValue = false)]
        public string Nickname { get; set; }


        [DataMember]
        public int Score { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AlreadyPlayedWord> WordsPlayed;
    }

    [DataContract]
    public class SecondPlayer
    {
        [DataMember(EmitDefaultValue = false)]
        public string Nickname { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AlreadyPlayedWord> WordsPlayed;
    }
}