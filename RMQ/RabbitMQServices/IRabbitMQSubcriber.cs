using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQ.RabbitMQ;
using RMQ.RabbitMQModels;
using System.Threading.Tasks;

namespace RMQ.RabbitMQServices
{
    public interface IRabbitMQSubcriber
    {
        Task<IModel> Subscribe(string queueName, RabbitMQBasicQosModel BasicQosModel, AsyncEventHandler<BasicDeliverEventArgs> subcriber_received);
    }

    public class RabbitMQSubcriber : IRabbitMQSubcriber
    {
        private readonly IRabbitMQClient _rabbitMQClient;

        public RabbitMQSubcriber(IRabbitMQClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

        public Task<IModel> Subscribe(string queueName, RabbitMQBasicQosModel BasicQosModel, AsyncEventHandler<BasicDeliverEventArgs> subscriberReceivedEventHandler)
        {
            IModel channel = _rabbitMQClient.GetChannel();
            channel.BasicQos(BasicQosModel.PreFetchSize, BasicQosModel.PreFetchCount, BasicQosModel.Global);

            AsyncEventingBasicConsumer Subcriber = new(channel);
            channel.BasicConsume(queueName, autoAck: false, Subcriber);

            Subcriber.Received += subscriberReceivedEventHandler;

            return Task.FromResult(channel);
        }
    }
}