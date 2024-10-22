﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Configuration;
using System.Data;
using System.Collections.ObjectModel;

namespace ReadBook.Pages
{
    public partial class Recommended : Page
    {
        SqlConnection connection;
        public Recommended()
        {
            InitializeComponent();
            string conStr = ConfigurationManager.ConnectionStrings["ReadBookEntities"].ConnectionString;
            connection = new SqlConnection(conStr);

            ObservableCollection<BookModel> books = new ObservableCollection<BookModel>();

            string isbn;
            string name;
            string author;
            string publisher;
            int yearPublic;
            int pages;
            string genre;
            string description;
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

        private void AddImg_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string ISBN = ((Image)sender).Tag as string;
                string name = "";

                connection.Open();

                SqlCommand query1 = new SqlCommand("SELECT * FROM [Каталог книг] WHERE [ISBN]=@isbnBook", connection);

                query1.Parameters.AddWithValue("@isbnBook", ISBN);

                SqlDataReader reader = query1.ExecuteReader();
                while (reader.Read())
                {
                    int nameIndex = reader.GetOrdinal("Название");
                    name = reader.GetString(nameIndex).Trim();
                }
                reader.Close();

                SqlCommand query2 = new SqlCommand("IF NOT EXISTS (SELECT ISBN FROM [Библиотека книг] WHERE [ISBN]=@isbn AND [Номер читательского билета]=@userId) INSERT INTO [Библиотека книг]([Номер читательского билета], [ISBN], [Название], [Дата добавления]) VALUES(@userId, @isbn, @name, @dateTime)", connection);

                query2.Parameters.AddWithValue("@userId", App.Current.Properties[2]);
                query2.Parameters.AddWithValue("@isbn", ISBN);
                query2.Parameters.AddWithValue("@name", name);
                query2.Parameters.Add("@dateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;

                query2.ExecuteNonQuery();
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