using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

//TCP Chat Room Project
//Users can chat with one another over the local network.

//There should be two projects: one client-side (Client), one server-side (Server). [DONE]
//Implement the Observer design pattern to send out a notification to all users 
//  that a new person has joined the chat room. [DONE]
//Implement Dependency Injection for logging (log all messages, 
//  log when someone joins the chat, log when someone leaves the chat). [DONE]
//  -have to have a separate filewriter class and pass filewriter object around
//Use a Dictionary to store users. [DONE]
//Use a Queue to store/process incoming messages. [DONE]
//HINT: Use TCPclient instead of raw Socket class. [DONE]

//Bonus (5 points): allow direct messages. []

//Bonus(5 points): allow creation of private chat rooms. []

//Super Bonus(5 points): implement a GUI. []

namespace Client
{
    public class Client
    {
        static void Main(string[] args)
        {
            ChatWindow chatWindow = new ChatWindow();
            chatWindow.OpenNewChatWindow();

            Console.ReadLine();
        }



    }
}
