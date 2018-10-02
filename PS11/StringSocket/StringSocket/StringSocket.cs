// Written by Joe Zachary for CS 3500, November 2012
// Revised by Joe Zachary April 2016
// Revised extensively by Joe Zachary April 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;


/// <summary>
/// Author: Yingjie Lian & Xiaochuang huang
/// Class: CS-3500
/// Version: 4.16.2018
/// </summary>
namespace CustomNetworking
{
    /// <summary>
    /// The type of delegate that is called when a StringSocket send has completed.
    /// </summary>
    public delegate void SendCallback(bool wasSent, object payload);

    /// <summary>
    /// The type of delegate that is called when a receive has completed.
    /// </summary>
    public delegate void ReceiveCallback(String s, object payload);

    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchronization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// We can write a string to a StringSocket ss by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameter to the callback is the payload.  
    /// 
    /// We can read a string from a StringSocket ss by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a string and the payload.  The string is the requested string (with the newline removed).
    /// </summary>

    public class StringSocket : IDisposable
    {
        //In Order TO Store The Information About The Messages, I Created Two Structs.

        /// <summary>
        /// This Struct Contains The Message In The Send Request, It Will Also Be
        /// Enqueued To The Queue When BeginSend() Is Called.
        /// </summary>
        private struct Message_Send
        {
            public string message { get; set; }
            public Send_Callback callback { get; set; }
            public object payload { get; set; }
        }

        /// <summary>
        /// This Struct Contains The Message In The Receive Request, It Will Also Be
        /// Enqueued To The Queue When BeginReceive() Is Called.
        /// </summary>
        private struct Messgae_Receive
        {
            public Receive_Callback callback { get; set; }
            public object payload { get; set; }
        }

        // Underlying socket
        private Socket socket;

        // Encoding used for sending and receiving
        private Encoding encoding;

        // These delegates describe the callbacks that are used for sending and receiving strings.
        /// <summary>
        /// The Callback To Send The String.
        /// </summary>
        public delegate void Send_Callback(Exception e, object payload);

        /// <summary>
        /// The Callback To Receive The String.
        /// </summary>
        public delegate void Receive_Callback(String s, object payload);

        private int countSent;

        private Queue<Message_Send> sendQueue;
        private Queue<Messgae_Receive> receiveQueue;

        // Check If Message Is Sending Or Receiving Currently.
        private bool isSending;
        private bool isReceiving;

        // The Byte Form Of The Sending String
        private Byte[] sendBytes;

        // The Byte Form Of The Receiving String
        private Byte[] receiveBytes;
        private string partialMessage;
        private Queue<String> receivedMessages;


        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// StringSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        internal StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoding = e;
            sendBytes = new Byte[1];
            receiveBytes = new Byte[1024];
            sendQueue = new Queue<Message_Send>();
            receiveQueue = new Queue<Messgae_Receive>();
            isSending = false;
            isReceiving = false;
            receivedMessages = new Queue<string>();
            //// TODO: Complete implementation of StringSocket
        }

        /// <summary>
        /// Shuts down this StringSocket.
        /// </summary>
        public void Shutdown(SocketShutdown mode)
        {
            socket.Shutdown(mode);
        }

        /// <summary>
        /// Closes this StringSocket.
        /// </summary>
        public void Close()
        {
            socket.Close();
        }

        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket it invokes the callback.  
        /// The parameters to the callback are true and the payload.
        /// 
        /// If it is impossible to send because the underlying Socket has closed, the callback 
        /// is invoked with false and the payload as parameters.
        ///
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(String s, Send_Callback callback, object payload)
        {


            lock (sendQueue)
            {
                // Create A New Send Message And Enqueue It.
                sendQueue.Enqueue(new Message_Send { message = s, callback = callback, payload = payload });

                // Start A New Thread If The Sending Process Is Not Runninng.
                if (!isSending)
                {
                    isSending = true;
                    sendingBytes();
                }
            }
            // TODO: Implement BeginSend
        }

        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        ///     
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, it 
        /// invokes the callback.  The parameters to the callback are a string and the payload.  
        /// The string is the requested string (with the newline removed).
        /// 
        /// Alternatively, we can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload, length)
        ///     
        /// If length is negative or zero, this behaves identically to the first case.  If length
        /// is positive, then it reads and decodes length bytes from the underlying Socket, yielding
        /// a string s.  The parameters to the callback are s and the payload
        ///
        /// In either case, if there are insufficient bytes to service a request because the underlying
        /// Socket has closed, the callback is invoked with null and the payload.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.
        /// </summary>
        public void BeginReceive(Receive_Callback callback, object payload, int length = 0)
        {
            lock (receiveQueue)
            {
                // Create A New Receive Message And Enqueue It.
                receiveQueue.Enqueue(new Messgae_Receive { callback = callback, payload = payload });

                // Start A New Thread If The Receiving Process Is Not Runninng.
                if (!isReceiving)
                {
                    isReceiving = true;
                    receivingBytes();
                }
            }
        }

