using ReadBook.Pages.ResetPassword;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReadBook
{
    public partial class ResetPassword : Window
    {
        readonly SqlConnection connection;
        private Uri page = new Uri("Pages/ResetPassword/InputPhone.xaml", UriKind.Relative);
        Page currentPage;
        private string code;
        private string phone;

        public ResetPassword()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);
            InitializeComponent();
            ContentFrame.Navigate(page);
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

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ContentFrame.Source == new Uri("Pages/ResetPassword/InputPhone.xaml",UriKind.Relative))
                {
                    connection.Open();
                    SqlCommand query = new SqlCommand("SELECT * FROM Читатели WHERE [Номер телефона]=@number", connection);

                    bool access = false;

                    long textInt = 0;
                    if (((InputPhone)currentPage).NumberTextBox.Text[0] == '+')
                    {
                        textInt = Convert.ToInt64(((InputPhone)currentPage).NumberTextBox.Text.Remove(0, 1));
                    }
                    else
                    {
                        textInt = Convert.ToInt64(((InputPhone)currentPage).NumberTextBox.Text);
                    }
                    phone = ((InputPhone)currentPage).NumberTextBox.Text = textInt.ToString("+#(###)###-##-##");

                    query.Parameters.AddWithValue("@number", ((InputPhone)currentPage).NumberTextBox.Text);

                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        access = true;
                    }
                    reader.Close();

                    if (access)
                    {
                        code = GenerateCode();

                        SmsRu.SmsRu sms = new SmsRu.SmsRu(ConfigurationManager.AppSettings["smsRuApi"]);

                        sms.Send(((InputPhone)currentPage).NumberTextBox.Text.Trim(), code);

                        ContentFrame.Navigate(new Uri("Pages/ResetPassword/InputCode.xaml", UriKind.Relative));
                    }
                    else
                    {
                        var text = "Введен неверный номер телефона";
                        Error window = new Error(text);
                        window.Show();
                    }
                }
                else if(ContentFrame.Source == new Uri("Pages/ResetPassword/InputCode.xaml", UriKind.Relative))
                {
                    if(((InputCode)currentPage).CodeTextBox.Text == code)
                    {
                        ContentFrame.Navigate(new Uri("Pages/ResetPassword/ChangePassword.xaml", UriKind.Relative));
                    }
                }
                else if (ContentFrame.Source == new Uri("Pages/ResetPassword/ChangePassword.xaml", UriKind.Relative))
                {
                    if (((ChangePassword)currentPage).NewPassBox.Password == ((ChangePassword)currentPage).RepeatPassBox.Password)
                    {
                        connection.Open();

                        SqlCommand query = new SqlCommand("UPDATE [Читатели] SET [Пароль]='@password' WHERE [Номер телефона]='@phone'", connection);

                        query.Parameters.AddWithValue("@password", ((ChangePassword)currentPage).NewPassBox.Password);
                        query.Parameters.AddWithValue("@phone", phone);
                        query.ExecuteNonQuery();

                        MessageBox.Show("Пароль успешно изменён", "Успешно");
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

        private string GenerateCode()
        {
            string code = "";
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                code += rnd.Next(0, 9).ToString();
            }

            return code;
        }

        private void ContentFrame_ContentRendered(object sender, EventArgs e)
        {
            currentPage = ContentFrame.Content as Page;
        }
    }
}
