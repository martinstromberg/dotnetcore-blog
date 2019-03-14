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
using GraphQL.Types;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Orleans;
using Orleans.Configuration;
using System.Threading;

namespace CoreBlog.WebApi
{
    using GrainClientServices;
    using GrainClientServices.Abstractions;
    using GraphQL;
    using GraphQL.Schema;
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
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IUserService, UserService>();

            // Add GraphQL
            services
                .AddGraphQLServices()
                .AddGraphQL(options => { options.ExposeExceptions = true; })
                .AddGraphTypes(ServiceLifetime.Singleton);

            // Add Orleans
            services.AddSingleton<IClusterClient>(provider => {
                var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();

                var clientBuilder = new ClientBuilder()
                    .Configure<ClusterOptions>(options => {
                        options.ClusterId = "dev";
                        options.ServiceId = "CoreBlog";
                    })
                    .ConfigureApplicationParts(parts => {
                        parts.AddFromApplicationBaseDirectory();
                    });

                if (hostingEnvironment.IsDevelopment()) {
                    clientBuilder = clientBuilder.UseLocalhostClustering();
                }

                var client = clientBuilder.Build();
                var reset = new ManualResetEvent(false);

                client.Connect(RetryFilter).ContinueWith(task => {
                    reset.Set();

                    return Task.CompletedTask;
                });

                reset.WaitOne();

                return client;

                async Task<bool> RetryFilter(Exception exception) {
                    provider.GetService<ILogger>()?.LogWarning(
                        exception, 
                        "Exception while attempting to connect to Orleans cluster"
                    );

                    await Task.Delay(TimeSpan.FromSeconds(2));

                    return true;
                }
            });

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

            app.UseGraphQL<BlogSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
