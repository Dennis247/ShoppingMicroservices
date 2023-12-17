using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Shopping.Common.Settings;

namespace Shopping.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                // Register all consumers in the entry assembly
                configure.AddConsumers(Assembly.GetEntryAssembly());

                // Configure MassTransit to use RabbitMQ
                configure.UsingRabbitMq((context, configurator) =>
                {
                    // Retrieve configuration and settings
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

                    // Configure RabbitMQ host
                    configurator.Host(new Uri(rabbitMQSettings!.Host), h =>
                    {
                        h.Username(rabbitMQSettings.Username);
                        h.Password(rabbitMQSettings.Password);
                    });

                    // Configure endpoints using a custom formatter
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings!.ServiceName, false));

                    // Configure message retry policy
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            return services;
        }
    }
}