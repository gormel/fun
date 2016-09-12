using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Lobby
    {
        public Room Room { get; } = new Room();

        List<LobbyServer> LobbyServers { get; } = new List<LobbyServer>(); 
        public LobbyServer CreateServer(string nick)
        {
            if (LobbyServers.Any(s => s.Nick == nick))
                return null;

            var result = new LobbyServer(this);
            result.Nick = nick;
            LobbyServers.Add(result);
            return result;
        }
    }
}
