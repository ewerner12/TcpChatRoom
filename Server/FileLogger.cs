using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class FileLogger : ILoggable
    {

        public void WriteMessageToFile(string message)
        {
            string path = @"C:\\Users\\Eric Werner\\Documents\\devCodeCamp\\TcpChatLog.txt";

            if (!File.Exists(path))
            {
                string fileCreateText = "File created on: " + DateTime.Now + "\r\n";
                File.WriteAllText(path, fileCreateText);
            }

            string appendText = DateTime.Now + ": " + message + "\r\n";
            File.AppendAllText(path, appendText);
        }

    }
}
