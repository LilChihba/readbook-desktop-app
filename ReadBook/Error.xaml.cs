using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        public Error(string text)
        {
            InitializeComponent();
            errorTextBlock.Text = text;
        }

        private void okeyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dragMoveZone_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
