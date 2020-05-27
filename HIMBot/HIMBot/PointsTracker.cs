using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Client;

namespace HIMBot
{
    internal class PointsTracker
    {
        private string path = @"c:\Users\HiImMike\Desktop\Twitch\Points\Points.json";
        private Scores obj = new Scores();
        private TwitchClient client;
        private Maths math = new Maths();

        [Serializable]
        private class Score
        {
            public String userName = String.Empty;
            public int numOfPoints = 0;
            public DateTime lastDaily;
            public DateTime lastWeekly;
        }

        [Serializable]
        private class Scores
        {
            public List<Score> scores = new List<Score>();
        }

        public PointsTracker(TwitchClient _client)
        {
            client = _client;
            string strJsonObj = string.Empty;

            if (!File.Exists(path))
            {
                Scores scores = new Scores();
                scores.scores.Add(new Score());
                strJsonObj = JsonConvert.SerializeObject(scores);
                File.WriteAllText(path, strJsonObj);
            }

            strJsonObj = File.ReadAllText(path);
            obj = JsonConvert.DeserializeObject<Scores>(strJsonObj);
        }

        public void Check(string _userName)
        {
            for (int i = 0; i < obj.scores.Count; i++)
            {
                if (obj.scores[i].userName == _userName)
                {
                    if (obj.scores[i].numOfPoints == 1)
                    {
                        client.SendMessage(TwitchInfo.ChannelName, $"{_userName} you have 1 Point!");
                    }
                    else
                    {
                        client.SendMessage(TwitchInfo.ChannelName, $"{_userName} you have {obj.scores[i].numOfPoints} Points!");

                    }
                    return;
                }
            }
            client.SendMessage(TwitchInfo.ChannelName, $"{_userName} you have no points type '^-^ points' to get some!");
        }

        public void Daily(string _userName)
        {
            for (int i = 0; i < obj.scores.Count; i++)
            {
                if (obj.scores[i].userName == _userName)
                {
                    if (obj.scores[i].lastDaily < DateTime.Now.AddDays(-1))
                    {
                        obj.scores[i].lastDaily = DateTime.Now;
                        int points = math.RndInt(1, 50);
                        obj.scores[i].numOfPoints += points;
                        client.SendMessage(TwitchInfo.ChannelName, $"{_userName} got {points} Points!");
                        return;
                    }
                    else
                    {
                        client.SendMessage(TwitchInfo.ChannelName, $"Please wait untill {obj.scores[i].lastDaily.AddDays(1)} to use that again.");
                        return;
                    }
                }
            }
        }

        public void Weekly(string _userName)
        {
            for (int i = 0; i < obj.scores.Count; i++)
            {
                if (obj.scores[i].userName == _userName)
                {
                    if (obj.scores[i].lastWeekly < DateTime.Now.AddDays(-7))
                    {
                        obj.scores[i].lastWeekly = DateTime.Now;
                        int points = math.RndInt(50, 200);
                        obj.scores[i].numOfPoints += points;
                        client.SendMessage(TwitchInfo.ChannelName, $"{_userName} got {points} Points!");
                        return;
                    }
                    else
                    {
                        client.SendMessage(TwitchInfo.ChannelName, $"Please wait untill {obj.scores[i].lastWeekly.AddDays(7)} to use that again.");
                        return;
                    }
                }
            }

            Score score = new Score();
            score.userName = _userName;
            obj.scores.Add(score);

            Daily(_userName);
        }

        public void Save()
        {
            string strJsonObj = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path, strJsonObj);
            strJsonObj = File.ReadAllText(path);
            obj = JsonConvert.DeserializeObject<Scores>(strJsonObj);
            Console.WriteLine("Saved Points");
        }
    }
}




