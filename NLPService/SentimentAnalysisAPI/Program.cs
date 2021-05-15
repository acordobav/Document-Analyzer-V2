using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

using DataHandlerAzureBlob;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using DataHandlerMongoDB.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;

using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SentimentAnalysisAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            SentimentAnalysisConfig.Config.Credential = Environment.GetEnvironmentVariable("SENTIMENT_ANALYSIS_CREDENTIAL");
            SentimentAnalysisConfig.Config.Endpoint = Environment.GetEnvironmentVariable("SENTIMENT_ANALYSIS_ENDPOINT");

            var factory = new ConnectionFactory() { Uri = new Uri(RabbitMQConfig.Config.ConnectionUrl) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "analysis", type: ExchangeType.Fanout);
                channel.ExchangeDeclare(exchange: "analysis_results", type: ExchangeType.Direct);

                channel.QueueDeclare(queue: "nlp", exclusive: true);

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
                    // Obtain the blob owner
                    string blob_owner = request.Owner.ToString();
                    // Obtain the blob title
                    string blob_title = request.Title;
                    // Create an empty list for offensive content
                    List<Sentiment> blob_sentiments = new List<Sentiment>();

                    // Obtain the extension of the file
                    string[] words = blob_title.Split(".");
                    string blob_extension = words[1];
                    // Get the blob_file
                    string blob_file = BlobHandler.GetBlobFile(blob_url, blob_extension);
                    // Obtain the blob file text
                    string text = FileHandler.FileHandler.GetBlobText(blob_file);
                    // Obtain the text offensive content
                    blob_sentiments = SentimentAnalysisClient.SentimentAnalysis(text);

                    Console.WriteLine("---------------------------- ");
                    // Print the analyzed sentiments
                    Console.WriteLine("Document analyzed sentiments:");
                    foreach (var sentiment in blob_sentiments)
                        Console.WriteLine(sentiment.Name + ": " + sentiment.Score);
                    Console.WriteLine("---------------------------- ");

                    // Update the document in the database
                    IMongoRepositoryFactory factory = new MongoRepositoryFactory();
                    IMongoRepository<FileMongo> repository = factory.Create<FileMongo>();
                    FileMongo update = repository.FindOne(file => file.Title == blob_title && file.Owner == int.Parse(blob_owner));
                    update.Sentiments = blob_sentiments.ToArray();
                    //update.Status = true;
                    repository.ReplaceOne(update);


                    Console.WriteLine("JSON Results:");
                    var offensiveJSON = JsonSerializer.Serialize(blob_sentiments);

                    // Result Response
                    byte[] result_bytes = Encoding.UTF8.GetBytes(blob_metadata);
                    channel.BasicPublish(exchange: "analysis_results",
                                 routingKey: "nlp",
                                 basicProperties: null,
                                 body: result_bytes);
                    byte[] offensive_bytes = Encoding.UTF8.GetBytes(offensiveJSON);
                    channel.BasicPublish(exchange: "analysis_results",
                                 routingKey: "offensive",
                                 basicProperties: null,
                                 body: offensive_bytes);
                };

                channel.BasicConsume(queue: "nlp", autoAck: true, consumer: nlpConsumer);
                Console.ReadLine();
            }
        }
    }
}
