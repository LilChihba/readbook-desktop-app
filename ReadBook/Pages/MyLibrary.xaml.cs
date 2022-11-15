using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Controls;

namespace ReadBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyLibrary.xaml
    /// </summary>
    public partial class MyLibrary : Page
    {
        SqlConnection connection;
        public MyLibrary()
        {
            InitializeComponent();
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);

            List<LibraryModel> books = new List<LibraryModel>();

            SqlCommand query = new SqlCommand("SELECT [Библиотека книг].Название as Название, " +
                                                     "[Библиотека книг].[Дата добавления] as [Дата добавления], " +
                                                     "[Каталог книг].Автор as Автор, " +
                                                     "[Каталог книг].Картинка as Картинка, " +
                                                     "[Библиотека книг].[Идентификационный номер книги] as [Идентификационный номер книги] " +
                                                     "FROM [Библиотека книг] JOIN [Каталог книг] on [Каталог книг].ISBN = [Библиотека книг].ISBN " +
                                                     "WHERE [Библиотека книг].[Номер читательского билета]=@userId ", connection);

            query.Parameters.AddWithValue("userId", Convert.ToInt32(App.Current.Properties[2]));

            connection.Open();
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                int nameIndex = reader.GetOrdinal("Название");
                string name = reader.GetString(nameIndex).Trim();

                int authorIndex = reader.GetOrdinal("Автор");
                string author = reader.GetString(authorIndex).Trim();

                int dTimeIndex = reader.GetOrdinal("Дата добавления");
                DateTime dTime = reader.GetDateTime(dTimeIndex);

                int imgIndex = reader.GetOrdinal("Картинка");
                byte[] imgData = (byte[])reader[imgIndex];

                int idIndex = reader.GetOrdinal("Идентификационный номер книги");
                string id = Convert.ToString(reader.GetInt32(idIndex));

                if (imgData == null || imgData.Length == 0)
                {
                    return;
                }

                var culture = new CultureInfo("ru-RU");

                books.Add(new LibraryModel()
                {
                    Id = id,
                    Name = name,
                    Author = author,
                    DTime = "Дата добавления: " + dTime.ToString("dd.MM.yyyy HH:mm", culture),
                    Img = ByteConverter.convertByteToBitmapImage(imgData)
                });
            }
            ListBooks.ItemsSource = books;
            reader.Close();
            connection.Close();
        }

        private void TrashbinImg_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string id = ((Image)sender).Tag as string;

                connection.Open();

                SqlCommand query = new SqlCommand("DELETE FROM [Библиотека книг] WHERE [Идентификационный номер книги]=@id AND [Номер читательского билета]=@userId", connection);

                query.Parameters.AddWithValue("@id", id);
                query.Parameters.AddWithValue("@userId", Convert.ToInt32(App.Current.Properties[2]));

                query.ExecuteNonQuery();
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
