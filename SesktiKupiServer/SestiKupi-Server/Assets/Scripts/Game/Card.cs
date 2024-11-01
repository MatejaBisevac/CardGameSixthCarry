using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    public class Card : IComparable<Card>
    {
        public int Num { get; set; }
        public int Val { get; set; }
        public ushort PlayerId { get; set; }
        public int Column { get; set; }

        public bool MarkForDelete { get; set; }
        public Card(int num, int val) {
            Num = num;
            Val = val;
            PlayerId = 0;
            Column = 0;
            MarkForDelete = false;
        }

        public Card(int num, int val, ushort playerId) : this(num, val)
        {
            PlayerId = playerId;
            MarkForDelete = false;
        }

        public int CompareTo(Card? other)
        {
            if (this.Num > other.Num)
                return 1;
            else if (this.Num == other.Num && this.Val > other.Val)
                return 0;
            else return -1;
        }
    }
}
