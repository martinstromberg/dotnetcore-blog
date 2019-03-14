using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading;

namespace CoreBlog.SiloHost {
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

        static void Main(string[] args) {
            new SiloHostBuilder()
                .SetClusteringEnvironment()
                .Configure<ClusterOptions>(options => {
                    options.ClusterId = "dev";
                    options.ServiceId = "CoreBlog";
                })
                .Configure<EndpointOptions>(options => {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureApplicationParts(parts => {
                    parts.AddApplicationPart(typeof(BlogPostGrain).Assembly).WithReferences();
                })

                .ConfigureLogging(logging => {
                    logging
                        .SetMinimumLevel(_environment == DevelopmentEnvironment
                            ? LogLevel.Information
                            : LogLevel.Warning)
                        .AddConsole();
                })

                .ConfigureServices(ConfigureServices)

                .UseDashboard(options => {
                    options.Host = "*";
                    options.Port = 8089;

                    options.Username = "admin";
                    options.Password = "admin";

                    options.CounterUpdateIntervalMs = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
                })

                .Build()
                
                .StartAsync()
                
                .ContinueWith(task => {
                    Console.WriteLine("CoreBlog SiloHost is running...");

                    return task;
                });

            resetEvent.WaitOne();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            
        }
    }
}
