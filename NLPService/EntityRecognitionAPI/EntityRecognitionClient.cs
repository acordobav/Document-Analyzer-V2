using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.TextAnalytics;
using DataHandlerMongoDB.Model;


namespace EntityRecognitionAPI
{
    public class EntityRecognitionClient
    {   
        // Set the credentials of the client
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(EntityRecognitionConfig.Config.Credential);
        // Set the endpoint of the client
        private static readonly Uri endpoint = new Uri(EntityRecognitionConfig.Config.Endpoint);

        /**
         * Method which recognizes all the entities of a document.
         * document: document text to be analyzed.
         */
        public static List<Reference> EntityRecognition(string document)
        {
            // Creates a client of the NLP API
            var nlp_client = new TextAnalyticsClient(endpoint, credentials);
            // Creates a list of Employee objects to store the recognized entities
            List<Reference> entities = new List<Reference>();
            // Maximun length of text
            int maxSize = 5000;
            // Length of the text 
            Console.WriteLine("Text Length = {0}", document.Length);

            if (document.Length > maxSize)
            {
                string[] words = document.Split(' ');
                var parts = new Dictionary<int, string>();
                string part = string.Empty;
                int partCounter = 0;
                foreach (var word in words)
                {
                    if (part.Length + word.Length < maxSize)
                    {
                        part += string.IsNullOrEmpty(part) ? word : " " + word;
                    }
                    else
                    {
                        parts.Add(partCounter, part);
                        part = word;
                        partCounter++;
                    }
                }
                parts.Add(partCounter, part);

                foreach (var item in parts)
                {
                    // Obtain the recognized entities in a response
                    var response = nlp_client.RecognizeEntities(item.Value);
                    // Adds the obtained entities into the list
                    foreach (var entity in response.Value)
                    {
                        // Condition to identifies all the person entities
                        if (entity.Category == "Person")
                        {
                            // Name of the recognized entity
                            string name = entity.Text;
                            if (entities.Exists(person => person.Name == entity.Text))
                            {
                                var reference = entities.Find(person => person.Name == entity.Text);
                                reference.Qty += 1;
                            }
                            else
                            {
                                var reference = new Reference();
                                reference.Name = name;
                                reference.Qty = 1;
                                entities.Add(reference);
                            }
                        }
                    }
                }
                return entities;
            }
            else
            {
                // Obtain the recognized entities in a response
                var response = nlp_client.RecognizeEntities(document);
                // Adds the obtained entities into the list
                foreach (var entity in response.Value)
                {
                    // Condition to identifies all the person entities
                    if (entity.Category == "Person")
                    {
                        // Name of the recognized entity
                        string name = entity.Text;
                        if (entities.Exists(person => person.Name == entity.Text))
                        {
                            var reference = entities.Find(person => person.Name == entity.Text);
                            reference.Qty += 1;
                        }
                        else
                        {
                            var reference = new Reference();
                            reference.Name = name;
                            reference.Qty = 1;
                            entities.Add(reference);
                        }
                    }
                }
                return entities;
            }

        }
    }
}
