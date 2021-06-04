using System;

namespace SentimentAnalysisAPI
{
    public sealed class SentimentAnalysisConfig
    {
        private SentimentAnalysisConfig() { }

        private static readonly Lazy<SentimentAnalysisConfig> lazy = new Lazy<SentimentAnalysisConfig>(() => new SentimentAnalysisConfig());

        public static SentimentAnalysisConfig Config
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
