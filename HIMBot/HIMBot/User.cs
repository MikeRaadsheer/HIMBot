using System;

namespace HIMBot
{
    [Serializable]
    class User
    {
        public string userName = string.Empty;
        public DateTime lastCommand = DateTime.Now;
    }
}
