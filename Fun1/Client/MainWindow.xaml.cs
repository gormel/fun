using System;
using System.Linq;
using System.Windows;
using Interfaces;
using SuperCore.Core;
using System.Threading;
using System.Windows.Threading;
using SuperCore.Async.SyncContext;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SuperClient mClient;
        
        private ILoginServer mLoginServer;
        private IRoomServer mRoom;

        public MainWindow()
        {
            InitializeComponent();
            
            mClient = new SuperClient(new SuperWPFSyncContext());
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            mRoom.Move();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            mRoom.Rotate(Direction.Left);
        }

        private void Top_Click(object sender, RoutedEventArgs e)
        {
            mRoom.Rotate(Direction.Up);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            mRoom.Rotate(Direction.Right);
        }

        private void Bottom_Click(object sender, RoutedEventArgs e)
        {
            mRoom.Rotate(Direction.Down);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            mClient.Connect("127.0.0.1", 666);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            mLoginServer = mClient.GetInstance<ILoginServer>();

            var lobby = mLoginServer.Login(Guid.NewGuid().ToString());
            
            var field = new Field(lobby);

            mRoom = field.RoomServer;

            //foreach (var user in field.RoomServer.OtherUsers.ToList())
            //{
            //    listener.UserUpdated(user);
            //}

            DataContext = field;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (mRoom == null)
                return;

            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    Move(Direction.Up);
                    break;
                case System.Windows.Input.Key.Right:
                    Move(Direction.Right);
                    break;
                case System.Windows.Input.Key.Down:
                    Move(Direction.Down);
                    break;
                case System.Windows.Input.Key.Left:
                    Move(Direction.Left);
                    break;
            }
        }

        private void Move(Direction moveDirection)
        {
            if (mRoom.Me.Direction == moveDirection)
            {
                mRoom.Move();
                return;
            }

            mRoom.Rotate(moveDirection);
        }
    }
}
