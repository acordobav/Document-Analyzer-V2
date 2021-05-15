using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;

namespace Test
{
    class MongoDB
    {
        
        static void Main(string[] args)
        {
            /*
            DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzerEntities";

            IMongoRepositoryFactory repositoryFactory = new MongoRepositoryFactory();

            IMongoRepository<File> repository = repositoryFactory.Create<File>();

            File file = new File();
            file.Title = "Introduction to MongoDb with .NET";
            file.Owner = 4;
            file.Url = "http://www.google.co.cr";

            string[] offensiveContent = { "word1", "word2", "word3", "word4" };
            file.OffensiveContent = offensiveContent;


            Sentiment[] sentiments = {new Sentiment("Positive", 21),
                                      new Sentiment("Negative", 74.5),
                                      new Sentiment("Neutral", 4.5)};
            file.Sentiments = sentiments;
            
            repository.InsertOne(file);
            //*/

            //string title = "Introduction to MongoDb with .NET";

            //File file = repository.FindOne(file => file.Title == title);

            //Console.WriteLine(file.Id);

            //file.Owner = 10;

            //repository.ReplaceOne(file);

            /*List<File> files = repository.GetAll().ToList();

            foreach (File file in files)
            {
                Console.WriteLine(file.Status);
            }*/
        }
    }
}
