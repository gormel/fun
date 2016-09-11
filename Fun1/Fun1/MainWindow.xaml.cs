using System.Windows;
using Fun1.Model;
using Fun1.View;

namespace Fun1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly FieldView mFieldView = new FieldView(new Field(10, 10));
        public MainWindow()
        {
            InitializeComponent();

            DataContext = mFieldView;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            dynamic dynSender = sender;
            dynSender.DataContext.CallOnClick();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic dynSender = sender;
            if (!dynSender.DataContext.Enabled)
                return;
            dynSender.DataContext.Fire();
        }
    }
}
