using System;

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
            /*
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
           */

            string title = "prueba.txt";

            File file2 = repository.FindOne(file => file.Title == title);

            Console.WriteLine(file2.Url);

        }
    }
}
