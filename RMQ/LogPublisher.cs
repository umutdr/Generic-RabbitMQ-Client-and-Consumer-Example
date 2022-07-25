using Microsoft.Extensions.Hosting;
using RMQ.RabbitMQModels;
using RMQ.RabbitMQServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMQ
{
    public class LogPublisher : BackgroundService
    {
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public LogPublisher(IRabbitMQPublisher rabbitMQPublisher)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                RabbitMQPublisherModel rabbitMQPublisherModel = new()
                {
                    Exchange = "amq.direct",
                    RoutingKey = "rawdata",
                };

                while (true)
                {
                    string messageText = DateTime.Now.ToLocalTime().ToString();

                    await _rabbitMQPublisher.Publish(messageText, rabbitMQPublisherModel);
                    Console.WriteLine(string.Format("Published message at {0} : {1}", DateTime.Now.ToLocalTime(), messageText));

                    if (stoppingToken.IsCancellationRequested)
                        break;

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }, stoppingToken);
        }
    }
}