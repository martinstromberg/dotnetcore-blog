using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBlog.Data.Abstractions.Posts {
    public interface IBlogPostWriteRepository : IWriteRepository<IBlogPost, Guid> {
    }
}
