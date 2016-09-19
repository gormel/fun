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
            if (mField == null)
                return;

            var user = mField.Users.FirstOrDefault(u => u.Nick == obj.Nick);
            if (user == null)
            {
                user = new User();
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
    }
}
