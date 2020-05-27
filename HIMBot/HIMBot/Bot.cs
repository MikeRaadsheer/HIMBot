using System;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;

namespace HIMBot
{
    internal class Bot
    {
        private readonly ConnectionCredentials creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.BotToken);
        private TwitchClient client;
        private Announcement follow;
        private Announcement multi;
        private Command command;
        private DebugTools debug;
        private PointsTracker points;
        private ChatMessages messages = new ChatMessages();
        private UsersManager usersManager = new UsersManager();

        internal void Connect(bool isLogging)
        {
            Console.WriteLine("[Bot]: Connecting...");
            client = new TwitchClient();
            client.Initialize(creds, TwitchInfo.ChannelName);
            client.OnConnectionError += Client_OnConnectionError;
            client.OnConnected += Client_OnConnected;
            
            if (isLogging)
            {
                client.OnLog += Client_OnLog;
            }

            client.Connect();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("[Bot]: Connected successfully!");
            client.OnUserTimedout += Client_OnUserTimedout;
            client.OnMessageReceived += Client_OnMessageReceived;
            points = new PointsTracker(client);
            follow = new Announcement(client, "Please remember to follow if you enjoy the stream!", TimeSpan.FromMinutes(40));
            multi = new Announcement(client, "Watch both of us at the same time on multistrean https://multistre.am/hiimmike/realedgythehedgy/layout5/", TimeSpan.FromMinutes(15));

            var start = TimeSpan.Zero;
            var announce = new System.Threading.Timer((evt) => {
                points.Save();
                usersManager.Save();
            }, null, start, TimeSpan.FromMinutes(5));

            command = new Command(client, points);
            debug = new DebugTools(client, points, usersManager);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("^-^ "))
            {
                Users _users = usersManager.GetUsers();
                for (int i = 0; i < _users.users.Count; i++)
                {
                    if (_users.users[i].userName == e.ChatMessage.DisplayName)
                    {
                        if (_users.users[i].lastCommand < DateTime.Now.AddMinutes(-1) || e.ChatMessage.DisplayName == TwitchInfo.ChannelName)
                        {
                            _users.users[i].lastCommand = DateTime.Now;
                            command.Execute(e);
                            return;
                        }
                        else
                        {
                            client.SendWhisper(e.ChatMessage.Username, "please wait for one minute to pass before using another command");
                            return;
                        }
                    }
                }
                usersManager.AddUser(e.ChatMessage.DisplayName);
                command.Execute(e);
                usersManager.Save();
            }

            if (e.ChatMessage.Message.StartsWith("Debug ") && e.ChatMessage.DisplayName == TwitchInfo.ChannelName)
            {
                debug.Execute(e);
                return;
            }
            messages.WriteMessage($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
        }

        private void Client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            string sentence = "";

            Random rnd = new Random();
            int val = rnd.Next(0, 3);

            switch (val)
            {
                case 0:
                    sentence = "got ZIPPED lol!";
                    break;
                case 1:
                    sentence = "get rekt scrub!";
                    break;
                case 2:
                    sentence = "git gud!";
                    break;
            }

            client.SendMessage(TwitchInfo.ChannelName, $"{e.UserTimeout.Username} " + sentence.ToString());
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine(e.Error);
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }

        internal void Disconnect()
        {
            Console.WriteLine("[Bot]: Disconnecting...");
            points.Save();
            usersManager.Save();
            client.Disconnect();
        }
    }
}