using System;

namespace RabbitMQ
{
    public sealed class RabbitMQConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private RabbitMQConfig() { }

        private static readonly Lazy<RabbitMQConfig> lazy = new Lazy<RabbitMQConfig>(() => new RabbitMQConfig());

        public static RabbitMQConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _connection_url;

        public string ConnectionUrl
        {
            get { return _connection_url; }
            set { _connection_url = value; }
        }
    }
}
