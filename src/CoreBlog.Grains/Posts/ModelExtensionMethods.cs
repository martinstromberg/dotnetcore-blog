namespace CoreBlog.Grains.Posts {
    using Data.Abstractions.Posts;
    using GrainModels.Posts;

    public static class ModelExtensionMethods {
        public static BlogPost ToGrainModel(this IBlogPost post) {
            return new BlogPost {
                BlogPostId = post.BlogPostId,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created.GetValueOrDefault(),
                AuthorId = post.AuthorId
            };
        }

        public static IBlogPost ToDataModel(this BlogPost post) {
            return new IncomingBlogPost {
                BlogPostId = post.BlogPostId,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created,
                AuthorId = post.AuthorId
            };
        }
    }
}