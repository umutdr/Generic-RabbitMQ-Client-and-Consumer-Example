using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQ.RabbitMQModels;
using RMQ.RabbitMQServices;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RMQ
{
    public class LogSubscriber : BackgroundService
    {
        private IModel _Channel;
        private readonly IRabbitMQSubcriber _rabbitMQSubcriber;

        public LogSubscriber(IRabbitMQSubcriber rabbitMQSubcriber)
        {
            _rabbitMQSubcriber = rabbitMQSubcriber;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQBasicQosModel rabbitMQBasicQosModel = new()
            {
                PreFetchCount = 100
            };

            _Channel = await _rabbitMQSubcriber.Subscribe("rawdata-companyarealog", rabbitMQBasicQosModel, SubcriberReceivedEventHandler);
        }

        private Task SubcriberReceivedEventHandler(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                byte[] messageBytes = e.Body.ToArray();
                string messageText = Encoding.UTF8.GetString(messageBytes);

                Console.WriteLine(string.Format("Consumed message at {0} : {1}", DateTime.Now.ToLocalTime(), messageText));

                _Channel.BasicAck(e.DeliveryTag, false);

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                _Channel.BasicNack(e.DeliveryTag, false, false);
                throw;
            }
        }
    }
}