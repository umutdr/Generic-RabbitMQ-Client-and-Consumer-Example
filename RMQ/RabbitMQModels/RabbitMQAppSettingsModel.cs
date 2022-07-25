namespace RMQ.RabbitMQModels
{
    public class RabbitMQAppSettingsModel
    {
        public Connection Connection { get; set; }
    }

    public class Connection
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHostName { get; set; }
        public string UriString { get; set; }
    }
}
