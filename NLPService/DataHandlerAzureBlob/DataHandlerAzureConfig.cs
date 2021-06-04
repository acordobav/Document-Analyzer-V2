using System;

namespace DataHandlerAzureBlob
{
    public sealed class DataHandlerAzureConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private DataHandlerAzureConfig() { }

        private static readonly Lazy<DataHandlerAzureConfig> lazy = new Lazy<DataHandlerAzureConfig>(() => new DataHandlerAzureConfig());

        public static DataHandlerAzureConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _folder_path;

        public string FolderPath
        {
            get { return _folder_path; }
            set { _folder_path = value; }
        }
    }
}

