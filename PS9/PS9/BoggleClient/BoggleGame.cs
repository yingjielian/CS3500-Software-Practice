// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient
{
    /// <summary>
    /// Represents a playable boggle game
    /// </summary>
    public class BoggleGame
    {
        /// <summary>
        /// State of the game. "active", "pending", "completed"
        /// </summary>
        public string GameState { private set; get; }
        
        /// <summary>
        /// Original time limit set on the game.
        /// </summary>
        public int TimeLimit { private set; get; }
        
        /// <summary>
        /// Time left in this game
        /// </summary>
        public int TimeLeft { private set; get; }
        
        /// <summary>
        /// The user
        /// </summary>
        public BogglePlayer HomePlayer { private set; get; }
        
        /// <summary>
        /// The remote player
        /// </summary>
        public BogglePlayer OpponentPlayer { private set; get; }
        
        /// <summary>
        /// A string representing the board.
        /// </summary>
        public string Board { private set; get; }

        /// <summary>
        /// Contains all the words played by this user.
        /// </summary>
        public List<string> HomePlayerWordsPlayedList { private set; get; }

        /// <summary>
        /// Contains all the points scored per word play by this user
        /// </summary>
        public List<string> HomePlayerScoresList { private set; get; }

        /// <summary>
        /// Contains all the words played by the opponent.
        /// </summary>
        public List<string> OpponentWordsPlayedList { private set; get; }

        /// <summary>
        /// Contains all the points scored per word play by the opponent
        /// </summary>
        public List<string> OpponentScoresList { private set; get; }

        /// <summary>
        /// Constructs a BoggleGame with the state of the whole game received 
        /// from a web request, and a nick name of the home player. </summary>
        /// 
        /// <param name="game">Game state returned by the web service</param>
        /// <param name="homePlayerUserToken">User token for the home player</param>
        /// <param name="homePlayerNickname">Nick name for the home player</param>
        public BoggleGame(dynamic game, string homePlayerNickname)
        {
            GameState = game.GameState;
            // Only fill game information if it is either "active" or "completed"
            if (GameState != "pending")
            {
                Board = game.Board;
                TimeLimit = game.TimeLimit;
                TimeLeft = game.TimeLeft;
                HomePlayerWordsPlayedList = new List<string>();
                HomePlayerScoresList = new List<string>();
                OpponentWordsPlayedList = new List<string>();
                OpponentScoresList = new List<string>();

                // Figure out if the home player is the first or second player in the game.
                if (game.Player1.Nickname == homePlayerNickname)
                {
                    HomePlayer = new BogglePlayer(game.Player1);
                    OpponentPlayer = new BogglePlayer(game.Player2);
                }
                else
                {
                    HomePlayer = new BogglePlayer(game.Player2);
                    OpponentPlayer = new BogglePlayer(game.Player1);
                }
            }
        }

        /// <summary>
        /// Plays the passed word.
        /// </summary>
        /// <param name="word">Word to be played</param>
        /// <returns>The score of the word play</returns>
        public int PlayWord(string word)
        {
            // username and time limit is not required when playing a word.
            BoggleApi api = new BoggleApi(BoggleApi.ServerName, "", 0);
            int score = api.PlayWord(word);
            HomePlayer.Score += score;
            return score;
        }

        /// <summary>
        /// Gets the current status of the game.
        /// </summary>
        /// <returns>The dynamic object for the game.</returns>
        public dynamic RefreshGame()
        {
            BoggleApi api = new BoggleApi(BoggleApi.ServerName, "", 0);
            return api.GetGameRequest();
        }

        /// <summary>
        /// Fills the lists of the words played and the points scored per 
        /// word-play by both the players. Takes into account which 
        /// player was considered to be player1 and player2 by the server 
        /// before making the lists for the home player and the opponent.
        /// </summary>
        /// 
        /// <param name="game"> Contains the entire state of the game as 
        /// received from the server. </param>
        public void ComputeFinalWordPlayed(dynamic game)
        {
            dynamic homePlayerWordsPlayed;
            dynamic opponentPLayerWordsPlayed;
            string player1Nickname = game.Player1.Nickname;

            // Home player (user) is player1
            if (HomePlayer.Nickname == player1Nickname)
            {
                homePlayerWordsPlayed = game.Player1.WordsPlayed;
                opponentPLayerWordsPlayed = game.Player2.WordsPlayed;
            }

            // Home player (user) is player2
            else
            {
                homePlayerWordsPlayed = game.Player2.WordsPlayed;
                opponentPLayerWordsPlayed = game.Player1.WordsPlayed;
            }

            // Make the lists of words played and the points score for 
            // each of those word-play by both the players.
            if(game.GameState == "completed" || game.GameState == "active")
            {
                foreach (dynamic word in homePlayerWordsPlayed)
                {
                    HomePlayerWordsPlayedList.Add(word.Word.ToString());
                    HomePlayerScoresList.Add(word.Score.ToString());
                }

                foreach (dynamic word in opponentPLayerWordsPlayed)
                {
                    OpponentWordsPlayedList.Add(word.Word.ToString());
                    OpponentScoresList.Add(word.Score.ToString());
                }
            }
        }
    }
}