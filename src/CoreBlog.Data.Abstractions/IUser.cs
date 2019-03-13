using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBlog.Data.Abstractions
{
    public interface IUser
    {
        Guid UserId { get; set; }

        string EmailAddress { get; set; }

        string DisplayName { get; set; }
    }
}
