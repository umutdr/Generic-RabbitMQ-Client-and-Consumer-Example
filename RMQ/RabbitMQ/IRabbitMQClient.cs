using RabbitMQ.Client;
using System;

namespace RMQ.RabbitMQ
{
    public interface IRabbitMQClient
    {
        IModel GetChannel();
    }

    public class RabbitMQClient : IRabbitMQClient, IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQClient(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            { _connection = _connectionFactory.CreateConnection(); }

            return _connection;
        }

        public IModel GetChannel()
        {
            if (_channel == null)
            { _channel = GetConnection().CreateModel(); }

            return _channel;
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
        }
    }

}
