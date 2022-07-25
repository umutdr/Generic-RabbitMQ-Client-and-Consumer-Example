using RabbitMQ.Client;

namespace RMQ.RabbitMQModels
{
    public class RabbitMQPublisherModel
    {
        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

        public string QueueName { get; set; }

        public IBasicProperties BasicProperties { get; set; }
    }
}
