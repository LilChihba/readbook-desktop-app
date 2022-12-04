using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    public partial class Profile : Window
    {
        private bool condition = false;
        private SqlConnection connection;

        public Profile()
        {
            InitializeComponent();
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);

            SqlCommand query = new SqlCommand("SELECT * FROM [Читатели] WHERE [Номер читательского билета] = @userId", connection);

            query.Parameters.AddWithValue("userId", Convert.ToInt32(App.Current.Properties[2]));

            connection.Open();
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                var culture = new CultureInfo("ru-RU");

                int fNameIndex = reader.GetOrdinal("Имя");
                string fName = reader.GetString(fNameIndex).Trim();

                int mNameIndex = reader.GetOrdinal("Фамилия");
                string mName = reader.GetString(mNameIndex).Trim();

                int lNameIndex = reader.GetOrdinal("Отчество");
                string lName = reader.GetString(lNameIndex).Trim();

                int dateBIndex = reader.GetOrdinal("Дата рождения");
                TextBoxDateBirth.Text = reader.GetDateTime(dateBIndex).ToString("dd.MM.yyyy", culture);

                int phoneIndex = reader.GetOrdinal("Номер телефона");
                TextBoxPhone.Text = reader.GetString(phoneIndex).Trim();

                int passwordIndex = reader.GetOrdinal("Пароль");
                TextBoxPassword.Text = reader.GetString(passwordIndex).Trim();

                TextBoxFirstName.Text = fName;
                TextBoxMiddleName.Text = mName;
                TextBoxLastName.Text = lName;
                FullNameLabel.Content = $"{fName} {mName[0]}. {lName[0]}.";
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

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!condition)
            {
                DataPanel.IsEnabled = true;
                EditBtn.Content = "Сохранить";
                condition = true;
            }
            else
            {
                try
                {
                    connection.Open();

                    SqlCommand query = new SqlCommand("UPDATE [Читатели] SET [Имя]=@name, [Фамилия]=@mname, [Отчество]=@lname, [Дата рождения]=@dateb, [Номер телефона]=@phone, [Пароль]=@password WHERE [Номер читательского билета]=@userId", connection);

                    var culture = new CultureInfo("en-US");
                    DateTime date = Convert.ToDateTime(TextBoxDateBirth.Text);

                    query.Parameters.AddWithValue("@name", TextBoxFirstName.Text.Trim());
                    query.Parameters.AddWithValue("@mname", TextBoxMiddleName.Text.Trim());
                    query.Parameters.AddWithValue("@lname", TextBoxLastName.Text.Trim());
                    query.Parameters.AddWithValue("@dateb", date.ToString("MM.dd.yyyy", culture));
                    query.Parameters.AddWithValue("@phone", TextBoxPhone.Text.Trim());
                    query.Parameters.AddWithValue("@password", TextBoxPassword.Text.Trim());
                    query.Parameters.AddWithValue("@userId", Convert.ToInt32(App.Current.Properties[2]));

                    query.ExecuteNonQuery();

                    DataPanel.IsEnabled = false;
                    EditBtn.Content = "Редактировать";
                    condition = false;
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
        }
    }
}
