using System;
using System.Collections.Generic;
using System.Linq;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using FileMongo = DataHandlerMongoDB.Model.File;
using DataHandlerSQL.Model;
using DataHandlerSQL.Repository;
using Finder.Models;
using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Factory;
using DataHandlerSQL.Configuration;
using DataHandlerSQL.Factory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using EmployeeFinderAPI.Models;
using System.Text.Json;

namespace EmployeeFinderAPI
{
    public class EmployeeFinder
    {
        private static IMongoRepository<FileMongo> mongoRepository;
        private static IUnitOfWork unitOfWork;

        private const String RESULTS_EXCHANGE = "analysis_results";
        private const String FINDER_EXCHANGE = "finder_results";
        private const String FINDER_ROUTING_KEY = "finder";
        private const String NLP_RESULT_QUEUE = "nlp_results_queue";
        private const String NLP_ROUTING_KEY = "nlp";

        public static void StartListening()
        {
            var factory = new ConnectionFactory() { Uri = new Uri("amqps://ayfpwbex:M_cMEhP-zE1VKSduyZa02bcck07zC8-d@orangutan.rmq.cloudamqp.com/ayfpwbex") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: RESULTS_EXCHANGE, type: ExchangeType.Direct);

                channel.QueueDeclare(queue: NLP_RESULT_QUEUE, exclusive: true);

                channel.QueueBind(queue: NLP_RESULT_QUEUE,
                                  exchange: RESULTS_EXCHANGE,
                                  routingKey: NLP_ROUTING_KEY);

                Console.WriteLine(" [*] Waiting for results.");

                var nlpConsumer = new EventingBasicConsumer(channel);

                nlpConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [NLP] {0}", message);

                    Request req = JsonSerializer.Deserialize<Request>(message);

                    List<Match> matches = FindEmployeeReferences(req.Title, req.Owner, req.References, req.Id);
                    AddUserReferences(matches);
                    FinderResult finalResult = new FinderResult();
                    finalResult.Matches = matches;
                    finalResult.Title = req.Title;
                    finalResult.Url = req.Url;

                    String matchesJSON = JsonSerializer.Serialize(finalResult);

                    channel.BasicPublish(exchange: FINDER_EXCHANGE,
                                         routingKey: FINDER_ROUTING_KEY,
                                         basicProperties: null,
                                         body: Encoding.UTF8.GetBytes(matchesJSON));

                };

                channel.BasicConsume(queue: NLP_RESULT_QUEUE,
                                     autoAck: true,
                                     consumer: nlpConsumer);

