using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Server
{
    class RoomServer : IRoomServer
    {
        private readonly Room mRoom;

        public UserInfo User { get; }

        public RoomServer(Room room, string nick)
        {
            mRoom = room;
            Nick = nick;

            User = new UserInfo
            {
                Alive = true,
                Direction = Direction.Up,
                Nick = Nick,
                X = 0,
                Y = 0
            };

            mRoom.RoomServers.Add(this);
            SendUpdated();
            foreach (var roomServer in mRoom.RoomServers)
            {
                UserUpdated?.Invoke(roomServer.User);
            }
        }

        public string Nick { get; }

        public event Action<IUserInfo> UserUpdated;

        public void Move()
        {
            switch (User.Direction)
            {
                case Direction.Up:
                    User.Y++;
                    break;
                case Direction.Right:
                    User.X++;
                    break;
                case Direction.Down:
                    User.Y--;
                    break;
                case Direction.Left:
                    User.X--;
                    break;
            }
            SendUpdated();
        }

        public void Rotate(Direction newDiection)
        {
            User.Direction = newDiection;
            SendUpdated();
        }

        private void SendUpdated()
        {
            foreach (var server in mRoom.RoomServers)
            {
                server.UserUpdated?.Invoke(User);
            }
        }

        public IEnumerable<IUserInfo> OtherUsers => mRoom.Users.Where(u => u != User);

        public IUserInfo Me => User;
    }
}
