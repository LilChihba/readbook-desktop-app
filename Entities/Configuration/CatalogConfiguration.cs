using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.HasData
            (
                new Catalog
                {
                    Id = new Guid("cdf4a3a6-27cf-4fff-8d44-2d9ddf9d5b7c"),
                    ISBN = "978-5-699-76423-5",
                    Name = "Колобок",
                    Author = "",
                    Year = 2019,
                    Pages = 48,
                    Genre = "Художественная литература",
                    Description = "«Люблю читать!» - это новая серия сказок для старшего дошкольного и младшего школьного возраста. В каждую книжку серии входят 4 сказки: в первых - наиболее простые и короткие слова, а в последней, самой длинной сказке книги, употребляются уже более сложные слова. Юный читатель может не только следить за увлекательным сюжетом сказки, но и набираться опыта чтения, переходя от простого к сложному.  В этом ему помогут крупный шрифт, слова с ударениями, простое построение предложений.  Книги серии полностью соответствуют требованиям САНПИН, по ним удобно и интересно учиться читать, узнавая и запоминая слова, разглядывая красочные рисунки.  Прочитав любую книгу серии, ребенок скажет: «Люблю читать!»"
                }
            );
        }
    }
}
