using System;
using System.IO;

namespace HIMBot
{
    internal class ChatMessages
    {
        private string path = @"c:\Users\HiImMike\Desktop\Twitch\Logs\Chat\";
        private string fileName;

        public ChatMessages()
        {
            fileName = DateTime.Now.TimeOfDay.ToString() + ".txt";
            fileName = fileName.Replace(":", "-");
            var log = File.CreateText(path + fileName);
            log.Close();
        }

        public void WriteMessage(string _message)
        {
            System.Console.WriteLine(_message);

            if (!File.Exists(path + fileName))
            {
                using (StreamWriter sw = File.CreateText(path + fileName))
                {
                    sw.WriteLine(_message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path + fileName))
                {
                    sw.WriteLine(_message);
                }
            }
        }
    }
}