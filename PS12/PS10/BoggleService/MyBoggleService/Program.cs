using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 4.24.2018
/// </summary>
namespace Boggle
{
    class Program
    {

        private static BoggleService boggle;
        private TcpListener server;
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();

        public static void Main()
        {
            HttpStatusCode status;
            UserInfo name = new UserInfo { Nickname = "Joe" };
            BoggleService service = new BoggleService();
            Token user = service.CreateAUser(name, out status);
            Console.WriteLine(user.UserToken);
            Console.WriteLine(status.ToString());

            // This is our way of preventing the main thread from
            // exiting while the server is in use
            //Console.ReadLine();


            new Program();
            Console.ReadLine();
        }



        /// <summary>
        /// Creates A TcpListner.
        /// </summary>
        public Program()
        {
            boggle = new BoggleService();
            server = new TcpListener(IPAddress.Any, 60000);
            server.Start();
            server.BeginAcceptSocket(ConnectionRequested, null);
        }

        /// <summary>
        /// Recieves The Request.
        /// </summary>
        /// <param name="result"></param>
        private void ConnectionRequested(IAsyncResult result)
        {
            Socket s = server.EndAcceptSocket(result);
            server.BeginAcceptSocket(ConnectionRequested, null);
            new StringSocket(s);
        }

    }
}
