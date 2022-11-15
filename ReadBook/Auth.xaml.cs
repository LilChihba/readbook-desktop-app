using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    public partial class Auth : Window
    {
        readonly SqlConnection connection;

        public Auth()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);
            InitializeComponent();

            if (App.Current.Properties[0] != null && App.Current.Properties[1] != null)
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE Логин=@login and Пароль=@password", connection);

                bool access = false;

                query.Parameters.AddWithValue("@login", App.Current.Properties[0].ToString());
                query.Parameters.AddWithValue("@password", App.Current.Properties[1].ToString());

                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    access = true;
                }
                reader.Close();

                if (access)
                {
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
                }
                connection.Close();
            }
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

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password.Length > 0)
            {
                WaterMarkPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                WaterMarkPassword.Visibility = Visibility.Visible;
            }
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE Логин=@login and Пароль=@password", connection);

                bool access = false;
                int id = 0;

                query.Parameters.AddWithValue("@login", LoginTextBox.Text);
                query.Parameters.AddWithValue("@password", PasswordTextBox.Password);

                SqlDataReader reader = query.ExecuteReader();
                while(reader.Read())
                {
                    int idIndex = reader.GetOrdinal("Номер читательского билета");
                    id = reader.GetInt32(idIndex);
                    access = true;
                }
                reader.Close();

                if(access)
                {
                    App.Current.Properties[2] = id;
                    if (AutologinCheckBox.IsChecked == true)
                    {
                        App.Current.Properties[0] = LoginTextBox.Text.Trim();
                        App.Current.Properties[1] = PasswordTextBox.Password.Trim();
                    }
                    if (AutologinCheckBox.IsChecked == false)
                    {
                        App.Current.Properties[0] = null;
                        App.Current.Properties[1] = null;
                    }
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
                }
                else
                {
                    var text = "Введен неверный логин или пароль!";
                    Error window = new Error(text);
                    window.Show();
                }
            }
            catch(Exception ex)
            {
                var text = ex.ToString();
                Error window = new Error(text);
                window.Show();
            }
            finally
            {
                connection.Close();
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            Reg window = new Reg();
            window.Show();
            this.Close();
        }

        private void ResetPassBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetPassword page = new ResetPassword();
            page.Show();
            this.Close();
        }
    }
}
