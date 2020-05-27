using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace HIMBot
{
    internal class Command
    {

        // Generate a random value in between 0 and 2
        private string sentence = "";
        private TwitchClient client;
        private OnMessageReceivedArgs e;
        private PointsTracker points;
        private Maths math = new Maths();

        public Command(TwitchClient _client, PointsTracker _points)
        {
            client = _client;
            points = _points;
        }

        internal void Execute(OnMessageReceivedArgs _e)
        {
            e = _e;

            string command = e.ChatMessage.Message.Replace("^-^ ", "");

            switch (command.ToLower())
            {
                case "lol":
                    client.SendMessage(TwitchInfo.ChannelName, $"{e.ChatMessage.DisplayName} is not funny..");
                    break;
                case "rules":
                    client.SendMessage(TwitchInfo.ChannelName, $"@{e.ChatMessage.DisplayName} Just be respectful to each other and don not spam. I will not make you have to think about what you say in chat, just be yourself but use some common sence.");
                    break;
                case "insult":
                    Insult();
                    break;
                case "compliment":
                    Compliment();
                    break;
                case "help":
                    Help();
                    break;
                case "?":
                    Help();
                    break;
                case "discord":
                    client.SendMessage(TwitchInfo.ChannelName, $"Hey {e.ChatMessage.DisplayName} feel free to join our discord server https://discord.com/invite/6mnqq3t");
                    break;
                case "youtube":
                    client.SendMessage(TwitchInfo.ChannelName, $"Hey {e.ChatMessage.DisplayName} please subscribe to my channel https://www.youtube.com/user/Dragon2Gaming");
                    break;
                case "points":
                    points.Check(e.ChatMessage.DisplayName);
                    break;
                case "daily":
                    points.Daily(e.ChatMessage.DisplayName);
                    break;
                case "weekly":
                    points.Weekly(e.ChatMessage.DisplayName);
                    break;
            }
        }

        private void Help()
        {
            client.SendMessage(TwitchInfo.ChannelName, "This bot uses ^-^ as an idetifier. just add the name of a command after the idetifier. more info at the Bot panel down below");
        }

        private void Compliment()
        {
            int _val = math.RndInt(0, 2);

            switch (_val)
            {
                case 0:
                    sentence = $"You're amazing {e.ChatMessage.DisplayName}!";
                    break;
                case 1:
                    sentence = $"haha you're funny... I'll kill you last {e.ChatMessage.DisplayName}!";
                    break;
                case 2:
                    sentence = $"Hey {e.ChatMessage.DisplayName} I really appriciate that you took the time to join the stream.";
                    break;
            }

            client.SendMessage(TwitchInfo.ChannelName, sentence.ToString());
        }

        private void Insult()
        {
            int _val = math.RndInt(0, 2);

            switch (_val)
            {
                case 0:
                    sentence = $"{e.ChatMessage.DisplayName} you're more depressing than my depression!";
                    break;
                case 1:
                    sentence = $"your name is worse than Chad and Karen combined {e.ChatMessage.DisplayName}!";
                    break;
                case 2:
                    sentence = $"hold still {e.ChatMessage.DisplayName} I'm trying to imagine you but less depressing.";
                    break;
            }

            client.SendMessage(TwitchInfo.ChannelName, sentence.ToString());
        }
    }
}