namespace CoreBlog.Grains.Posts {
    using Data.Abstractions.Posts;
    using GrainModels.Posts;

    public static class IBlogPostExtensionMethods {
        public static BlogPost ToGrainModel(this IBlogPost post) {
            return new BlogPost {
                BlogPostId = post.BlogPostId,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created.GetValueOrDefault(),
                AuthorId = post.AuthorId
            };
        }
    }
}