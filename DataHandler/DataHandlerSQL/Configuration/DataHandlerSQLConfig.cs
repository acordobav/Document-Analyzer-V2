using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerSQL.Configuration
{
    public sealed class DataHandlerSQLConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private DataHandlerSQLConfig() { }

        private static readonly Lazy<DataHandlerSQLConfig> lazy = new Lazy<DataHandlerSQLConfig>(() => new DataHandlerSQLConfig());

        public static DataHandlerSQLConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _ConnectionString;

        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
    }
}
