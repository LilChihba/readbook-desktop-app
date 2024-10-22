﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    public partial class Reg : Window
    {
        readonly SqlConnection connection;
        private List<string> logins = new List<string>();
        private List<string> phones = new List<string>();
        bool backspace = false;

        public Reg()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);
            InitializeComponent();

            connection.Open();
            SqlCommand query = new SqlCommand("SELECT Логин, [Номер телефона] FROM Читатели", connection);

            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                int loginIndex = reader.GetOrdinal("Логин");
                int emailIndex = reader.GetOrdinal("Номер телефона");
                logins.Add(reader.GetString(loginIndex).Trim());
                phones.Add(reader.GetString(emailIndex).Trim());
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
                if(FnameTextBox.Text == "" || LnameTextBox.Text == "" || MnameTextBox.Text == "" || DatebirthTextBox.Text == "" || NumberTextBox.Text == "" || LoginTextBox.Text == "" || PasswordTextBox.Password == "")
                {
                    var text = "Вы не заполнили поля с данными!";
                    Error window = new Error(text);
                    window.Show();
                }
                else
                {
                    connection.Open();

                    bool access = false;

                    foreach(string login in logins)
                    {
                        if (login == LoginTextBox.Text)
                        {
                            var text = "Такой логин уже существует, попробуйте другой!";
                            Error window = new Error(text);
                            window.Show();
                            access = false;
                        }
                        else
                            access = true;
                    }

                    foreach(string phone in phones)
                    {
                        if (NumberTextBox.Text == phone.Trim())
                        {
                            var text = "Этот номер телефона уже существует, попробуйте другую!";
                            Error window = new Error(text);
                            window.Show();
                            access = false;
                        }
                        else
                            access = true;
                    }
                    if(access == true)
                    {
                        SqlCommand query1 = new SqlCommand("SELECT Count(*) FROM Читатели", connection);
                        Int32 count = Convert.ToInt32(query1.ExecuteScalar()) + 1;
                        var culture = new CultureInfo("en-US");
                        SqlCommand query2 = new SqlCommand("INSERT INTO Читатели([Номер читательского билета], Имя, Фамилия, Отчество, [Дата рождения], [Номер телефона], Логин, Пароль) VALUES(@count, @fname, @lname, @mname, @dateb, @number, @login, @password)", connection);

                        DateTime date = Convert.ToDateTime(DatebirthTextBox.Text);

                        query2.Parameters.AddWithValue("@count", count);
                        query2.Parameters.AddWithValue("@fname", FnameTextBox.Text);
                        query2.Parameters.AddWithValue("@lname", LnameTextBox.Text);
                        query2.Parameters.AddWithValue("@mname", MnameTextBox.Text);
                        query2.Parameters.AddWithValue("@dateb", date.ToString("MM.dd.yyyy", culture));
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
                foreach (string login in logins)
                {
                    if (login == LoginTextBox.Text)
                    {
                        OkImgLogin.Visibility = Visibility.Hidden;
                        NonokImgLogin.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                    {
                        NonokImgLogin.Visibility = Visibility.Hidden;
                        OkImgLogin.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                var text = ex.ToString();
                Error window = new Error(text);
                window.Show();
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
            if (e.Key.ToString() == "Space")
            {
                e.Handled = true;
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
            catch (Exception ex)
            {
                var text = ex.ToString();
                Error window = new Error(text);
                window.Show();
            }
        }

        private void NumberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (NumberTextBox.Text != "")
                {
                    if (NumberTextBox.Text[0] == '8')
                    {
                        NumberTextBox.Text = "+7";
                        NumberTextBox.Select(NumberTextBox.Text.Length, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                var text = ex.ToString();
                Error window = new Error(text);
                window.Show();
            }
        }
    }
}
