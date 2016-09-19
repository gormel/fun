using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Interfaces;

namespace Client
{
    public class Field : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>(); 

        private readonly IRoomServer mRoomServer;
        
        public Field(IRoomServer roomServer)
        {
            mRoomServer = roomServer;
            mRoomServer.UserUpdated += MRoomServer_UserUpdated;
            MRoomServer_UserUpdated(mRoomServer.Me);
        }

        private void MRoomServer_UserUpdated(IUserInfo obj)
        {
            var user = Users.FirstOrDefault(u => u.Nick == obj.Nick);
            if (user == null)
            {
                user = new User();
                Users.Add(user);
            }

            user.Nick = obj.Nick;
            user.X = obj.X * user.Width;
            user.Y = -obj.Y * user.Height;
            user.SkinImage = !obj.Alive ? "GrayTank.png": 
                         obj.Nick == mRoomServer.Me.Nick ? "GreenTank.png" : 
                         "RedTank.png";

            user.Direction = (int)obj.Direction * 90;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
