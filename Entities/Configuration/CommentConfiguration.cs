using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasData
            (
                new Comment
                {
                    Id = Guid.NewGuid(),
                    BookId = new Guid("cdf4a3a6-27cf-4fff-8d44-2d9ddf9d5b7c"),
                    UserId = new Guid("8fb3eda3-aa8a-4a1e-899b-006d376d34fe"),
                    Date = DateTime.Now,
                    Preview = "Крутая книга! :)"
                }
            );
        }
    }
}
