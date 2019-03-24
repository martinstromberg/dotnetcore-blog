using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Posts {
    using Data.Abstractions;
    using Abstractions.Posts;
    using Data.Abstractions.Posts;
    using GrainModels.Posts;

    public class BlogPostGrain : Grain, IBlogPostGrain {
        private readonly IUnitOfWork _unitOfWork;

        public BlogPostGrain(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<BlogPost> Find() {
            var blogPost = await _unitOfWork.Posts.Get(this.GetPrimaryKey());

            return blogPost.ToGrainModel();
        }

        public async Task Update(BlogPost blogPost) {
            var postEntity = await _unitOfWork.Posts.Get(this.GetPrimaryKey());

            postEntity.Title = blogPost.Title;
            postEntity.Content = blogPost.Content;

            await _unitOfWork.Commit();
        }
    }
}