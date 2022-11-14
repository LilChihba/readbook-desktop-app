using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

            List<MyLibrary> books = new List<MyLibrary>();
            int id;
            int userId;
            string name;
            string author;
            byte[] imgData;

            SqlCommand query = new SqlCommand("SELECT * FROM [Каталог книг]", connection);
            connection.Open();
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                int ISBNIndex = reader.GetOrdinal("ISBN");
                isbn = reader.GetString(ISBNIndex).Trim();

                int nameIndex = reader.GetOrdinal("Название");
                name = reader.GetString(nameIndex).Trim();

                int authorIndex = reader.GetOrdinal("Автор");
                author = reader.GetString(authorIndex).Trim();

                int publisherIndex = reader.GetOrdinal("Издательство");
                publisher = reader.GetString(publisherIndex).Trim();

                int yearPublicIndex = reader.GetOrdinal("Год издания");
                yearPublic = reader.GetInt32(yearPublicIndex);

                int pagesIndex = reader.GetOrdinal("Количество страниц");
                pages = reader.GetInt32(pagesIndex);

                int genreIndex = reader.GetOrdinal("Жанр");
                genre = reader.GetString(genreIndex).Trim();

                int descriptionIndex = reader.GetOrdinal("Описание");
                description = reader.GetString(descriptionIndex).Trim();

                int imgIndex = reader.GetOrdinal("Картинка");
                imgData = (byte[])reader[imgIndex];

                if (imgData == null || imgData.Length == 0)
                {
                    return;
                }

                books.Add(new BookModel()
                {
                    ISBN = isbn,
                    Name = name,
                    Author = author,
                    Publisher = publisher,
                    YearPublic = yearPublic,
                    Pages = pages,
                    Genre = genre,
                    Description = description,
                    Img = ByteConverter.convertByteToBitmapImage(imgData)
                });
            }
            ListBooks.ItemsSource = books;
            reader.Close();
            connection.Close();
        }
    }
}
