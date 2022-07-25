namespace RMQ.RabbitMQModels
{
    public class RabbitMQBasicQosModel
    {
        public uint PreFetchSize { get; set; }

        public ushort PreFetchCount { get; set; }

        public bool Global { get; set; }
    }
}
