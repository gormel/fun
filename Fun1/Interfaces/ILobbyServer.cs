namespace Interfaces
{
    public interface ILobbyServer
    {
        IRoomServer Join(IRoomServerListener listener);
    }
}