        /// <summary>
        /// Frees resources associated with this StringSocket.
        /// </summary>
        public void Dispose()
        {
            Shutdown(SocketShutdown.Both);
            Close();
        }


        /// <summary>
        /// This is a helper method for sending byte in socket to server
        /// </summary>
        private void sendingBytes()
        {
            //Creating a bytes array for representing first message.
            sendBytes = encoding.GetBytes(sendQueue.Peek().message);
            //set up the conut sent to 0
            countSent = 0;

            try
            {
                // Send Bytes To Server.
                socket.BeginSend(sendBytes, countSent, sendBytes.Length - 1, SocketFlags.None, SendByteCallback, null);
            }
            catch (Exception e)
            {
                Message_Send exceptionMessage = sendQueue.Dequeue();
                ThreadPool.QueueUserWorkItem(o => exceptionMessage.callback(e, exceptionMessage.payload));
            }
        }


        /// <summary>
        /// This is a helper method to receive the bytes in socket
        /// </summary>
        private void receivingBytes()
        {
            while (receiveQueue.Count > 0)
            {
                if (receivedMessages.Count > 0)
                {
                    Messgae_Receive justReceived = receiveQueue.Dequeue();
                    string message = receivedMessages.Dequeue();
                    ThreadPool.QueueUserWorkItem(o => justReceived.callback(message, justReceived.payload));
                }
                else break;
            }

            int countReceive = 0;

            //Begin Receive String.
            if (receiveQueue.Count > 0)
            {
                try
                {
                    //Receiving Bytes.
                    socket.BeginReceive(receiveBytes, countReceive, receiveBytes.Length, SocketFlags.None, ReceiveByteCallback, null);
                }
                catch (Exception e)
                {
                    Messgae_Receive exceptionMessage = receiveQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(o => exceptionMessage.callback(null, exceptionMessage.payload));
                }
            }
            else
            {
                isReceiving = false;
            }
        }

        /// <summary>
        /// This is a helper method to check is it all bytes begin to send.
        /// </summary>
        /// <param name="ar"></param>
        private void SendByteCallback(IAsyncResult ar)
        {
            lock (sendQueue)
            {
                // Returns Number Of Bytes Sent In BeginSend()
                countSent += socket.EndSend(ar);

                // Returns ThE Remaining Bytes
                int remainingBytes = sendBytes.Length - countSent;

                // Sending Bytes If Still Have Bytes.
                if (remainingBytes > 0)
                {
                    socket.BeginSend(sendBytes, countSent, remainingBytes, SocketFlags.None, SendByteCallback, null);
                }
                else
                {
                    Message_Send justSent = sendQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(o => justSent.callback(null, justSent.payload));

                    // If Still Have Message Remaining....
                    if (sendQueue.Count > 0)
                    {
                        sendingBytes();
                    }
                    else
                    {
                        isSending = false;
                    }
                }
            }
        }

        /// <summary>
        /// This is a helper method to check is it all bytes begin to receive.
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveByteCallback(IAsyncResult ar)
        {
            lock (receiveQueue)
            {
                int bytesReceived = socket.EndReceive(ar);
                partialMessage += encoding.GetString(receiveBytes, 0, bytesReceived);

                while (!(partialMessage.IndexOf("\n").Equals(-1)))
                {
                    int messageLength = partialMessage.IndexOf("\n");
                    receivedMessages.Enqueue(partialMessage.Substring(0, messageLength));
                    partialMessage = partialMessage.Substring(messageLength + 1);
                }

                receivingBytes();
            }
        }
    }
}
