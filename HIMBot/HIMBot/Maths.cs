using System;

namespace HIMBot
{
    internal class Maths
    {
        public int RndInt(int _min, int _max)
        {
            Random rnd = new Random();
            return rnd.Next(_min, _max + 1);
        }
    }
}