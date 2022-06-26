using System;
using System.Collections.Generic;
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
        SqlConnection connection;
        List<string> logins = new List<string>();
        bool backspace = false;


        public Reg()
        {
            string conStr = @"workstation id=mydbreadbook.mssql.somee.com;packet size=4096;user id=danila-yurov_SQLLogin_1;pwd=788domkbj4;data source=mydbreadbook.mssql.somee.com;persist security info=False;initial catalog=mydbreadbook";
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Auth window = new Auth();
            window.Show();
            this.Close();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (fnameTextBox.Text == "" || lnameTextBox.Text == "" || mnameTextBox.Text == "" || datebirthTextBox.Text == "" || numberTextBox.Text == "" || loginTextBox.Text == "" || passwordTextBox.Password == "")
                {
                    var text = "Вы не заполнили поля с данными!";
                    Error window = new Error(text);
                    window.Show();
                }
                else
                {
                    connection.Open();

                    SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE Логин=@login", connection);
                    query.Parameters.AddWithValue("@login", loginTextBox.Text);

                    string login = "";

                    SqlDataReader reader = query.ExecuteReader();
                    while(reader.Read())
                    {
                        int loginIndex = reader.GetOrdinal("Логин");
                        login = reader.GetString(loginIndex);
                    }
                    reader.Close();

                    if(loginTextBox.Text == login.Trim())
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
                        query2.Parameters.AddWithValue("@fname", fnameTextBox.Text);
                        query2.Parameters.AddWithValue("@lname", lnameTextBox.Text);
                        query2.Parameters.AddWithValue("@mname", mnameTextBox.Text);
                        query2.Parameters.AddWithValue("@dateb", DateTime.Parse(datebirthTextBox.Text));
                        query2.Parameters.AddWithValue("@number", numberTextBox.Text);
                        query2.Parameters.AddWithValue("@login", loginTextBox.Text);
                        query2.Parameters.AddWithValue("@password", passwordTextBox.Password);

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

        private void loginTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                connection.Open();

                foreach (string login in logins)
                {
                    if (login == loginTextBox.Text)
                    {
                        okImg.Visibility = Visibility.Hidden;
                        nonokImg.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                    {
                        nonokImg.Visibility = Visibility.Hidden;
                        okImg.Visibility = Visibility.Visible;
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

        private void numberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void datebirthTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0) || e.Text[0] == '.' || e.Text[0] == '-')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void loginTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.ToString() == "Space")
            {
                e.Handled = true;
            }
        }

        private void passwordTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Space")
            {
                e.Handled = true;
            }
        }

        private void passwordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void loginTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void datebirthTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (backspace)
            {
                backspace = false;
                return;
            }
            if (datebirthTextBox.Text.Length == 2 || datebirthTextBox.Text.Length == 5)
            {
                datebirthTextBox.Text += '.';
                datebirthTextBox.Select(datebirthTextBox.Text.Length, 0);
            }
        }

        private void datebirthTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Back")
            {
                if (datebirthTextBox.Text != "")
                {
                    if (datebirthTextBox.Text.Substring(datebirthTextBox.Text.Length - 1) == ".")
                    {
                        datebirthTextBox.Text = datebirthTextBox.Text.Remove(datebirthTextBox.Text.Length - 1);
                        datebirthTextBox.Select(datebirthTextBox.Text.Length, 0);
                        backspace = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void numberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                long text = 0;
                if (numberTextBox.Text[0] == '+')
                {
                    text = Convert.ToInt64(numberTextBox.Text.Remove(0, 1));
                }
                else
                {
                    text = Convert.ToInt64(numberTextBox.Text);
                }
                numberTextBox.Text = text.ToString("+#(###)###-##-##");
            }
            catch
            {
                return;
            }
        }

        private void numberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (numberTextBox.Text[0] == '8')
                {
                    numberTextBox.Text = "+7";
                    numberTextBox.Select(numberTextBox.Text.Length, 0);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
