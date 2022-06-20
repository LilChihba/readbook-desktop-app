using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            loginTextBox.Text = "Логин";
            loginTextBox.Foreground = Brushes.LightGray;
            loginTextBox.GotFocus += RemoveText;
            loginTextBox.LostFocus += AddText;

            passwordTextBox.Password = "Пароль";
            passwordTextBox.Foreground = Brushes.LightGray;
            passwordTextBox.GotFocus += RemoveText;
            passwordTextBox.LostFocus += AddText;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void RollupImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (loginTextBox.Text == "Логин")
            {
                loginTextBox.Text = "";
                loginTextBox.Foreground = Brushes.Black;
            }
            if (passwordTextBox.Password == "Пароль")
            {
                passwordTextBox.Password = "";
                passwordTextBox.Foreground = Brushes.Black;
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginTextBox.Text))
            {
                loginTextBox.Text = "Логин";
                loginTextBox.Foreground = Brushes.LightGray;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Password))
            {
                passwordTextBox.Password = "Пароль";
                passwordTextBox.Foreground = Brushes.LightGray;
            }
        }
    }
}
