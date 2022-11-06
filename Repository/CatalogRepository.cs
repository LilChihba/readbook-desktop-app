using Entities.Models;
using Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CatalogRepository : RepositoryBase<Catalog>, ICatalogRepository
    {
        public CatalogRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
