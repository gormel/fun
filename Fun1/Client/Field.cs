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
            user.Y = obj.Y * user.Height;
            user.Color = !obj.Alive
                ? new SolidColorBrush(Colors.Gray)
                : obj.Nick == mRoomServer.Me.Nick ? new SolidColorBrush(Colors.Green) : 
                new SolidColorBrush(Colors.Red);

            switch (obj.Direction)
            {
                case Direction.Up:
                    user.Direction = 0;
                    break;
                case Direction.Right:
                    user.Direction = 90;
                    break;
                case Direction.Down:
                    user.Direction = 180;
                    break;
                case Direction.Left:
                    user.Direction = 270;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
