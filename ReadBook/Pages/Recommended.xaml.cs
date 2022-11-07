using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Configuration;

namespace ReadBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recommended.xaml
    /// </summary>
    public partial class Recommended : Page
    {
        SqlConnection connection;
        public Recommended()
        {
            InitializeComponent();
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);

            List<Book> books = new List<Book>();
            string isbn;
            string name;
            string author;
            string publisher;
            int yearPublic;
            int pages;
            string genre;
            string description;
            string url;
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

                books.Add(new Book()
                {
                    ISBN = isbn,
                    Name = name,
                    Author = author,
                    Publisher = publisher,
                    YearPublic = yearPublic,
                    Pages = pages,
                    Genre = genre,
                    Description = description,
                    Img = convertByteToBitmapImage(imgData)
                });
            }
            ListBooks.ItemsSource = books;
            reader.Close();
            connection.Close();
        }

        public BitmapImage convertByteToBitmapImage(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream mem = new MemoryStream(bytes);
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
            return image;
        }
    }
}