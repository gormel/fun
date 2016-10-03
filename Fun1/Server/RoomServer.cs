using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Server
{
    class RoomServer : IRoomServer
    {
        private readonly Room mRoom;
        private IRoomServerListener mListener;

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

            SendUpdated();
            mRoom.RoomServers.Add(this);
        }

        public string Nick { get; }

        public void Move()
        {
            if (!User.Alive)
                return;
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
            if (!User.Alive)
                return;
            User.Direction = newDiection;
            SendUpdated();
        }

        private bool InFrontOf(IUserInfo user)
        {
            if (user == null)
                return false;
            switch (User.Direction)
            {
                case Direction.Up:
                case Direction.Down:
                    return User.X == user.X;
                case Direction.Right:
                case Direction.Left:
                    return User.Y == user.Y;
            }
            return false;
        }

        private int DistanceSq(IUserInfo user)
        {
            if (user == null)
                return 0;
            return User.X * user.X + User.Y * user.Y;
        }

        public void Fire()
        {
            if (!User.Alive)
                return;
            var userToKill = OtherUsers.Where(InFrontOf).OrderBy(DistanceSq).FirstOrDefault() as UserInfo;
            foreach (var server in mRoom.RoomServers)
            {
                server.mListener?.LaserFiered(User.X, User.Y, User.Direction, (int)Math.Sqrt(DistanceSq(userToKill)));
            }

            if (userToKill == null)
                return;

            mRoom.Kill(userToKill);
        }

        internal void SendUpdated()
        {
            foreach (var server in mRoom.RoomServers)
            {
                server.mListener?.UserUpdated(User);
            }
        }

        public IEnumerable<IUserInfo> OtherUsers => mRoom.Users.Where(u => u != User);

        public void AddListener(IRoomServerListener listener)
        {
            mListener = listener;
            foreach (var roomServer in mRoom.RoomServers)
            {
                mListener.UserUpdated(roomServer.User);
            }
        }

        public IUserInfo Me => User;
    }
}
