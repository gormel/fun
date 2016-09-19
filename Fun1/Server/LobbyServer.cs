using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Server
{
    class LobbyServer : ILobbyServer
    {
        private readonly Lobby mLobby;

        public LobbyServer(Lobby lobby)
        {
            mLobby = lobby;
        }

        public string Nick { get; set; }
        public IRoomServer Join(IRoomServerListener listener)
        {
            return mLobby.Room.CreateServer(Nick, listener);
        }
    }
}
