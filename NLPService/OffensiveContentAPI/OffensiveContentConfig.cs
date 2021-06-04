using System;

namespace OffensiveContentAPI
{
    public class OffensiveContentConfig
    {
        private OffensiveContentConfig() { }

        private static readonly Lazy<OffensiveContentConfig> lazy = new Lazy<OffensiveContentConfig>(() => new OffensiveContentConfig());

        public static OffensiveContentConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _suscriptionKey;
        private string _endpoint;

        public string SuscriptionKey
        {
            get { return _suscriptionKey; }
            set { _suscriptionKey = value; }
        }

        public string Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }
    }
}
