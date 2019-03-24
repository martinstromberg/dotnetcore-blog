using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Posts {
    using Abstractions.Posts;
    using Data.Abstractions;
    using GrainModels.Posts;

    public class BlogPostRegistryGrain : Grain, IBlogPostRegistryGrain {
        private readonly IUnitOfWork _unitOfWork;

        public BlogPostRegistryGrain(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BlogPost>> Query() {
            return (await _unitOfWork.Posts.GetAll())
                .Select(ModelExtensionMethods.ToGrainModel)
                .ToList();
        }

        public async Task<Guid> Add(BlogPost blogPost) {
            var entity = blogPost.ToDataModel();

            _unitOfWork.Posts.Add(entity);
            await _unitOfWork.Commit();

            return entity.BlogPostId;
        }
    }
}