using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static System.Net.HttpStatusCode;
using System.Data.SqlClient;

namespace Boggle
{
    public class BoggleService
    {


        private readonly static HashSet<String> Dictionary = The_Dictionary();
        private static bool board = false;
        private static string connectionString;

        //The String DataBase Connection.
        private static string BoggleDB;

        /// <summary>
        /// Connecting With The DataBase.
        /// </summary>
        /// <returns></returns>
        static BoggleService()
        {
            string dbFolder = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            connectionString = String.Format(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {0}\BoggleDB.mdf; Integrated Security = True", dbFolder);
        }


        public Stream API(out HttpStatusCode status)
        {
            status = OK;
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }



        /// <summary>
        /// Creates A New User And Set The Status If Meets Any Situation.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token CreateAUser(UserInfo user, out HttpStatusCode status)
        {

            //Forbbidden Status
            if (user.Nickname == null || user.Nickname.Trim().Length == 0)
            {
                status = Forbidden;
                return null;
            }

            //Connection With DataBase
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Open The DataBase.
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("insert into Users (UserID, Nickname) values (@UserID, @Nickname)", conn, trans))
                    {
                        string UserID = Guid.NewGuid().ToString();

                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Nickname", user.Nickname);

                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.ExecuteNonQuery();
                        }

                        status = Created;
                        trans.Commit();

                        return new Token() { UserToken = UserID };
                    }
                }
            }
        }

        /// <summary>
        /// Joins A Pending Game Or Creates A New Game If Needed. It Also Sets Possible Status
        /// If Needed.
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        public TheGameID JoinGame(JoiningAGame join, out HttpStatusCode status)
        {
            int timeLimit;
            int.TryParse(join.TimeLimit, out timeLimit);
            int GameId = -1;
            TheGameID GameID;

            //Sets Any Possible Status.
            if (!Token_Valid(join.UserToken))
            {
                status = Forbidden;
                return null;
            }
            if (timeLimit < 5 || timeLimit > 120)
            {
                status = Forbidden;
                return null;
            }

            if (Player_In_Pending(new Token() { UserToken = join.UserToken }))
            {
                status = Conflict;
                return null;
            }

            //Connection With DataBase.
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Using The Qurey.
                    using (SqlCommand command = new SqlCommand("Select Player2, GameID, TimeLimit from Games where Player2 IS NULL", conn, trans))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GameId = (int)reader["GameID"];
                                timeLimit = (int)reader["TimeLimit"];
                            }
                        }
                    }

                    //Set Up The Qurey Which Is Being Used.
                    string query;
                    if (GameId == -1)
                    {
                        query = "insert into Games (Player1, TimeLimit) output inserted.GameID values(@Player1, @TimeLimit)";
                    }
                    else
                    {
                        query = "update Games set Player2=@Player2, TimeLimit=@TimeLimit, Board=@Board, StartTime=@StartTime where GameID=@GameID";
                    }

                    using (SqlCommand command = new SqlCommand(query, conn, trans))
                    {
                        if (GameId == -1)
                        {
                            command.Parameters.AddWithValue("@Player1", join.UserToken);
                            command.Parameters.AddWithValue("@TimeLimit", join.TimeLimit);
                            GameID = new TheGameID() { GameID = command.ExecuteScalar().ToString() };
                            status = Accepted;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Player2", join.UserToken);
                            command.Parameters.AddWithValue("@TimeLimit", Time_Limit(timeLimit, join.TimeLimit));

                            if (board)
                            {
                                command.Parameters.AddWithValue("@Board", new BoggleBoard("NAMEPAINRAINGAIN").ToString());
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@Board", new BoggleBoard().ToString());
                            }
                            command.Parameters.AddWithValue("@StartTime", DateTime.Now);
                            command.Parameters.AddWithValue("@GameID", GameId);
                            GameID = new TheGameID() { GameID = GameId + "" };

                            status = Created;

                            if (command.ExecuteNonQuery() == 0)
                            {
                                command.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        return GameID;
                    }
                }
            }
        }

        /// <summary>
        /// Cancels The Join Request. It Also Sets Possible Status
        /// If Needed.
        /// </summary>
        /// <param name="token"></param>
        public void CancelAJoin(Token token, out HttpStatusCode status)
        {
            if (!Token_Valid(token.UserToken) || !Player_In_Pending(token))
            {
                status = Forbidden;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("Delete from Games where Player1 = @UserID and Player2 IS NULL", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", token.UserToken);

                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.ExecuteNonQuery();
                        }

                        status = OK;
                        trans.Commit();
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// Play A Word In A Game. Also Sets Possible Status
        /// If Needed.
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public TheScore PlayWord(string GameID, PlayWord word, out HttpStatusCode status)
        {

            //Set Any Status If Needed.
            if (!Word_Valid(word) || !Token_Valid(word.UserToken) || !GameID_Valid(GameID) || !User_In_Game(GameID, word))
            {
                status = Forbidden;
                return null;
            }

            if (Player_In_Pending(new Token() { UserToken = word.UserToken }) || Time_Left(GameID) <= 0)
            {
                status = Conflict;
                return null;
            }

            int gameid;
            int.TryParse(GameID, out gameid);
            string normWord = word.Word.ToUpper().Trim();
            status = OK;
            return Play_The_Word(gameid, word, normWord);
        }

        /// <summary>
        /// Get The Information Of The Game, Also Sets Any Possible Status
        /// If Needed.
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="Brief"></param>
        /// <returns></returns>
        public GameStatus Status(string GameID, string Brief, out HttpStatusCode status)
        {
            int enteredID;

            //Check Every Situation.
            if (!int.TryParse(GameID, out enteredID))
            {
                status = Forbidden;
                return null;
            }
            if (!GameID_Valid(GameID))
            {
                status = Forbidden;
                return null;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //Set Up Transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check If Player Posting The Game Is In A Pending Game
                    using (SqlCommand command = new SqlCommand("Select * from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", enteredID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //Pending
                                if (Player_In_Pending(new Token { UserToken = (string)reader["Player1"] }))
                                {
                                    status = OK;
                                    GameStatus pending = new GameStatus();
                                    pending.GameState = "pending";
                                    return pending;
                                }

                                //Completed
                                else if (Time_Left(GameID) <= 0)
                                {
                                    status = OK;
                                    GameStatus active = new GameStatus();
                                    active.GameState = "completed";
                                    active.Player1 = new FirstPlayer();
                                    active.Player2 = new SecondPlayer();
                                    active.Board = (string)reader["Board"];
                                    active.Player1.Nickname = GetNickname((string)reader["Player1"]);
                                    active.Player2.Nickname = GetNickname((string)reader["Player2"]);
                                    active.Player1.Score = Score_In_Total((string)reader["Player1"]);
                                    active.Player2.Score = Score_In_Total((string)reader["Player2"]);
                                    active.TimeLimit = (int)reader["TimeLimit"];
                                    if (Brief == null || Brief == "no")
                                    {
                                        List<AlreadyPlayedWord> player1 = The_Played_Words((string)reader["Player1"]);
                                        List<AlreadyPlayedWord> player2 = The_Played_Words((string)reader["Player2"]);
                                        active.Player1.WordsPlayed = player1;
                                        active.Player2.WordsPlayed = player2;
                                        active.TimeLeft = 0;
                                    }
                                    return active;
                                }

                                //Active
                                else
                                {
                                    status = OK;
                                    GameStatus active = new GameStatus();
                                    active.GameState = "active";
                                    active.TimeLeft = Time_Left(GameID);
                                    active.Player1 = new FirstPlayer();
                                    active.Player2 = new SecondPlayer();
                                    active.Player1.Nickname = GetNickname((string)reader["Player1"]);
                                    active.Player2.Nickname = GetNickname((string)reader["Player2"]);
                                    active.Player1.Score = Score_In_Total((string)reader["Player1"]);
                                    active.Player2.Score = Score_In_Total((string)reader["Player2"]);

                                    // If brief was not an option
                                    if (Brief == null || Brief == "no")
                                    {
                                        active.Board = (string)reader["Board"];
                                        active.TimeLimit = (int)reader["TimeLimit"];
                                    }
                                    return active;
                                }
                            }
                        }
                        status = Forbidden;
                        return null;
                    }
                }
            }
        }



        /// <summary>
        /// Below Are The Helper Methods.
        /// </summary>
        /// <returns></returns>



        /// <summary>
        /// Returns A Hashset Of The dictionary.txt
        /// </summary>
        /// <returns></returns>
        private static HashSet<String> The_Dictionary()
        {
            HashSet<String> dictionary = new HashSet<string>();

            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    dictionary.Add(line);
                }
            }

            return dictionary;
        }


        /// <summary>
        /// Plays the word
        /// </summary>
        /// <param name="gameid"></param>
        /// <param name="word"></param>
        /// <param name="normWord"></param>
        /// <returns></returns>

        private TheScore Play_The_Word(int gameid, PlayWord word, string normWord)
        {
            TheScore score = new TheScore();
            string tokenp2 = "";

            //The Connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //The Transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    using (SqlCommand command = new SqlCommand("Select Player2 from Games where GameID=@GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tokenp2 = (string)reader["Player2"];
                            }
                        }
                    }

                    using (SqlCommand command = new SqlCommand("Select Word from Words where Player=@Player1 or Player=@Player2 order by Word", conn, trans))
                    {
                        command.Parameters.AddWithValue("@Player1", word.UserToken);
                        command.Parameters.AddWithValue("@Player2", tokenp2);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (((string)reader["Word"]).Equals(normWord))
                                {
                                    return new TheScore { Score = 0 };
                                }
                            }
                        }
                    }


                    //Basically Sets The Correct Score When Needed.
                    using (SqlCommand command = new SqlCommand("insert into Words (Player, GameID, Word, Score) values (@UserID, @GameID, @Word, @Score)", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", word.UserToken);
                        command.Parameters.AddWithValue("@GameID", gameid);
                        command.Parameters.AddWithValue("@Word", normWord);

                        if (Word_Forming(gameid + "", normWord))
                        {
                            if (Dictionary.Contains(word.Word) && normWord.Length > 2)
                            {
                                if (normWord.Length == 3 || normWord.Length == 4)
                                {
                                    command.Parameters.AddWithValue("@Score", 1);
                                    score = new TheScore { Score = 1 };
                                }
                                else if (normWord.Length == 5)
                                {
                                    command.Parameters.AddWithValue("@Score", 2);
                                    score = new TheScore { Score = 1 };
                                }
                                else if (normWord.Length == 6)
                                {
                                    command.Parameters.AddWithValue("@Score", 3);
                                    score = new TheScore { Score = 3 };
                                }
                                else if (normWord.Length == 7)
                                {
                                    command.Parameters.AddWithValue("@Score", 5);
                                    score = new TheScore { Score = 5 };
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@Score", 11);
                                    score = new TheScore { Score = 11 };
                                }
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@Score", 0);
                                score = new TheScore { Score = 0 };
                            }
                        }
                        else if (normWord.Length <= 2)
                        {
                            command.Parameters.AddWithValue("@Score", 0);
                            score = new TheScore { Score = 0 };
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Score", -1);
                            score = new TheScore { Score = -1 };
                        }


                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return score;
                    }

                }
            }
        }


        /// <summary>
        /// Checks If A User Is In A Pending Game
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        private bool Player_In_Pending(Token token)
        {
            //The connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //The transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Checks If A User Is In A Pending Game
                    using (SqlCommand command = new SqlCommand("Select Player1, Player2 from Games where Player1 = @UserID and Player2 IS NULL", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", token.UserToken);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks If The Boggle Board Can Form The Word
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool Word_Forming(string GameID, string word)
        {
            string board = null;
            int gameid;
            int.TryParse(GameID, out gameid);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("Select Board from Games where GameID=@GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                board = (String)reader["Board"];
                            }
                        }
                        trans.Commit();
                    }
                }
            }

            if (new BoggleBoard(board).CanBeFormed(word))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Returns How Much Time Left.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2unparsed"></param>
        /// <returns></returns>
        private int Time_Limit(int t1, string t2unparsed)
        {
            int t2;
            int.TryParse(t2unparsed, out t2);

            return (t1 + t2) / 2;
        }

        /// <summary>
        /// Checks If The Token Is Valid.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool Token_Valid(string token)
        {
            if (token == null || token.Length != 36)
            {
                return false;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("Select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", token);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if GameID is valid in our database
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns></returns>
        private bool GameID_Valid(string GameID)
        {
            if (GameID == null)
            {
                return false;
            }

            int gameid;
            int.TryParse(GameID, out gameid);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("Select GameID from Games where GameID=@GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Checks to see if the word is valid
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool Word_Valid(PlayWord word)
        {
            if (word.Word.Trim().Length == 0 || word.Word == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks to see if the user exists in the game
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool User_In_Game(string GameID, PlayWord word)
        {
            //Set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //set up transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("Select GameID, Player1, Player2 from Games where GameID=@GameID and (Player1=@UserID or Player2=@UserID)", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        command.Parameters.AddWithValue("@UserID", word.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the time left in any given game
        /// true == active game
        /// false == completed game
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns></returns>
        private int Time_Left(string GameID)
        {
            DateTime gameStart = DateTime.Now;
            int timeLimit = 0;
            int gameid;
            int.TryParse(GameID, out gameid);

            //Set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //set up transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("SELECT TimeLimit, StartTime  from Games where GameID=@GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                timeLimit = (int)reader["TimeLimit"];
                                gameStart = reader.GetDateTime(1);
                            }
                        }
                        trans.Commit();
                    }
                }
            }

            DateTime now = DateTime.Now;

            int timeElapsed = (int)now.Subtract(gameStart).TotalSeconds;

            return timeLimit - timeElapsed;

        }
        /// <summary>
        /// Makes SQL call to sum the score of all words played by "userID"
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private int Score_In_Total(string userID)
        {
            int sum = 0;
            //Set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //set up transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("Select convert(varchar(30),SUM(Score)) as TotalScore from Words where Player=@UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    return 0;
                                }
                                else
                                {
                                    int.TryParse((string)reader["TotalScore"], out sum);
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            return sum;
        }
        /// <summary>
        /// SQL to retrieve nickname of "userID" from Users table 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private string GetNickname(string userID)
        {
            string name = null;
            //Set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //set up transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("Select NickName from Users where UserID=@UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name = reader.GetString(0);
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            return name;
        }

        /// <summary>
        /// SQL Call to retrieve all the words played by "userID"
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<AlreadyPlayedWord> The_Played_Words(string userID)
        {
            List<AlreadyPlayedWord> words = new List<AlreadyPlayedWord>();
            //Set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //set up transaction
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Check to see if the player is in the game
                    using (SqlCommand command = new SqlCommand("Select Word, score from Words where Player=@UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AlreadyPlayedWord played = new AlreadyPlayedWord();
                                played.Score = (int)reader["Score"];
                                played.Word = (string)reader["Word"];
                                words.Add(played);
                            }
                        }
                        trans.Commit();
                    }
                }
            }
            if (words.Count == 0)
            {
                return null;
            }
            return words;
        }
    }
}
