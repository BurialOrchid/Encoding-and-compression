using System;
using System.Collections.Generic;
using System.Text;

namespace List3Exercise5
{
    internal class Myletter
    {
        public Myletter(char letter, int num)
        {
            this.Character = letter;
            this.Quantity = num;
        }

        public char Character { get; set; }
        public int Quantity { get; set; }
        public double Probability { get; set; }
    }
}