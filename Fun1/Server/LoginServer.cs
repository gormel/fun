using Interfaces;

namespace Server
{
    class LoginServer : ILoginServer
    {
        readonly Lobby mLobby = new Lobby();
        public ILobbyServer Login(string nick)
        {
            return mLobby.CreateServer(nick);
        }
    }
}
