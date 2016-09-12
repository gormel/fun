using System.Collections.Generic;
using System.Linq;

namespace Server
{
    class Room
    {
        public List<RoomServer> RoomServers { get; } = new List<RoomServer>();

        public IEnumerable<UserInfo> Users => RoomServers.Select(s => s.User); 

        public RoomServer CreateServer(string nick)
        {
            if (RoomServers.Any(s => s.Nick == nick))
                return null;

            var result = new RoomServer(this, nick);
            return result;
        }
    }
}
