using Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _userRepository;
        private ILibraryRepository _libraryRepository;
        private ICommentRepository _commentRepository;
        private ICatalogRepository _catalogRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext);
                return _userRepository;
            }
        }

        public ICatalogRepository Catalog
        {
            get
            {
                if (_catalogRepository == null)
                    _catalogRepository = new CatalogRepository(_repositoryContext);
                return _catalogRepository;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_repositoryContext);
                return _commentRepository;
            }
        }

        public ILibraryRepository Library
        {
            get
            {
                if (_libraryRepository == null)
                    _libraryRepository = new LibraryRepository(_repositoryContext);
                return _libraryRepository;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
