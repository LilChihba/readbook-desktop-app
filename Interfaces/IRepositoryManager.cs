using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        ICommentRepository Comment { get; }
        ILibraryRepository Library { get; }
        ICatalogRepository Catalog { get; }

        void Save();
    }
}
