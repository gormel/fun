using System;
using System.Windows;
using Interfaces;
using SuperCore.Core;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SuperClient mClient = new SuperClient();
        private ILoginServer mLoginServer;
        private IRoomServer mRoom;

        public MainWindow()
        {
            InitializeComponent();
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

            mRoom = lobby.Join();

            var field = new Field(mRoom);

            DataContext = field;
        }
    }
}
