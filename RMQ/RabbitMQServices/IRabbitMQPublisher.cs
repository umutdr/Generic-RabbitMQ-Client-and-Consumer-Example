using RabbitMQ.Client;
using RMQ.RabbitMQ;
using RMQ.RabbitMQModels;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.RabbitMQServices
{
    public interface IRabbitMQPublisher
    {
        Task Publish(string message, RabbitMQPublisherModel publishModel);
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IRabbitMQClient _rabbitMQClient;

        public RabbitMQPublisher(IRabbitMQClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

        public Task Publish(string message, RabbitMQPublisherModel rabbitMQModel)
        {
            byte[] modelByte = Encoding.UTF8.GetBytes(message);

            IModel channel = _rabbitMQClient.GetChannel();
            channel.BasicPublish(
                exchange: rabbitMQModel.Exchange,
                routingKey: rabbitMQModel.RoutingKey,
                basicProperties: rabbitMQModel.BasicProperties,
                body: modelByte);

            return Task.CompletedTask;
        }
    }
}