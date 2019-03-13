using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CoreBlog.GraphQL.Services;
using CoreBlog.GraphQL.Types;
using GraphQL.Types;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using CoreBlog.GraphQL.Schema;

namespace CoreBlog.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<IBlogPostService, BlogPostService>();

            // Types
            services.AddSingleton<GuidGraphType>();
            services.AddSingleton<BlogPostType>();

            // Query
            services.AddSingleton<BlogPostsQuery>();

            // Schema
            services.AddSingleton<BlogPostsSchema>();

            // Dependency Resolver
            services.AddSingleton<IDependencyResolver>(provider => 
                new FuncDependencyResolver(provider.GetRequiredService));

            // Add GraphQL
            services.AddGraphQL(options => { options.ExposeExceptions = true; })
                .AddGraphTypes(ServiceLifetime.Singleton);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphQL<BlogPostsSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
