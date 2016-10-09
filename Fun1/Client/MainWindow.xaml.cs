using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Interfaces;
using SuperCore.Core;
using System.Threading;
using System.Windows.Input;
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
            
            mClient = new SuperClient(new SuperWpfSyncContext());
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
        }

        bool connected = false;
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (connected)
                return;
            connected = true;
            mClient.Connect("127.0.0.1", 666);
            mLoginServer = mClient.GetInstance<ILoginServer>();

            var lobby = mLoginServer.Login(Guid.NewGuid().ToString());
            
            var field = new Field(lobby);

            mRoom = field.RoomServer;

            DataContext = field;
        }

        private bool mDown;

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (mDown)
                return;
            mDown = true;

            if (mRoom == null)
                return;

            switch (e.Key)
            {
                case Key.Up:
                    Move(Direction.Up);
                    break;
                case Key.Right:
                    Move(Direction.Right);
                    break;
                case Key.Down:
                    Move(Direction.Down);
                    break;
                case Key.Left:
                    Move(Direction.Left);
                    break;
                case Key.Space:
                    mRoom.Fire();
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

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            mDown = false;
        }
    }
}
