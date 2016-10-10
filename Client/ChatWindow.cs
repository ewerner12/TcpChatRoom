using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class ChatWindow
    {
        private int portNumber;
        private string clientIp;
        //const string CLIENT_IP = IPAddress.Parse("127.0.0.1").ToString();

        public ChatWindow()
        {
            portNumber = 8888;
            clientIp = "127.0.0.1";
        }

        public void OpenNewChatWindow()
        {
            try
            {
                TcpClient client = new TcpClient(clientIp, portNumber);
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string usernameRequestFromServer = reader.ReadLine();
                Console.WriteLine(usernameRequestFromServer);
                string username = Console.ReadLine();
                writer.WriteLine(username);
                writer.Flush();
                ////Console.WriteLine("Username sent to server!!");

                Task listenForMessages = Task.Factory.StartNew(() => 
                                                    ListenForMessagesFromServer(reader, client));
                SendMessagesToServer(writer, client);
            }
            catch(Exception e)
            {
                ExitChat();
                Console.WriteLine("\nCLIENT EXCEPTION 1: {0}\n", e);
            }

        }

        public void ListenForMessagesFromServer(StreamReader reader, TcpClient client)
        {
            while (client.Connected)
            {
                string serverMessage = reader.ReadLine();
                Console.WriteLine(serverMessage);
            }
            //client.Close();
        }

        public void SendMessagesToServer(StreamWriter writer, TcpClient client)
        {
            while (client.Connected)
            {
                string messageToSendToServer = Console.ReadLine();
                writer.WriteLine(messageToSendToServer);
                writer.Flush();
            }
            //client.Close();
        }

        public void ExitChat()
        {
            Console.WriteLine("Active server NOT FOUND! Closing chat window.");
        }



    }
}
