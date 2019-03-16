using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting.Internal;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading;

namespace CoreBlog.SiloHost {
    using Data.EntityFramework;
    using Grains;

    public static class Program {
        const string DevelopmentEnvironment = "Development";
        static readonly string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DevelopmentEnvironment;

        static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

        static ISiloHostBuilder SetClusteringEnvironment(this ISiloHostBuilder builder) {
            if (_environment == DevelopmentEnvironment) {
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

            Console.WriteLine(Configuration["Logging.LogLevel.Default"]);
            return;

            var siloHostBuilder = new SiloHostBuilder()
                .SetClusteringEnvironment()
                .Configure<ClusterOptions>(options => {
                    options.ClusterId = Configuration["Orleans.ClusterId"];
                    options.ServiceId = Configuration["Orleans.ServiceId"];
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

            if (Configuration.GetValue<bool>("OrleansDashboard.Enabled", false)) {
                siloHostBuilder = siloHostBuilder.UseDashboard(options => {
                    options.Host = Configuration["OrleansDashboard.Host"];
                    options.Port = Configuration.GetValue<int>("OrleansDashboard.Port");

                    options.Username = Configuration["OrleansDashboard.Username"];
                    options.Password = Configuration["OrleansDashboard.Password"];

                    options.CounterUpdateIntervalMs = Configuration.GetValue<int>("OrleansDashboard.UpdateInterval");
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
                //.AddEnvironmentVariables(prefix: "ENVIRONMENT_")
                //.AddCommandLine(args)
                .Build();
        }
    }
}
