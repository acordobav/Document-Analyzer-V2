using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Repository;

namespace DataHandlerMongoDB.Factory
{
    public class MongoRepositoryFactory : IMongoRepositoryFactory
    {
        public IMongoRepository<TDocument> Create<TDocument>() where TDocument : IDocument
        {
            // Settings of the MongoClient
            string connectionString = DataHandlerMongoDBConfig.Config.ConnectionString;
            string databaseName = DataHandlerMongoDBConfig.Config.DataBaseName;
            
            // Creation of the Database
            IMongoDatabase database = new MongoClient(connectionString).GetDatabase(databaseName);

            return new MongoRepository<TDocument>(database);
        }
    }
}
