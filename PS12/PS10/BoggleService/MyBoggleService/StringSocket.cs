using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 4.24.2018
/// </summary>
namespace Boggle
{
    class StringSocket
    {
        private BoggleService service;
        private Socket socket;
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        private Decoder decoder = encoding.GetDecoder();
        private bool sendIsOngoing = false;
        private readonly object sendSync = new object();

        //Size To Read Incoming Bytes
        private const int BUFFER_SIZE = 1024;

        //Received Texts
        private StringBuilder incoming;

        //The Text Which Is Ready To Be Sent
        private StringBuilder outgoing;

        //Arrays To contain Incoming Bytes And Characters.
        private byte[] incomingBytes = new byte[BUFFER_SIZE];
        private char[] incomingChars = new char[BUFFER_SIZE];

        //Bytes that we are actively trying to send, along with the
        //index of the leftmost byte whose send has not yet been completed
        private byte[] pendingBytes = new byte[0];
        private int pendingIndex = 0;

        //The Array Of Request.
        private object[] request_array = new object[6];

        public StringSocket(Socket s)
        {
            socket = s;
            service = new BoggleService();
            incoming = new StringBuilder();
            outgoing = new StringBuilder();

            // Calls MessageReceived When Received 1024 Bytes.
            socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, MessageReceived, null);
        }

        private void SendBytes()
        {
            //Keep Sending Bytes If The Bytes Are In Sending Progress.
            if (pendingIndex < pendingBytes.Length)
            {
                try
                {
                    Console.WriteLine("\tSending " + (pendingBytes.Length - pendingIndex) + " bytes");
                    socket.BeginSend(pendingBytes, pendingIndex, pendingBytes.Length - pendingIndex,
                                     SocketFlags.None, MessageSent, null);
                }
                catch (ObjectDisposedException)
                {

                }
            }

            else if (outgoing.Length > 0)
            {
                try
                {
                    pendingBytes = encoding.GetBytes(outgoing.ToString());
                    pendingIndex = 0;
                    Console.WriteLine("\tConverting " + outgoing.Length + " chars into " + pendingBytes.Length + " bytes, sending them");
                    outgoing.Clear();
                    socket.BeginSend(pendingBytes, 0, pendingBytes.Length,
                                     SocketFlags.None, MessageSent, null);
                }
                catch (ObjectDisposedException)
                {

                }
            }

            //If Nothing To Send, Shut Down.
            else
            {
                Console.WriteLine("Shutting down send mechanism\n");
                sendIsOngoing = false;
            }
        }



        /// <summary>
        /// Sends A Message To Client.
        /// </summary>
        private void SendMessage(string lines)
        {
            lock (sendSync)
            {
                outgoing.Append(lines);

                //If It's Not Ongoing, Start One.
                if (!sendIsOngoing)
                {
                    Console.WriteLine("Appending a " + lines.Length + " char line, starting send mechanism");
                    sendIsOngoing = true;
                    SendBytes();
                }
                else
                {
                    Console.WriteLine("\tAppending a " + lines.Length + " char line, send mechanism already running");
                }
            }
        }



        private void MessageSent(IAsyncResult result)
        {
            int bytesSent = socket.EndSend(result);
            Console.WriteLine("\t" + bytesSent + " bytes were successfully sent");

            lock (sendSync)
            {
                //Close The Socket.
                if (bytesSent == 0)
                {
                    socket.Close();
                    Console.WriteLine("Socket closed");
                }

                // Updates The pendingIndex.
                else
                {
                    pendingIndex += bytesSent;
                    SendBytes();
                }
            }
        }




