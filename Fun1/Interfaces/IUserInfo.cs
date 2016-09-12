namespace Interfaces
{
    public interface IUserInfo
    {
        int X { get; }
        int Y { get; }
        Direction Direction { get; }
        bool Alive { get; }
        string Nick { get; }
    }
}
