using System;

namespace CoreBlog.Data.Abstractions.Posts {
    public static class IBlogPostExtensionMethods {
        public static TType CopyTo<TType>(this IBlogPost source, TType destination) where TType : IBlogPost {
            if (destination == null) {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.BlogPostId = source.BlogPostId;
            destination.Title = source.Title;
            destination.Content = source.Content;
            destination.Created = source.Created;
            destination.LastUpdated = source.LastUpdated;
            destination.AuthorId = source.AuthorId;

            return destination;
        }
    }
}