        private void MessageReceived(IAsyncResult result)
        {
            //The Number Of Bytes Come In
            int bytesRead = socket.EndReceive(result);

            if (bytesRead == 0)
            {
                Console.WriteLine("Socket closed");
            }

            else
            {
                int charsRead = decoder.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);
                incoming.Append(incomingChars, 0, charsRead);
                int lastNewline = -1;
                int start = 0;
                for (int i = 0; i < incoming.Length; i++)
                {
                    if (incoming[i] == '\n')
                    {
                        String line = incoming.ToString(start, i + 1 - start);

                        Dataparse(line);

                        lastNewline = i;
                        start = i + 1;

                        if (line.Length == 2 && (string)request_array[0] == "GET")
                        {
                            ConfigureRequest();
                        }
                    }
                    else if (incoming[i] == '}')
                    {
                        String line = incoming.ToString(start, i + 1 - start);

                        Dataparse(line);
                        ConfigureRequest();
                        lastNewline = i;
                        start = i + 1;
                    }
                }

                incoming.Remove(0, lastNewline + 1);

                try
                {
                    socket.BeginReceive(incomingBytes, 0, incomingBytes.Length,
                        SocketFlags.None, MessageReceived, null);
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }




















        //////////////////////////////////////////        Below Are The Helper Methods        //////////////////////////////////////////





        private void Get_API()
        {
            SendMessage("HTTP/1.1 200 OK\n");
            var API = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "..\\index.html");
            SendMessage("Content-Length: text/html\r\n");
            SendMessage("\r\n");
            SendMessage(API);
        }


        private void GetGames(string requestBody, string gameid, string Brief)
        {
            GameStatus gameStatus;
            HttpStatusCode serviceStatus;

            // Make A Call
            if (Brief.Trim() == "yes")
            {
                gameStatus = service.Status(gameid, "yes", out serviceStatus);
            }
            else
            {
                gameStatus = service.Status(gameid, "no", out serviceStatus);
            }


            String s = JsonConvert.SerializeObject(gameStatus);

            //Sending Back Messages.
            SendMessage("HTTP/1.1 " + (int)serviceStatus + " " + serviceStatus.ToString() + "\r\n");
            SendMessage("Content-Type: application/json\r\n");
            SendMessage("Content-Length: " + s.Length + "\r\n");
            SendMessage("\r\n");
            SendMessage(s);
        }


        private void ConfigureRequest()
        {
            if ((string)request_array[0] == "POST")
            {
                if ((string)request_array[1] == "games")
                {
                    JoinGame((string)request_array[5]);
                }

                //Posting To User
                else
                {
                    RegisterUser((string)request_array[5]);
                }
            }
            else if ((string)request_array[0] == "PUT")
            {
                //Cancel Game
                if ((string)request_array[1] == "games" && request_array[2] == null)
                {
                    CancelGame((string)request_array[5]);
                }


                else
                {
                    PlayWord((string)request_array[5], (string)request_array[2]);
                }
            }

            else
            {
                GetGames((string)request_array[5], (string)request_array[2], (string)request_array[3]);
            }

        }




        private void CancelGame(string requestBody)
        {
            HttpStatusCode serviceStatus;
            Token token = JsonConvert.DeserializeObject<Token>(requestBody);
            service.CancelAJoin(token, out serviceStatus);
            SendMessage("HTTP/1.1 " + (int)serviceStatus + " " + serviceStatus.ToString() + "\r\n");
            SendMessage("Content-Length:0\r\n");
            SendMessage("\r\n");
        }

        private void RegisterUser(string RequestBody)
        {
            HttpStatusCode serviceStatus;
            UserInfo user = JsonConvert.DeserializeObject<UserInfo>(RequestBody);
            Token token = service.CreateAUser(user, out serviceStatus);

            String s = JsonConvert.SerializeObject(token, new JsonSerializerSettings
            { DefaultValueHandling = DefaultValueHandling.Ignore });

            SendMessage("HTTP/1.1 " + (int)serviceStatus + " " + serviceStatus.ToString() + "\r\n");
            SendMessage("Content-Type: application/json\r\n");
            SendMessage("Content-Length: " + s.Length + "\r\n");
            SendMessage("\r\n");
            SendMessage(s);
        }

