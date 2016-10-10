using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    public class ChatRoom
    {
        private Dictionary connectedClients;
        private TcpListener server;
        private Queue<string> messageQueue;
        private Queue<string> serverQueue;
        public ILoggable fileLogger;

        public ChatRoom(ILoggable logger)
        {
            connectedClients = new Dictionary();
            server = new TcpListener(IPAddress.Any, 8888);
            messageQueue = new Queue<string>();
            serverQueue = new Queue<string>();
            fileLogger = logger;
        }

        public void OpenChatRoom()
        {
            Console.WriteLine("<<<<<<<Chat room open>>>>>>>\n");
            server.Start();

            while (true)
            {
                TcpClient newClient = server.AcceptTcpClient();
                Console.WriteLine("*****New client connected!*****");

                Thread chatRoomThread = new Thread(StartChat);
                chatRoomThread.Start(newClient);
            }
        }

        public void StartChat(object passedClient)
        {
            TcpClient client = (TcpClient)passedClient;
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            AskClientForUsername(writer);
            string clientName = reader.ReadLine().ToUpper();
            connectedClients.AddClientToActiveList(clientName, client); //check for dup usernames
            PrintNewClientMessageToServer(clientName);
            Task listenForMessages = Task.Factory.StartNew(() => 
                                            ListenForMessages(reader, messageQueue, serverQueue, clientName));
            ////connectedClients.PrintActiveClientsList();

            while (client.Connected)
            {
                try
                {
                    PrintChatMessageToServer(messageQueue, clientName);
                    connectedClients.PrintMessageToChatRoom(clientName, messageQueue, writer);
                    //writer.WriteLine(clientName + " > " + clientOutput); //to client, change to observer message
                    //writer.Flush();
                }
                catch(Exception e)
                {
                    connectedClients.RemoveClientFromActiveList(clientName);
                    reader.Close();
                    writer.Close();
                    client.Close();
                    Console.WriteLine("((({0} has left the chat)))", clientName);
                    Console.WriteLine("\nEXCEPTION: {0}\n", e);
                }
            }
        }

        public void AskClientForUsername(StreamWriter writer)
        {
            string username = "Please enter your username: ";
            writer.WriteLine(username);
            writer.Flush();
        }

        public string ListenForMessages(StreamReader reader, Queue<string> messageQueue, 
                                                                Queue<string> serverQueue, string clientName)
        {
            while (true)
            {
                string incomingMessage = reader.ReadLine();
                string loggerMessage = "[" + clientName + "] " + incomingMessage;
                fileLogger.WriteMessageToFile(loggerMessage);
                messageQueue.Enqueue(incomingMessage);
                serverQueue.Enqueue(incomingMessage);
            }
        }

        public void PrintNewClientMessageToServer(string clientName)
        {
            string newClientJoinedMessaged = ">>>" + clientName + " has joined the chat<<<";
            Console.WriteLine(newClientJoinedMessaged);
            fileLogger.WriteMessageToFile(newClientJoinedMessaged);
        }

        public void PrintChatMessageToServer(Queue<string> serverQueue, string clientName)
        {
            while (true)
            {
                if (serverQueue.Count > 0)
                {
                    Console.WriteLine("{0}: {1}", clientName, serverQueue.Dequeue());
                }
            }
        }








    }
}
