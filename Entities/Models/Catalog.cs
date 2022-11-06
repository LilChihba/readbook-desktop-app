using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;

namespace Entities.Models
{
    public class Catalog
    {
        [Column("Идентификационный номер книги")]
        public Guid Id { get; set; }

        [Column("ISBN")]
        [Required(ErrorMessage = "ISBN является обязательным полем.")]
        public string ISBN { get; set; }

        [Column("Название")]
        [Required(ErrorMessage = "Название является обязательным полем.")]
        public string Name { get; set; }

        [Column("Автор")]
        public string Author { get; set; }

        [Column("Год издания")]
        [Required(ErrorMessage = "Год издания является обязательным полем.")]
        public int Year { get; set; }

        [Column("Количество страниц")]
        [Required(ErrorMessage = "Количество страниц является обязательным полем.")]
        public int Pages { get; set; }

        [Column("Жанр")]
        [Required(ErrorMessage = "Жанр является обязательным полем.")]
        public string Genre { get; set; }

        [Column("Описание")]
        public string Description { get; set; }

        [Column("Картинка")]
        public BitmapImage Image { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
