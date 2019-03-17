using System;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.CodeGeneration;
using Orleans.Configuration;
using Orleans.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

[assembly: KnownAssembly(typeof(CoreBlog.Grains.Abstractions.Posts.IBlogPostGrain))]
[assembly: KnownAssembly(typeof(CoreBlog.Grains.Posts.BlogPostGrain))]

namespace CoreBlog.SiloHost {
    using Data.EntityFramework;

    public static class Program {
        const string DevelopmentEnvironment = "Development";

        static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

        static ISiloHostBuilder SetClusteringEnvironment(this ISiloHostBuilder builder) {
            if (HostingEnvironment.EnvironmentName == DevelopmentEnvironment) {
                return builder.UseLocalhostClustering();
            }

            // TODO: Add code to set up production clustering

            return builder;
        }

        static IConfiguration Configuration;
        static IHostingEnvironment HostingEnvironment;
        static void Main(string[] args) {
            HostingEnvironment = GetHostingEnvironment();
            Configuration = GetConfiguration(HostingEnvironment, args);

            var siloHostBuilder = new SiloHostBuilder()
                .SetClusteringEnvironment()
                .Configure<ClusterOptions>(options => {
                    options.ClusterId = Configuration["Orleans:ClusterId"];
                    options.ServiceId = Configuration["Orleans:ServiceId"];
                })
                .Configure<EndpointOptions>(options => {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureApplicationParts(parts => {
                    parts.AddApplicationPart(typeof(Grains.Posts.BlogPostGrain).Assembly).WithReferences();
                })

                .ConfigureLogging(logging => {
                    logging
                        .AddConfiguration(Configuration)
                        .AddConsole();
                })

                .ConfigureServices(ConfigureServices);

            if (Configuration.GetValue<bool>("OrleansDashboard:Enabled", false)) {
                siloHostBuilder = siloHostBuilder.UseDashboard(options => {
                    options.Host = Configuration["OrleansDashboard:Host"];
                    options.Port = Configuration.GetValue<int>("OrleansDashboard:Port");

                    options.Username = Configuration["OrleansDashboard:Username"];
                    options.Password = Configuration["OrleansDashboard:Password"];

                    options.CounterUpdateIntervalMs = Configuration.GetValue<int>("OrleansDashboard:UpdateInterval", 1000);
                });
            }

            var siloHost = siloHostBuilder.Build();
                
            siloHost.StartAsync()
                
                .ContinueWith(task => {
                    Console.WriteLine("CoreBlog SiloHost is running...");

                    return task;
                });

            resetEvent.WaitOne();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHostingEnvironment>(HostingEnvironment);
            
            services.AddSingleton(new LoggerFactory());
            services.AddLogging(logging => {
                logging.AddConsole();
            });

            services.AddEntityFrameworkBloggingServices(Configuration);
        }

        static IHostingEnvironment GetHostingEnvironment() {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fileProvider = new PhysicalFileProvider(baseDirectory);

            return new HostingEnvironment {
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                ContentRootFileProvider = fileProvider,
                ContentRootPath = baseDirectory,
                EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DevelopmentEnvironment
            };
        }

        static IConfiguration GetConfiguration(IHostingEnvironment hostingEnvironment, string[] args) {
            return new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile(
                    provider: hostingEnvironment.ContentRootFileProvider,
                    path: "appsettings.json",
                    optional: false, 
                    reloadOnChange: true)
                .AddJsonFile(
                    provider: hostingEnvironment.ContentRootFileProvider,
                    path: $"appsettings.{hostingEnvironment.EnvironmentName}.json", 
                    optional: true, 
                    reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "ENVIRONMENT_")
                .AddCommandLine(args)
                .Build();
        }
    }
}
