using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        readonly SqlConnection connection;
        List<string> logins = new List<string>();
        bool backspace = false;


        public Reg()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);
            InitializeComponent();

            connection.Open();
            SqlCommand query = new SqlCommand("SELECT Логин FROM Читатели", connection);

            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                int loginIndex = reader.GetOrdinal("Логин");
                logins.Add(reader.GetString(loginIndex).Trim());
            }

            reader.Close();
            connection.Close();
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Auth window = new Auth();
            window.Show();
            this.Close();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FnameTextBox.Text == "" || LnameTextBox.Text == "" || MnameTextBox.Text == "" || DatebirthTextBox.Text == "" || NumberTextBox.Text == "" || LoginTextBox.Text == "" || PasswordTextBox.Password == "")
                {
                    var text = "Вы не заполнили поля с данными!";
                    Error window = new Error(text);
                    window.Show();
                }
                else
                {
                    connection.Open();

                    SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE Логин=@login", connection);
                    query.Parameters.AddWithValue("@login", LoginTextBox.Text);

                    string login = "";

                    SqlDataReader reader = query.ExecuteReader();
                    while(reader.Read())
                    {
                        int loginIndex = reader.GetOrdinal("Логин");
                        login = reader.GetString(loginIndex);
                    }
                    reader.Close();

                    if(LoginTextBox.Text == login.Trim())
                    {
                        var text = "Такой логин уже существует, попробуйте другой!";
                        Error window = new Error(text);
                        window.Show();
                    }
                    else
                    {
                        SqlCommand query1 = new SqlCommand("SELECT Count(*) FROM Читатели", connection);
                        Int32 count = Convert.ToInt32(query1.ExecuteScalar()) + 1;

                        SqlCommand query2 = new SqlCommand("INSERT INTO Читатели([Номер читательского билета], Имя, Фамилия, Отчество, [Дата рождения], [Номер телефона], Логин, Пароль) VALUES(@count, @fname, @lname, @mname, @dateb, @number, @login, @password)", connection);

                        query2.Parameters.AddWithValue("@count", count);
                        query2.Parameters.AddWithValue("@fname", FnameTextBox.Text);
                        query2.Parameters.AddWithValue("@lname", LnameTextBox.Text);
                        query2.Parameters.AddWithValue("@mname", MnameTextBox.Text);
                        query2.Parameters.AddWithValue("@dateb", DateTime.Parse(DatebirthTextBox.Text));
                        query2.Parameters.AddWithValue("@number", NumberTextBox.Text);
                        query2.Parameters.AddWithValue("@login", LoginTextBox.Text);
                        query2.Parameters.AddWithValue("@password", PasswordTextBox.Password);

                        query2.ExecuteNonQuery();
                        MainWindow window = new MainWindow();
                        window.Show();
                        this.Close();
                    }
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

        private void LoginTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                connection.Open();

                foreach (string login in logins)
                {
                    if (login == LoginTextBox.Text)
                    {
                        OkImg.Visibility = Visibility.Hidden;
                        NonokImg.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                    {
                        NonokImg.Visibility = Visibility.Hidden;
                        OkImg.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
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

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0) || e.Text[0] == '-' || e.Text[0] == '(' || e.Text[0] == ')' || e.Text[0] == '+')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void NumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                long text = 0;
                if (NumberTextBox.Text[0] == '+')
                {
                    text = Convert.ToInt64(NumberTextBox.Text.Remove(0, 1));
                }
                else
                {
                    text = Convert.ToInt64(NumberTextBox.Text);
                }
                NumberTextBox.Text = text.ToString("+#(###)###-##-##");
            }
            catch
            {
                return;
            }
        }

        private void NumberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (NumberTextBox.Text[0] == '8')
                {
                    NumberTextBox.Text = "+7";
                    NumberTextBox.Select(NumberTextBox.Text.Length, 0);
                }
            }
            catch
            {
                return;
            }
        }

        private void LoginTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.ToString() == "Space")
            {
                e.Handled = true;
            }
        }

        private void LoginTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text[0] >= 'А' && e.Text[0] <= 'Я') || (e.Text[0] >= 'а' && e.Text[0] <= 'я'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void PasswordTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Space")
            {
                e.Handled = true;
            }
        }

        private void PasswordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text[0] >= 'А' && e.Text[0] <= 'Я') || (e.Text[0] >= 'а' && e.Text[0] <= 'я'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void DatebirthTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (backspace)
            {
                backspace = false;
                return;
            }
            if (DatebirthTextBox.Text.Length == 2 || DatebirthTextBox.Text.Length == 5)
            {
                DatebirthTextBox.Text += '.';
                DatebirthTextBox.Select(DatebirthTextBox.Text.Length, 0);
            }
        }

        private void DatebirthTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Back")
            {
                if (DatebirthTextBox.Text != "")
                {
                    if (DatebirthTextBox.Text.Substring(DatebirthTextBox.Text.Length - 1) == ".")
                    {
                        DatebirthTextBox.Text = DatebirthTextBox.Text.Remove(DatebirthTextBox.Text.Length - 1);
                        DatebirthTextBox.Select(DatebirthTextBox.Text.Length, 0);
                        backspace = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void DatebirthTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
