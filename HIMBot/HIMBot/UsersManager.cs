using Newtonsoft.Json;
using System;
using System.IO;

namespace HIMBot
{

    internal class UsersManager
    {
        private string strJsonObj = string.Empty;
        private string path = @"c:\Users\HiImMike\Desktop\Twitch\Users.json";
        private Users users = new Users();

        public UsersManager()
        {
            if (!File.Exists(path))
            {
                Users users = new Users();
                users.users.Add(new User());
                strJsonObj = JsonConvert.SerializeObject(users);
                File.WriteAllText(path, strJsonObj);
            }

            strJsonObj = File.ReadAllText(path);
            users = JsonConvert.DeserializeObject<Users>(strJsonObj);
        }

        public void AddUser(string _userName)
        {
            User user = new User();
            user.userName = _userName;
            users.users.Add(user);
        }

        public Users GetUsers()
        {
            return users;
        }

        public void Save()
        {
            string strJsonObj = JsonConvert.SerializeObject(users);
            File.WriteAllText(path, strJsonObj);
            strJsonObj = File.ReadAllText(path);
            users = JsonConvert.DeserializeObject<Users>(strJsonObj);
            Console.WriteLine("Saved Users");
        }
    }
}