        private void JoinGame(string requestBody)
        {
            HttpStatusCode serviceStatus;
            JoiningAGame user = JsonConvert.DeserializeObject<JoiningAGame>(requestBody);
            TheGameID gameID = service.JoinGame(user, out serviceStatus);

            String s = JsonConvert.SerializeObject(gameID, new JsonSerializerSettings
            { DefaultValueHandling = DefaultValueHandling.Ignore });

            SendMessage("HTTP/1.1 " + (int)serviceStatus + " " + serviceStatus.ToString() + "\r\n");
            SendMessage("Content-Type: application/json\r\n");
            SendMessage("Content-Length: " + s.Length + "\r\n");
            SendMessage("\r\n");
            SendMessage(s);
        }

        private void PlayWord(string requestBody, string gameId)
        {
            HttpStatusCode serviceStatus;
            PlayWord word = JsonConvert.DeserializeObject<PlayWord>(requestBody);

            TheScore wordScore = service.PlayWord(gameId, word, out serviceStatus);

            String s = JsonConvert.SerializeObject(wordScore);

            SendMessage("HTTP/1.1 " + (int)serviceStatus + " " + serviceStatus.ToString() + "\r\n");
            SendMessage("Content-Type: application/json\r\n");
            SendMessage("Content-Length: " + s.Length + "\r\n");
            SendMessage("\r\n");
            SendMessage(s);
        }

        private void Dataparse(String line)
        {
            Regex r;
            Match m;
            string regexString = "(?:(/BoggleService.svc/))";
            string requestType;

            if (IsHttpRequest(line, out requestType))
            {
                if (requestType == "GET")
                {
                    if ((m = (r = new Regex(regexString.Insert(23, "games/(.*)\\?(B|b)rief=(.*) "))).Match(line)).Success)
                    {
                        request_array[0] = "GET";
                        request_array[1] = "games";
                        request_array[2] = m.Groups[2].Value;
                        request_array[3] = m.Groups[4].Value;
                    }
                }
                else if (requestType == "PUT")
                {
                    if ((m = (r = new Regex(regexString.Insert(23, "games/(\\d+)"))).Match(line)).Success)
                    {
                        request_array[0] = "PUT";
                        request_array[1] = "games";
                        request_array[2] = m.Groups[2].Value;
                    }
                    else if ((m = (r = new Regex(regexString.Insert(23, "games"))).Match(line)).Success)
                    {
                        request_array[0] = "PUT";
                        request_array[1] = "games";
                    }
                }

                //Otherwise, POST Type
                else
                {
                    if ((m = (r = new Regex(regexString.Insert(23, "users"))).Match(line)).Success)
                    {
                        request_array[0] = "POST";
                        request_array[1] = "users";
                    }
                    else if ((m = (r = new Regex(regexString.Insert(23, "games"))).Match(line)).Success)
                    {
                        request_array[0] = "POST";
                        request_array[1] = "games";
                    }
                }
            }
            else if (IsContentLength(line))
            {
                string length = line.Substring(16);
                request_array[4] = int.Parse(length);
            }
            else if (IsRequestBody(line))
            {
                request_array[5] = line;
            }
        }

        private bool IsRequestBody(string line)
        {
            if (line[0] == '{')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks If It's The Content Length.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsContentLength(string line)
        {
            if (line[0] == 'C')
            {
                if (line.Contains("Length"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks If The Next Line Is Request Body.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool NextIsRequestBody(string line)
        {
            if (line.Length == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks If It's The HTTP Request.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="RequestType"></param>
        /// <returns></returns>
        private bool IsHttpRequest(string line, out string RequestType)
        {
            if (line[0] == 'G')
            {
                RequestType = "GET";
                return true;
            }
            else if (line[0] == 'P')
            {
                if (line[1] == 'U')
                {
                    RequestType = "PUT";
                    return true;
                }
                else
                {
                    RequestType = "POST";
                    return true;
                }
            }
            else
            {
                RequestType = null;
                return false;
            }
        }
    }
}
