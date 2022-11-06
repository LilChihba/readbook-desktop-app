using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Comment
    {
        [Column("Идентификационный номер отзыва")]
        public Guid Id { get; set; }

        [Column("Идентификационный номер книги")]
        [ForeignKey(nameof(Catalog))]
        public Guid BookId { get; set; }
        public Catalog Catalog { get; set; }

        [Column("Идентификационный номер пользователя")]
        [ForeignKey(nameof(Users))]
        public Guid UserId { get; set; }
        public User Users { get; set; }

        [Column("Дата отзыва")]
        public DateTime Date { get; set; }

        [Column("Отзыв")]
        public string Preview { get; set; }
    }
}
