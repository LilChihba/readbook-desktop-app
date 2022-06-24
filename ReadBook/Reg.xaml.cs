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
                    MessageBox.Show("Вы не заполнили поля с данными!", "Ошибка");
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
                        MessageBox.Show("Такой логин уже занят", "Ошибка");
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
                MessageBox.Show(ex.ToString(), "Ошибка");
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
                MessageBox.Show(ex.ToString(), "Ошибка");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
