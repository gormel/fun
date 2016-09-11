using System;

namespace Fun1.Model
{
    public class Square
    {
        private static readonly Random mRnd = new Random();

        public int Color { get; set; } = mRnd.Next(4);
    }
}
