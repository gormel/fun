using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Interfaces;

namespace Client
{
    public class Field : INotifyPropertyChanged
    {
        public int CellWidth { get; } = 30;
        public int CellHeight { get; } = 30;

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public ObservableCollection<LaserFire> Fires { get; } = new ObservableCollection<LaserFire>(); 

        public CompositeCollection VisibleObjects { get; } = new CompositeCollection();

        public IRoomServer RoomServer { get; }

        public Field(ILobbyServer lobbyServer)
        {
            VisibleObjects.Add(new CollectionContainer { Collection = Users });
            VisibleObjects.Add(new CollectionContainer { Collection = Fires });
            RoomServer = lobbyServer.Join();
            RoomServer.AddListener(new RoomServerListener(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
