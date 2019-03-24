using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreBlog.Data.EntityFramework {
    using Abstractions;
    using Abstractions.Posts;
    using Abstractions.Users;
    using Posts;
    using Users;

    public class EfUnitOfWork : IUnitOfWork {
        private readonly BloggingContext _databaseContext;

        private readonly Lazy<IBlogPostRepository> _blogPostRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        private bool _disposed;

        public EfUnitOfWork(BloggingContext databaseContext) {
            _databaseContext = databaseContext;

            _blogPostRepository = new Lazy<IBlogPostRepository>(GetBlogPostRepository);
            _userRepository = new Lazy<IUserRepository>(GetUserRepository);
        }

        public IBlogPostRepository Posts => _blogPostRepository.Value;

        public IUserRepository Users => _userRepository.Value;

        private IBlogPostRepository GetBlogPostRepository() {
            return new BlogPostRepository(_databaseContext);
        }

        private IUserRepository GetUserRepository() {
            return new UserRepository(_databaseContext);
        }

        public async Task Commit() {
            await _databaseContext.SaveChangesAsync();
        }

        private void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }

            _databaseContext.Dispose();

            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Reject() {
            var changes = _databaseContext.ChangeTracker.Entries()
                .Where(c => c.State != EntityState.Unchanged && c.State != EntityState.Detached);

            foreach (var change in changes) {
                if (change.State == EntityState.Added) {
                    change.State = EntityState.Detached;
                }

                change.Reload();
            }
        }
    }
}
