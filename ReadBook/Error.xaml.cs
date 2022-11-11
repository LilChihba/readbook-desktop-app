using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    public partial class Error : Window
    {
        public Error(string text)
        {
            InitializeComponent();
            ErrorTextBlock.Text = text;
        }

        private void OkeyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragMoveZone_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
