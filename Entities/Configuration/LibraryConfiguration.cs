using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class LibraryConfiguration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.HasData
            (
                new Library
                {
                    Id = new Guid("7a061304-276a-4e4c-85b8-1bccf07bea36"),
                    IdUser = new Guid("8fb3eda3-aa8a-4a1e-899b-006d376d34fe"),
                    IdBook = new Guid("cdf4a3a6-27cf-4fff-8d44-2d9ddf9d5b7c"),
                    Date = DateTime.Now,
                    Name = "Колобок"
                }
            );
        }
    }
}
