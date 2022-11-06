using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData
            (
                new User
                {
                    Id = new Guid("8fb3eda3-aa8a-4a1e-899b-006d376d34fe"),
                    Name = "Данила",
                    LastName = "Юров",
                    MiddleName = "Алексеевич",
                    Date = new DateTime(2003, 11, 23),
                    PhoneNumber = "+79625929284",
                    EMail = "danila-yurov@mail.ru",
                    Login = "danilaY",
                    Password = "Pass123"
                }
            );
        }
    }
}
