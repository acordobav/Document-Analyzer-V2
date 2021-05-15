using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerMongoDB.Configuration
{
    public class DataHandlerMongoDBConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private DataHandlerMongoDBConfig() { }

        private static readonly Lazy<DataHandlerMongoDBConfig> lazy = new Lazy<DataHandlerMongoDBConfig>(() => new DataHandlerMongoDBConfig());

        public static DataHandlerMongoDBConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _ConnectionString;
        private string _DataBaseName;

        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        public string DataBaseName
        {
            get { return _DataBaseName; }
            set { _DataBaseName = value; }
        }
    }
}
