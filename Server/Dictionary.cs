using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace Server
{
    public class Dictionary : IEnumerable<KeyValuePair<string, TcpClient>>
    {
        private Dictionary<string, TcpClient> clients;
        private Dictionary<string, TcpClient>.KeyCollection clientsKeys;
        private Dictionary<string, TcpClient>.ValueCollection clientsValues;

        public Dictionary()
        {
            clients = new Dictionary<string, TcpClient>();
            clientsKeys = clients.Keys;
            clientsValues = clients.Values;
        }

        public IEnumerator<KeyValuePair<string, TcpClient>> GetEnumerator()
        {
            foreach (var pair in clients) 
            {
                yield return new KeyValuePair<string, TcpClient>(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void AddClientToActiveList(string username, TcpClient client)
        {
            clients.Add(username, client);
            SendNewClientNotificationToChatRoom(username);
        }

        public void RemoveClientFromActiveList(string username)
        {
            clients.Remove(username);
            SendClientLeftNotificationToChatRoom(username);
        }

        public void PrintActiveClientsList()
        {
            if(!(clients.Count == 0))
            {
                Console.WriteLine("===============");
                Console.WriteLine("ACTIVE CLIENTS: ");
                foreach (var pair in clients)
                {
                    Console.WriteLine(">{0}", pair.Key);
                }
                Console.WriteLine("===============");
            }
        }

        public void SendNewClientNotificationToChatRoom(string username)
        {
            foreach (TcpClient client in clientsValues)
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(">>{0} has joined the chat!<<", username);
                writer.Flush();
            }
        }

        public void PrintMessageToChatRoom(string clientName, Queue<string> messageQueue, StreamWriter writer)
        {
            foreach (TcpClient client in clientsValues)
            {
                while (messageQueue.Count > 0)
                {
                    writer.WriteLine("{0}: {1}", clientName, messageQueue.Dequeue());
                    writer.Flush();
                }
            }
        }

        public void SendClientLeftNotificationToChatRoom(string username)
        {
            foreach(TcpClient client in clientsValues)
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine("((({0} has left the chat.)))", username);
                writer.Flush();
            }
        }



    }
}
