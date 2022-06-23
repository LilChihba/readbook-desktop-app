using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        SqlConnection connection;

        public Auth()
        {
            string conStr = @"workstation id=mydbreadbook.mssql.somee.com;packet size=4096;user id=danila-yurov_SQLLogin_1;pwd=788domkbj4;data source=mydbreadbook.mssql.somee.com;persist security info=False;initial catalog=mydbreadbook";
            connection = new SqlConnection(conStr);
            InitializeComponent();
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

        private void passwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Password.Length > 0)
            {
                waterMarkPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                waterMarkPassword.Visibility = Visibility.Visible;
            }
        }

        private void authBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE Логин=@login and Пароль=@password", connection);

                bool access = false;

                query.Parameters.AddWithValue("@login", loginTextBox.Text);
                query.Parameters.AddWithValue("@password", passwordTextBox.Password);

                SqlDataReader reader = query.ExecuteReader();
                while(reader.Read())
                {
                    access = true;
                }
                reader.Close();

                if(access)
                {
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы ввели неверный логин или пароль! Повторите попытку", "Ошибка");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка подключения");
            }
            finally
            {
                connection.Close();
            }
        }

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            Reg window = new Reg();
            window.Show();
            this.Close();
        }
    }
}
