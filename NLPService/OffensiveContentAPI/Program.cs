using System;
using System.Threading;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

using DataHandlerAzureBlob;
using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;

using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OffensiveContentAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Environment.GetEnvironmentVariable("MONGODB_HOST");
            string port = Environment.GetEnvironmentVariable("MONGODB_PORT");
            string connection_string = "mongodb://" + host + ":" + port;
            DataHandlerMongoDBConfig.Config.ConnectionString = connection_string;
            DataHandlerMongoDBConfig.Config.DataBaseName = Environment.GetEnvironmentVariable("MONGODB_NAME");
            DataHandlerAzureConfig.Config.FolderPath = Environment.GetEnvironmentVariable("OFFENSIVE_FOLDER_PATH");
            Console.WriteLine("Folder Path: " + DataHandlerAzureConfig.Config.FolderPath);


            OffensiveContentConfig.Config.SuscriptionKey = Environment.GetEnvironmentVariable("OFFENSIVE_CONTENT_SUSCRIPTION_KEY");
            OffensiveContentConfig.Config.Endpoint = Environment.GetEnvironmentVariable("OFFENSIVE_CONTENT_ENDPOINT");

            RabbitMQConfig.Config.ConnectionUrl = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_URL");

            var factory = new ConnectionFactory() { Uri = new Uri(RabbitMQConfig.Config.ConnectionUrl) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "analysis", type: ExchangeType.Fanout);
                channel.ExchangeDeclare(exchange: "analysis_results", type: ExchangeType.Direct);

                channel.QueueDeclare(queue: "offensive", exclusive: false);
                channel.QueueBind(queue: "offensive",
                                  exchange: "analysis",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for requests.");

                var nlpConsumer = new EventingBasicConsumer(channel);


                nlpConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var blob_metadata = Encoding.UTF8.GetString(body);

                    Console.WriteLine(" [NLP] {0}", blob_metadata);

                    // Obtain the request
                    Request request = JsonSerializer.Deserialize<Request>(blob_metadata);
                    // Obtain the blob url
                    string blob_url = request.Url;
                    Console.WriteLine("Url: " + blob_url);
                    // Obtain the blob owner
                    string blob_owner = request.Owner;
                    // Obtain the blob title
                    string blob_title = request.Title;
                    // Create an empty list for offensive content
                    List<string> blob_offensive_content = new List<string>();

                    // Obtain the extension of the file
                    string[] words = blob_title.Split(".");
                    string blob_extension = words[1];
                    //Console.WriteLine("Extension: " + blob_extension);
                    // Get the blob_file
                    string blob_file = BlobHandler.GetBlobFile(blob_url, blob_extension);
                    // Obtain the blob file text
                    string text = FileHandler.FileHandler.GetBlobText(blob_file);
                    //Console.WriteLine(text);
                    // Obtain the text offensive content
                    blob_offensive_content = OffensiveContentClient.OffensiveContent(text);

                    Console.WriteLine("---------------------------- ");
                    // Print the offensive content
                    Console.WriteLine("Document offensive content:");
                    foreach (var offensive in blob_offensive_content)
                        Console.WriteLine(offensive);
                    Console.WriteLine("---------------------------- ");

                    Console.WriteLine("JSON Results:");
                    var offensiveJSON = JsonSerializer.Serialize(blob_offensive_content);
                    Console.WriteLine(offensiveJSON);

                    // Result Response
                    byte[] offensive_bytes = Encoding.UTF8.GetBytes(offensiveJSON);
                    channel.BasicPublish(exchange: "analysis_results",
                                 routingKey: "offensive",
                                 basicProperties: null,
                                 body: offensive_bytes);

                    // Update the document in the database
                    IMongoRepositoryFactory factory = new MongoRepositoryFactory();
                    IMongoRepository<FileMongo> repository = factory.Create<FileMongo>();
                    FileMongo update = repository.FindOne(file => file.Title == blob_title && file.Owner == blob_owner);
                    update.OffensiveContent = blob_offensive_content.ToArray();
                    update.Status = true;
                    repository.ReplaceOne(update);
                };

                channel.BasicConsume(queue: "offensive", autoAck: true, consumer: nlpConsumer);

                while (true) { Thread.Sleep(100000); }
            }
        }
    }
}
