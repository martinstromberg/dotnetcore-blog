using System;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions {
    using Posts;
    using Users;

    public interface IUnitOfWork : IDisposable {
        IBlogPostRepository Posts { get; }
        IUserRepository Users { get; }

        Task Commit();

        void Reject();
    }
}
