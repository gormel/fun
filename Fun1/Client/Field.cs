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

        public IRoomServer RoomServer { get; }

        public Field(ILobbyServer lobbyServer)
        {
            RoomServer = lobbyServer.Join(new RoomServerListener(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
