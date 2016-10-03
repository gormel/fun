using System;
using System.Diagnostics;
using System.Linq;
using Interfaces;

namespace Client
{
    class RoomServerListener : IRoomServerListener
    {
        private readonly Field mField;

        public RoomServerListener(Field field)
        {
            mField = field;
        }
        
        public void UserUpdated(IUserInfo obj)
        {
            if (mField?.RoomServer == null)
            {
                return;
            }

            var user = mField.Users.FirstOrDefault(u => u.Nick == obj.Nick);
            if (user == null)
            {
                user = new User(mField);
                mField.Users.Add(user);
            }

            user.Nick = obj.Nick;
            user.X = obj.X * user.Width;
            user.Y = -obj.Y * user.Height;
            user.SkinImage = !obj.Alive ? "GrayTank.png" :
                         obj.Nick == mField.RoomServer.Me.Nick ? "GreenTank.png" :
                         "RedTank.png";

            user.Direction = (int)obj.Direction * 90;
        }

        public void LaserFiered(int x, int y, Direction direction, int lenght)
        {
            if (mField?.RoomServer == null)
                return;

            var fire = new LaserFire()
            {
                X = x * mField.CellWidth,
                Y = y * mField.CellHeight
            };

            int dir = (int)direction;
            var len = (lenght > 0 ? lenght : 1000) * (dir % 2 == 0 ? mField.CellHeight : mField.CellWidth);

            switch (direction)
            {
                case Direction.Down:
                    fire.X1 = fire.X;
                    fire.Y1 = fire.Y + len;
                    break;
                case Direction.Up:
                    fire.X1 = fire.X;
                    fire.Y1 = fire.Y + len;
                    break;
                case Direction.Left:
                    fire.X1 = fire.X - len;
                    fire.Y1 = fire.Y;
                    break;
                case Direction.Right:
                    fire.X1 = fire.X + len;
                    fire.Y1 = fire.Y;
                    break;
            }

            mField.Fires.Add(fire);
        }
    }
}