                Console.ReadLine();
            }
        }

        public static void setUpDBConnection()
        {
            DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzerEntities";
            IMongoRepositoryFactory repositoryFactory = new MongoRepositoryFactory();
            mongoRepository = repositoryFactory.Create<FileMongo>();

            string postgreConnString = "Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = root;";
            DataHandlerSQLConfig.Config.ConnectionString = postgreConnString;
            IUnitOfWorkFactory uowFactory = new UnitOfWorkFactory();
            unitOfWork = uowFactory.Create();
        }

        public static List<Match> FindEmployeeReferences(String title, int owner, Reference[] refs, String docId)
        {
            /*FileMongo userFile = mongoRepository.FilterBy(file => file.Title == title && file.Owner == owner).ToList().First();

            while (!userFile.Status)
            {
                userFile = mongoRepository.FilterBy(file => file.Title == title && file.Owner == owner).ToList().First();
            }*/

            IRepository<Employee> employeeRepo = unitOfWork.GetRepository<Employee>();
            List<Employee> employeeList = employeeRepo.Get().ToList();
            List<Match> employeesIdentified = new List<Match>();

            foreach (Reference fileRef in refs)
            {
                (bool, int, string) isReferenced = IsEmployeeReferenced(employeeList, fileRef.Name);
                if (isReferenced.Item1)
                {
                    employeesIdentified.Add(new Match(isReferenced.Item3, docId, fileRef.Qty, isReferenced.Item2));
                }
            }
            return employeesIdentified;
        }

        private static (bool, int, string) IsEmployeeReferenced(List<Employee> employeeLst, string employeeName)
        {
            string[] splitSearchName = employeeName.ToLower().Split(' ');

            if (splitSearchName.Length > 1) // if it is only a name identification is not possible
            {
                foreach (Employee emp in employeeLst)
                {
                    string[] currentDBEmployeeName = emp.FullName.ToLower().Split(' ');

                    int matchCount = 0;

                    if (emp.FullName.Equals(employeeName)) // first verify if it's a total match
                    {
                        return (true, emp.EmployeeId, emp.FullName);
                    }
                    else if (currentDBEmployeeName.First().Equals(splitSearchName.First()))
                    {
                        int similarCount = CountEmployeeOccurrences(currentDBEmployeeName, employeeLst);

                        foreach (string s in splitSearchName)
                        {
                            if (currentDBEmployeeName.Contains(s)) matchCount++;
                        }

                        int diff = currentDBEmployeeName.Length - matchCount;

                        if (diff == 1 && similarCount == 1) // if there is one lastname missing but there is only one employee with that name, it counts as a match
                        {
                            return (true, emp.EmployeeId, emp.FullName);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return (false, -1, String.Empty);
        }

        private static int CountEmployeeOccurrences(string[] employeeSplit, List<Employee> employeeLst)
        {
            int count = 0;
            foreach (Employee employee in employeeLst)
            {
                string[] currentEmployee = employee.FullName.ToLower().Split(' ');

                if (currentEmployee.First().Equals(employeeSplit.First()) && currentEmployee[1].Equals(employeeSplit[1]))
                {
                    // compare first name and first last name.
                    count++;
                }
            }
            return count;
        }

        public static List<UserDocument> FindEmployeeDocuments(int employeeId)
        {
            List<FileMongo> userFiles = mongoRepository.FilterBy(file => file.Owner == employeeId).ToList();
            List<UserDocument> result = new List<UserDocument>();

            foreach (FileMongo file in userFiles)
            {
                result.Add(new UserDocument(file.Title, file.Url, file.Id.ToString(), file.Status));
            }
            return result;
        }

        public static void AddUserReferences(List<Match> processingResult)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unitOfWork.GetRepository<EmployeeReferenceByDocument>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();

            foreach (Match match in processingResult)
            {
                EmployeeReferenceByDocument newMatch = new EmployeeReferenceByDocument();
                newMatch.DocumentId = match.documentId;
                newMatch.EmployeeId = match.employeeId;
                newMatch.Ocurrences = match.qty;


                if (!ReferenceExists(userRefs, match.employeeId, match.documentId))
                {
                    referencesRepo.Insert(newMatch);
                }
                
                unitOfWork.Commit();
            }
        }

        private static bool ReferenceExists(List<EmployeeReferenceByDocument> currentRefs, int empId, string docId)
        {
            foreach (EmployeeReferenceByDocument eRef in currentRefs)
            {
                if (eRef.EmployeeId == empId && eRef.DocumentId.Equals(docId)) return true;
            }
            return false;
        }

        public static List<UserDocument> GetDocumentReferences(List<UserDocument> userDocs)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unitOfWork.GetRepository<EmployeeReferenceByDocument>();
            IRepository<Employee> empRepo = unitOfWork.GetRepository<Employee>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();
            List<Reference> docReferences = new List<Reference>();


            foreach (EmployeeReferenceByDocument docRef in userRefs)
            {
                foreach (UserDocument uDoc in userDocs)
                {
                    if (docRef.DocumentId.Equals(uDoc.DocId))
                    {
                        Employee emp = empRepo.GetById(docRef.EmployeeId);
                        Reference newRef = new Reference();
                        newRef.Name = emp.FullName;
                        newRef.Qty = (int)docRef.Ocurrences;
                        uDoc.UserDocumentReferences.Add(newRef);
                    }
                }
            }

            return userDocs;
        }

        public static List<EmployeeCount> GetEmployeeGlobalCounter()
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unitOfWork.GetRepository<EmployeeReferenceByDocument>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();
            IRepository<Employee> empRepo = unitOfWork.GetRepository<Employee>();
            List<Employee> employeeList = empRepo.Get().ToList();

            List<EmployeeCount> result = new List<EmployeeCount>();

            foreach (Employee emp in employeeList)
            {
                int? currentCount = 0;
                List<Reference> userDocs = new List<Reference>();
                foreach (EmployeeReferenceByDocument docRef in userRefs)
                {
                    if (docRef.EmployeeId == emp.EmployeeId)
                    {
                        currentCount += docRef.Ocurrences;

                        FileMongo userFile = mongoRepository.FindById(docRef.DocumentId);
                        Reference newRef = new Reference();
                        newRef.Name = userFile.Title;
                        newRef.Qty = (int)docRef.Ocurrences;
                        userDocs.Add(newRef);
                    }
                }
                result.Add(new EmployeeCount(emp.FullName, currentCount, userDocs));
            }
            return result;
        }

    }
}

