using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Server
{
    class UserInfo : IUserInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
        public bool Alive { get; set; }
        public string Nick { get; set; }
    }
}
