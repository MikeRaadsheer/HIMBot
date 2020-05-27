using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace HIMBot
{
    internal class DebugTools
    {
        private TwitchClient client;
        private PointsTracker points;
        private UsersManager usersManager;

        public DebugTools(TwitchClient _client, PointsTracker _points, UsersManager _usersManager)
        {
            client = _client;
            points = _points;
            usersManager = _usersManager;
        }

        public void Execute(OnMessageReceivedArgs e)
        {
            string command = e.ChatMessage.Message.Replace("Debug ", "");

            switch (command.ToLower())
            {
                case "save":
                    points.Save();
                    usersManager.Save();
                    break;
            }
        }
    }
}