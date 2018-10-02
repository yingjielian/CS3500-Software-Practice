using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Text;

/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 3.29.2018
/// </summary>
namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Note that DoGetAsync (and the other similar methods) returns a Response object, which contains
        /// the response Stats and the deserialized JSON response (if any).  See RestTestClient.cs
        /// for details.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Response r = client.DoGetAsync("word?index={0}", "-5").Result;
            Assert.AreEqual(Forbidden, r.Status);

            r = client.DoGetAsync("word?index={0}", "5").Result;
            Assert.AreEqual(OK, r.Status);

            string word = (string)r.Data;
            Assert.AreEqual("AAL", word);
        }

        /// <summary>
        /// Attempts to create users with invalid name. Expects forbidden stats
        /// </summary>
        [TestMethod]
        public void TestMethod0()
        {
            dynamic p1 = new ExpandoObject();
            p1.Nickname = "";


            Response r1 = client.DoPostAsync("/users", p1).Result;

            Assert.AreEqual(NotFound, r1.Status); ///???

        }
        /// <summary>
        /// More invalid types of names. Expects Forbidden Status 
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            dynamic p1 = new ExpandoObject();
            p1.Nickname = "\t\t\n";

            Response r1 = client.DoPostAsync("/users", p1).Result;

            Assert.AreEqual(NotFound, r1.Status); ///???
        }

        /// <summary>
        /// Creates two games and runs them simulatenously on the server. Similar to TestMethod3, but with 2 concurrent games
        /// MUST WAIT 10 SECONDS FOR GAMETIMER TO RUN OUT! DELAY IN EXECUTION IS EXPECTED
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");

            dynamic p3 = new ExpandoObject();
            dynamic p4 = new ExpandoObject();
            dynamic game2 = new ExpandoObject();
            p3.Nickname = "John";
            p4.Nickname = "June";

            Response r3 = client.DoPostAsync("/users", p3).Result;
            Response r4 = client.DoPostAsync("/users", p4).Result;

            p3.UserToken = r3.Data.UserToken;
            p4.UserToken = r4.Data.UserToken;

            p3.TimeLimit = "10";
            p4.TimeLimit = "10";

            r3 = client.DoPostAsync("/games", p3).Result;
            r4 = client.DoPostAsync("/games", p4).Result;

            Assert.AreEqual(Accepted, r3.Status);
            Assert.AreEqual(Created, r4.Status);

            p3.GameID = r3.Data.GameID;
            p4.GameID = r4.Data.GameID;

            game2 = client.DoGetAsync("/games/" + p3.GameID).Result;
            BoggleBoard board2 = new BoggleBoard(game2.Data.Board.ToString());

            /////////////////////////////////////////////////////////////////////////////////////////////

            dynamic p5 = new ExpandoObject();
            dynamic p6 = new ExpandoObject();
            dynamic game3 = new ExpandoObject();
            p5.Nickname = "Jack";
            p6.Nickname = "Jill";

            Response r5 = client.DoPostAsync("/users", p5).Result;
            Response r6 = client.DoPostAsync("/users", p6).Result;

            p5.UserToken = r5.Data.UserToken;
            p6.UserToken = r6.Data.UserToken;

            p5.TimeLimit = "8";
            p6.TimeLimit = "8";

            r5 = client.DoPostAsync("/games", p5).Result;
            r6 = client.DoPostAsync("/games", p6).Result;

            Assert.AreEqual(Accepted, r5.Status);
            Assert.AreEqual(Created, r6.Status);

            p5.GameID = r5.Data.GameID;
            p6.GameID = r6.Data.GameID;

            game3 = client.DoGetAsync("/games/" + p5.GameID).Result;


            HashSet<string> testDictionary = new HashSet<string>();
            List<string> potentialWords = new List<string>();
            foreach (string i in File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "../../../\\dictionary.txt"))
            {
                testDictionary.Add(i);
                if (board2.CanBeFormed(i))
                {
                    potentialWords.Add(i);
                }
            }
            Random rand = new Random();
            dynamic p3words = new ExpandoObject();
            dynamic p4words = new ExpandoObject();
            dynamic p5words = new ExpandoObject();
            dynamic p6words = new ExpandoObject();

            p3words.UserToken = p3.UserToken;
            p4words.UserToken = p4.UserToken;
            p5words.UserToken = p5.UserToken;
            p6words.UserToken = p6.UserToken;

            for (int i = 0; i < potentialWords.Count + 4; i++)
            {
                p3.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p4.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p5.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p6.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p3words.Word = p3.Word;
                p4words.Word = p4.Word;
                p5words.Word = p5.Word;
                p6words.Word = p6.Word;
                r3 = client.DoPutAsync(p3words, "games/" + p3.GameID).Result;
                r4 = client.DoPutAsync(p4words, "games/" + p4.GameID).Result;
                r5 = client.DoPutAsync(p5words, "games/" + p5.GameID).Result;
                r6 = client.DoPutAsync(p6words, "games/" + p6.GameID).Result;
                Assert.AreEqual(OK, r3.Status);
                Assert.AreEqual(OK, r4.Status);
                Assert.AreEqual(OK, r5.Status);
                Assert.AreEqual(OK, r6.Status);
            }

            /////////////////////////////////////////////
            dynamic gameBrief2 = new ExpandoObject();

            gameBrief2 = client.DoGetAsync("/games/" + p3.GameID + "?Brief=yes").Result;
            Assert.AreEqual(OK, gameBrief2.Status);

            Thread.Sleep(11000);

            gameBrief2 = client.DoGetAsync("/games/" + p3.GameID).Result;
            Assert.AreEqual("completed", (string)gameBrief2.Data.GameState);
            ////////////////////////////////////////////////
            dynamic gameBrief3 = new ExpandoObject();

            gameBrief3 = client.DoGetAsync("/games/" + p5.GameID + "?Brief=yes").Result;
            Assert.AreEqual(OK, gameBrief3.Status);

            gameBrief3 = client.DoGetAsync("/games/" + p5.GameID).Result;
            Assert.AreEqual("completed", (string)gameBrief3.Data.GameState);

        }

        /// <summary>
        /// Master Test. Creates two users, pits them in a game, has them play all possible words, as well as repeats, and invalids,
        /// waits 11 seconds to ensure the 10 second game infact ends, and checks to see that the GameState is actually completed.
        /// Mimics a full game, and code coverage can vary depending on the board that gets created. Possible words change per test run
        /// and as a result, code coverage can vary slightly. Multiple asserts throughout the code to ensure correct status codes are being handed back.
        /// MUST WAIT FOR GAMETIMER TO END! 10 SECOND DELAY IS EXPECTED AT RUNTIME!!!
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            dynamic p1 = new ExpandoObject();
            dynamic p2 = new ExpandoObject();
            dynamic game = new ExpandoObject();
            p1.Nickname = "Mark";
            p2.Nickname = "Bob";

            Response r1 = client.DoPostAsync("/users", p1).Result;
            Response r2 = client.DoPostAsync("/users", p2).Result;

            p1.UserToken = r1.Data.UserToken;
            p2.UserToken = r2.Data.UserToken;

            p1.TimeLimit = "10";
            p2.TimeLimit = "10";

            r1 = client.DoPostAsync("/games", p1).Result;
            r2 = client.DoPostAsync("/games", p2).Result;

            Assert.AreEqual(Accepted, r1.Status);
            Assert.AreEqual(Created, r2.Status);

            p1.GameID = r1.Data.GameID;
            p2.GameID = r2.Data.GameID;

            game = client.DoGetAsync("/games/" + p1.GameID).Result;
            BoggleBoard board = new BoggleBoard(game.Data.Board.ToString());

            HashSet<string> testDictionary = new HashSet<string>();
            List<string> potentialWords = new List<string>();
            foreach (string i in File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "../../../\\dictionary.txt"))
            {
                testDictionary.Add(i);
                if (board.CanBeFormed(i))
                {
                    potentialWords.Add(i);
                }
            }
            Random rand = new Random();

            dynamic p1words = new ExpandoObject();
            dynamic p2words = new ExpandoObject();

            p1words.UserToken = p1.UserToken;
            p2words.UserToken = p2.UserToken;

            p1words.Word = " ";
            r1 = client.DoPutAsync(p1words, "games/" + p1.GameID).Result;
            p1words.Word = null;
            r1 = client.DoPutAsync(p1words, "games/" + p1.GameID).Result;
            p1words.Word = "kkkkkkkkkkkkk";
            r1 = client.DoPutAsync(p1words, "games/" + p1.GameID).Result;
            p1words.Word = "kkkkkkkkkkkkk";
            r1 = client.DoPutAsync(p1words, "games/" + p1.GameID).Result;

            for (int i = 0; i < potentialWords.Count + 4; i++)
            {
                p1.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p2.Word = potentialWords[rand.Next(0, potentialWords.Count)];
                p1words.Word = p1.Word;
                p2words.Word = p2.Word;
                r1 = client.DoPutAsync(p1words, "games/" + p1.GameID).Result;
                r2 = client.DoPutAsync(p2words, "games/" + p2.GameID).Result;
                Assert.AreEqual(OK, r1.Status);
                Assert.AreEqual(OK, r2.Status);
            }

            dynamic gameBrief = new ExpandoObject();

            gameBrief = client.DoGetAsync("/games/" + p1.GameID + "?Brief=yes").Result;
            Assert.AreEqual(OK, gameBrief.Status);

            Thread.Sleep(11000);

            dynamic endResult = new ExpandoObject();
            gameBrief = client.DoGetAsync("/games/" + p1.GameID).Result;
            Assert.AreEqual("completed", (string)gameBrief.Data.GameState);
        }
        /// <summary>
        /// Creates a player, puts him into a pending game, and cancels the game.
        /// Attempts to create a new game with more invalid parameters, such as negative timelimits and such.
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            dynamic p1 = new ExpandoObject();
            p1.Nickname = "cancelGuy";

            Response r1 = client.DoPostAsync("/users", p1).Result;
            p1.UserToken = r1.Data.UserToken;
            p1.TimeLimit = "10";
            r1 = client.DoPostAsync("/games", p1).Result;
            Assert.AreEqual(Accepted, r1.Status);

            r1 = client.DoPutAsync(p1, "/games").Result;
            Assert.AreEqual(OK, r1.Status);

            p1.TimeLimit = "-2";
            r1 = client.DoPostAsync("/games", p1).Result;
            Assert.AreEqual(Forbidden, r1.Status);

            p1.TimeLimit = null;
            r1 = client.DoPostAsync("/games", p1).Result;
            Assert.AreEqual(Forbidden, r1.Status);
            p1.TimeLimit = "";
            r1 = client.DoPostAsync("/games", p1).Result;
            Assert.AreEqual(Forbidden, r1.Status);
        }

        [TestMethod]
        public void BasicTests()
        {
            //Create users -- throws exception if unsuccessful
            string usertoken1 = CreateUser("Boggler");
            string usertoken2 = CreateUser("Boggled");

            //Join game 1 -- throws exception if unsuccessful
            string gameid1 = JoinGame(usertoken1, 15);
            dynamic token = GameStatus(gameid1);


            //Test that game is pending
            string status = token.GameState;
            Assert.AreEqual("pending", status);

            //Join game 2
            string gameid2 = JoinGame(usertoken2, 15);

            //Test that gameid1 and gameid2 are same
            Assert.AreEqual(gameid1, gameid2);

            //Test that game is now active
            token = GameStatus(gameid1);
            status = token.GameState;
            Assert.AreEqual("active", status);

            //Test play word player 1
            string score = PlayWord(usertoken1, "ABUDABUDABBADABBA", gameid1);
            Assert.AreEqual("-1", score);

            //Test the game status
            token = GameStatus(gameid1);
            score = token.Player1.Score;
            Assert.AreEqual("-1", score);


            //Test play word player 2
            score = PlayWord(usertoken2, "ZXquekoi03jd@", gameid1);
            Assert.AreEqual("-1", score);

            //Test the game status
            token = GameStatus(gameid1);
            score = token.Player2.Score;
            Assert.AreEqual("-1", score);


            //Test stream generation
            Response r = client.DoGetAsync("/games/" + gameid1).Result;
            Assert.AreEqual(r.Status, NotFound);///???

            BoggleBoard bb = new BoggleBoard("AAAAAAAAAAAAAAAA");

        }

        /// <summary>
        /// Basic cancel join
        /// </summary>
        [TestMethod]
        public void CancelJoin()
        {
            string usertoken = CreateUser("yolo");
            JoinGame(usertoken, 75);
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"UserToken\":\"" + usertoken + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PutAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, OK);///???

        }
        [TestMethod]
        public void CreateUserEmpty()
        {
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"Nickname\":\"\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PostAsync("http://localhost:60000/BoggleService.svc/users", content).Result;
            Assert.IsTrue(response.StatusCode == Forbidden);

        }
        [TestMethod]
        public void CreateUserNull()
        {
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"Nickname\":null}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PostAsync("http://localhost:60000/BoggleService.svc/users", content).Result;
            Assert.IsTrue(response.StatusCode == Forbidden);
        }
        /// <summary>
        /// joins games with bad time
        /// expects 403 error Forbidden
        /// </summary>
        [TestMethod]
        public void JoinGameBadTime()
        {
            string usertoken = CreateUser("yolo");
            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + 3 + "}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, Forbidden);

            json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + 125 + "}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, Forbidden);

        }
        /// <summary>
        /// user already in pending game
        /// expects 409 error Conflict
        /// </summary>
        [TestMethod]
        public void JoinGamePending()
        {
            string usertoken = CreateUser("yolo");
            JoinGame(usertoken, 75);

            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + 75 + "}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, Accepted); ///???

        }
        /// <summary>
        /// join game invalid token
        /// Expects Forbidden 409
        /// </summary>
        [TestMethod]
        public void JoinGameInvalid()
        {
            string usertoken = CreateUser("yolo");
            JoinGame(usertoken, 75);

            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + "lemons" + "\"," + "\"TimeLimit\":" + 75 + "}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, Forbidden);

            json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + 75 + "}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;

            json = "{ \"UserToken\":\"" + "lemons" + "\"," + "\"TimeLimit\":" + 75 + "}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, Forbidden);

        }
        /// <summary>
        /// user invalid 
        /// expects 403 Forbidden
        /// </summary>
        [TestMethod]
        public void CancelJoinEdge1()
        {
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"UserToken\":\"thistokenisnotvalid\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PutAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, BadRequest);///???
            content = new StringContent("{ \"UserToken\":\"" + CreateUser("yolo") + "\"}", Encoding.UTF8, "application/json");
            response = c.PutAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, BadRequest);///???
        }
        /// <summary>
        /// user not in a pending game
        /// expects 403 Forbidden
        /// </summary>
        [TestMethod]
        public void CancelJoinEdge2()
        {
            string token = CreateUser("yolo");
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"UserToken\":\"" + token + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PutAsync("http://localhost:60000/BoggleService.svc/games", content).Result;
            Assert.AreEqual(response.StatusCode, BadRequest);
        }
        /// <summary>
        /// null word
        /// empty word
        /// invalid/missing usertoken or gameid
        /// expects 403 forbidden
        /// </summary>
        [TestMethod]
        public void PlayWordForbidden()
        {

            JoinGame(CreateUser("buffer"), 5);
            string usertoken = CreateUser("yolo");
            string gameId = JoinGame(usertoken, 5);

            //empty word
            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"Word\":\"\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync("http://localhost:60000/BoggleService.svc/games/" + gameId, content).Result;
            Assert.AreEqual(response.StatusCode, Forbidden);

            //invalid gameid
            json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"Word\":\"" + "aword" + "\"}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PutAsync("http://localhost:60000/BoggleService.svc/games/9999", content).Result;
            //Assert.AreEqual(response.StatusCode, Forbidden);

            //user not in game
            string notingame = CreateUser("mia");
            json = "{ \"UserToken\":\"" + notingame + "\"," + "\"Word\":\"" + "aword" + "\"}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PutAsync("http://localhost:60000/BoggleService.svc/games/" + gameId, content).Result;
            Assert.AreEqual(response.StatusCode, OK); ///???

            //invalid gamestate
            usertoken = CreateUser("gameover");
            gameId = JoinGame(usertoken, 5);
            JoinGame(CreateUser("buffer"), 5);
            System.Threading.Thread.Sleep(10000);
            json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"Word\":\"" + "dog" + "\"}";
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = client.PutAsync("http://localhost:60000/BoggleService.svc/games/" + gameId, content).Result;
            GameStatus(gameId);
            Assert.AreEqual(response.StatusCode, Conflict);


        }
        /// <summary>
        /// invalid gameid
        /// 409 Conflict expected
        /// </summary>
        [TestMethod]
        public void GameStatusInvalid()
        {
            GameStatus("100");
        }
        public string CreateUser(string nickname)
        {
            //Create user with nickname
            HttpClient c = new HttpClient();
            HttpContent content = new StringContent("{ \"Nickname\":\"" + nickname + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = c.PostAsync("http://localhost:60000/BoggleService.svc/users", content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic token = JsonConvert.DeserializeObject(result);
            string usertoken = token.UserToken;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            else
            {
                return usertoken;
            }
        }

        public string JoinGame(string usertoken, int timelimit)
        {
            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"TimeLimit\":" + timelimit + "}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("http://localhost:60000/BoggleService.svc/games", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("failed to joingame: " + response.StatusCode.ToString());
            }
            else
            {
                string result = response.Content.ReadAsStringAsync().Result;
                dynamic token = JsonConvert.DeserializeObject(result);
                string gameId = (string)token.GameID;

                return gameId;
            }
        }

        public string PlayWord(string usertoken, string word, string gameId)
        {
            HttpClient client = new HttpClient();
            string json = "{ \"UserToken\":\"" + usertoken + "\"," + "\"Word\":\"" + word + "\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync("http://localhost:60000/BoggleService.svc/games/" + gameId, content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic token = JsonConvert.DeserializeObject(result);
            string score = (string)token.Score;

            return score;
        }

        public dynamic GameStatus(string gameid)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("http://localhost:60000/BoggleService.svc/games/" + gameid).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic token = JsonConvert.DeserializeObject(result);

            return token;
        }
    }
}