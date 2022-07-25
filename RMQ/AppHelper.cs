using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RMQ.RabbitMQ;
using RMQ.RabbitMQModels;
using RMQ.RabbitMQServices;
using System;
using System.IO;

namespace RMQ
{
    public static class AppHelper
    {
        public static IHostBuilder CreateHostBuilder()
        {
            var hostBuilder
                = Host
                    .CreateDefaultBuilder()
                    .ConfigureRequiredDependencies();

            return hostBuilder;
        }

        #region Private
        private static IConfiguration _configuration;

        private static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration
                    = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
            }

            return _configuration;
        }

        private static IHostBuilder ConfigureRequiredDependencies(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((serviceCollection) =>
                serviceCollection
                    .AddSingleton(serviceProvider => CreateRabbitMQConnectionFactory())
                    .AddScoped<IRabbitMQClient, RabbitMQClient>()
                    .AddSingleton<IRabbitMQSubcriber, RabbitMQSubcriber>()
                    .AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>()
                    .AddHostedService<LogPublisher>()
                    .AddHostedService<LogSubscriber>()
            );
        }

        private static ConnectionFactory CreateRabbitMQConnectionFactory()
        {
            RabbitMQAppSettingsModel rabbitMqAppSettingsModel =
                GetConfiguration().GetSection("RabbitMQ").Get<RabbitMQAppSettingsModel>();

            var connectionFactory = new ConnectionFactory()
            {
                DispatchConsumersAsync = true
            };

            if (string.IsNullOrWhiteSpace(rabbitMqAppSettingsModel.Connection.UriString))
            {
                connectionFactory.HostName = rabbitMqAppSettingsModel.Connection.HostName;
                connectionFactory.Port = rabbitMqAppSettingsModel.Connection.Port;
                connectionFactory.VirtualHost = rabbitMqAppSettingsModel.Connection.VirtualHostName;
                connectionFactory.UserName = rabbitMqAppSettingsModel.Connection.UserName;
                connectionFactory.Password = rabbitMqAppSettingsModel.Connection.Password;
            }
            else
            {
                connectionFactory.Uri = new Uri(rabbitMqAppSettingsModel.Connection.UriString);
            }

            return connectionFactory;
        }
        #endregion
    }
}