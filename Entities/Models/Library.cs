using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Library
    {
        [Column("Идентификационный номер")]
        public Guid Id { get; set; }

        [Column("Идентификационный номер пользователя")]
        public Guid IdUser { get; set; }

        [Column("Идентификационный номер книги")]
        [ForeignKey(nameof(Catalog))]
        public Guid IdBook { get; set; }
        public Catalog Catalog { get; set; }

        [Column("Дата добавления")]
        public DateTime Date{ get; set; }

        [Column("Название")]
        public string Name { get; set; }
    }
}
