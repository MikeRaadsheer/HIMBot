using System;

namespace HIMBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();

            bot.Connect(false);

            Console.ReadLine();

            bot.Disconnect();
        }
    }
}
