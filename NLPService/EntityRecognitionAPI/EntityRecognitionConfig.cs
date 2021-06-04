using System;

namespace EntityRecognitionAPI
{
    public sealed class EntityRecognitionConfig
    {
        private EntityRecognitionConfig() { }

        private static readonly Lazy<EntityRecognitionConfig> lazy = new Lazy<EntityRecognitionConfig>(() => new EntityRecognitionConfig());

        public static EntityRecognitionConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _credential;
        private string _endpoint;

        public string Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        public string Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }
    }
}
