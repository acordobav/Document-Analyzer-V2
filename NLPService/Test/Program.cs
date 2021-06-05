using System;
using System.Collections.Generic;

using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using DataHandlerMongoDB.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzer";
            IMongoRepositoryFactory repositoryFactory = new MongoRepositoryFactory();
            IMongoRepository<File> repository = repositoryFactory.Create<File>();

            
            File file = new File();
            file.Title = "prueba2.pdf";
            file.Owner = "10001";
         
            file.Url = "https://soafiles.blob.core.windows.net/files/prueba2.pdf";
            List<Reference> references = new List<Reference>();
            file.References = references.ToArray();
            List<string> offensiveContent = new List<string>();
            file.OffensiveContent = offensiveContent.ToArray();
            List<Sentiment> sentiments = new List<Sentiment>();
            file.Sentiments = sentiments.ToArray(); ;
            file.Status = false;

            repository.InsertOne(file);
        }
    }
}
