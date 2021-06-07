using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;

using DataHandlerAzureBlob;
using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using DataHandlerMongoDB.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;

using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace EntityRecognitionAPI
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
            DataHandlerAzureConfig.Config.FolderPath = Environment.GetEnvironmentVariable("ENTITY_FOLDER_PATH");
            Console.WriteLine("Folder Path: " + DataHandlerAzureConfig.Config.FolderPath);

            EntityRecognitionConfig.Config.Credential = Environment.GetEnvironmentVariable("ENTITY_RECOGNITION_CREDENTIAL");
            EntityRecognitionConfig.Config.Endpoint = Environment.GetEnvironmentVariable("ENTITY_RECOGNITION_ENDPOINT");

            RabbitMQConfig.Config.ConnectionUrl = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_URL");

            var factory = new ConnectionFactory() { Uri = new Uri(RabbitMQConfig.Config.ConnectionUrl) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "analysis", type: ExchangeType.Fanout);
                channel.ExchangeDeclare(exchange: "analysis_results", type: ExchangeType.Direct);

                channel.QueueDeclare(queue: "nlp");

                channel.QueueBind(queue: "nlp",
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
                    // Create an empty list for references
                    List<Reference> blob_references = new List<Reference>();
                    
                    // Obtain the extension of the file
                    string[] words = blob_title.Split(".");
                    string blob_extension = words[1];
                    Console.WriteLine("Extension: " + blob_extension);
                    // Get the blob_file
                    string blob_file = BlobHandler.GetBlobFile(blob_url, blob_extension);
                    Console.WriteLine("File downloaded: " + blob_file);
                    // Obtain the blob file text
                    string text = FileHandler.FileHandler.GetBlobText(blob_file);
                    Console.WriteLine("File text obtained");
                    Console.WriteLine(text);
                    // Obtain the text offensive content
                    blob_references = EntityRecognitionClient.EntityRecognition(text);

                    Console.WriteLine("---------------------------- ");
                    // Print the recognized entities
                    Console.WriteLine("Document recognized entities:");
                    foreach (var reference in blob_references)
                        Console.WriteLine(reference.Name + ": " + reference.Qty);
                    Console.WriteLine("---------------------------- ");


                    // Update the document in the database
                    IMongoRepositoryFactory factory = new MongoRepositoryFactory();
                    IMongoRepository<FileMongo> repository = factory.Create<FileMongo>();
                    FileMongo update = repository.FindOne(file => file.Title == blob_title && file.Owner == blob_owner);
                    update.References = blob_references.ToArray();
                    //update.Status = true;
                    repository.ReplaceOne(update);
                    //repository.InsertOne(update);

                    Console.WriteLine("JSON Results:");
                    request.References = blob_references;
                    request.Id = update.Id.ToString();
                    var resultJSON = JsonSerializer.Serialize(request);
                    Console.WriteLine(resultJSON);

                    // Result Response
                    byte[] result_bytes = Encoding.UTF8.GetBytes(resultJSON);
                    channel.BasicPublish(exchange: "analysis_results",
                                 routingKey: "nlp",
                                 basicProperties: null,
                                 body: result_bytes);
                };

                channel.BasicConsume(queue: "nlp", autoAck: true, consumer: nlpConsumer);

                while (true) { Thread.Sleep(100000); }
            }
        }
    }
}
