using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Server
{
    class Room
    {
        public List<RoomServer> RoomServers { get; } = new List<RoomServer>();

        public IEnumerable<UserInfo> Users => RoomServers.Select(s => s.User);

        public RoomServer CreateServer(string nick, IRoomServerListener listener)
        {
            if (RoomServers.Any(s => s.Nick == nick))
                return null;

            var result = new RoomServer(this, nick, listener);
            return result;
        }
    }
}
