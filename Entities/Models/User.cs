using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class User
    {
        [Column("Идентификационный номер пользователя")]
        public Guid Id { get; set; }

        [Column("Имя")]
        public string Name { get; set; }

        [Column("Фамилия")]
        public string LastName { get; set; }

        [Column("Отчество")]
        public string MiddleName { get; set; }

        [Column("Дата рождения")]
        public DateTime Date { get; set; }

        [Column("Номер телефона")]
        public string PhoneNumber { get; set; }

        [Column("Почта")]
        [Required(ErrorMessage = "Почта является обязательным полем.")]
        public string EMail { get; set; }

        [Column("Логин")]
        [Required(ErrorMessage = "Логин является обязательным полем.")]
        public string Login { get; set; }

        [Column("Пароль")]
        [Required(ErrorMessage = "Пароль является обязательным полем.")]
        public string Password { get; set; }

        public ICollection<Library> Libraries { get; set; }
    }
}
