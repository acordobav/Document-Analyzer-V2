using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using DataHandlerMongoDB.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;

namespace DocumentAnalyzerAPI.Models
{
    public class BrokerHandler
    {
        // EXCHANGES
        private const String REQUEST_EXCHANGE_NAME = "analysis";

        public static void NotifyAnalyzers(List<NotificationData> requests, string owner)
        {
            var factory = new ConnectionFactory() { Uri = new Uri("amqps://ayfpwbex:M_cMEhP-zE1VKSduyZa02bcck07zC8-d@orangutan.rmq.cloudamqp.com/ayfpwbex") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: REQUEST_EXCHANGE_NAME, type: ExchangeType.Fanout);

                foreach (NotificationData req in requests)
                {
                    // Create an empty lists
                    List<Reference> blob_references = new List<Reference>();
                    List<Sentiment> blob_sentiments = new List<Sentiment>();
                    List<string> blob_offensive_content = new List<string>();

                    // Create the mongo database file
                    FileMongo file = new FileMongo();
                    file.Title = req.Title;
                    file.Url = req.Url;
                    file.References = blob_references.ToArray();
                    file.Sentiments = blob_sentiments.ToArray();
                    file.OffensiveContent = blob_offensive_content.ToArray();
                    file.Owner = owner;
                    file.Status = false;

                    // Insert the document into the database
                    IMongoRepositoryFactory mongoRepository = new MongoRepositoryFactory();
                    IMongoRepository<FileMongo> repository = mongoRepository.Create<FileMongo>();
                    repository.InsertOne(file);


                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Request(req.Title, req.Url, owner)));

                    channel.BasicPublish(exchange: REQUEST_EXCHANGE_NAME,
                                         routingKey: "",
                                         basicProperties: null,
                                         body: body);
                }
            }
        }

        /*public static void StartListening()
        {
            var factory = new ConnectionFactory() { Uri = new Uri("amqps://ayfpwbex:M_cMEhP-zE1VKSduyZa02bcck07zC8-d@orangutan.rmq.cloudamqp.com/ayfpwbex") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: RESULT_EXCHANGE_NAME, type: ExchangeType.Direct);

                channel.QueueDeclare(queue: NLP_RESULT_QUEUE, exclusive: true);
                channel.QueueDeclare(queue: OFFENSIVE_RESULT_QUEUE, exclusive: true);
                channel.QueueDeclare(queue: SENTIMENT_RESULT_QUEUE, exclusive: true);

                channel.QueueBind(queue: NLP_RESULT_QUEUE,
                                  exchange: RESULT_EXCHANGE_NAME,
                                  routingKey: "nlp");

                channel.QueueBind(queue: OFFENSIVE_RESULT_QUEUE,
                                  exchange: RESULT_EXCHANGE_NAME,
                                  routingKey: "offensive");

                channel.QueueBind(queue: SENTIMENT_RESULT_QUEUE,
                                  exchange: RESULT_EXCHANGE_NAME,
                                  routingKey: "sentiment");

                Console.WriteLine(" [*] Waiting for requests.");

                var nlpConsumer = new EventingBasicConsumer(channel);
                var offensiveConsumer = new EventingBasicConsumer(channel);
                var sentimentConsumer = new EventingBasicConsumer(channel);

                nlpConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [NLP] {0}", message);
                };

                offensiveConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [OFFENSIVE] {0}", message);
                };

                sentimentConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [SENTIMENT] {0}", message);
                };

                while (true)
                {

                    channel.BasicConsume(queue: NLP_RESULT_QUEUE,
                                         autoAck: true,
                                         consumer: nlpConsumer);

                    channel.BasicConsume(queue: OFFENSIVE_RESULT_QUEUE,
                                         autoAck: true,
                                         consumer: offensiveConsumer);

                    channel.BasicConsume(queue: SENTIMENT_RESULT_QUEUE,
                                         autoAck: true,
                                         consumer: sentimentConsumer);
                }
            }
        }*/
    }
}
