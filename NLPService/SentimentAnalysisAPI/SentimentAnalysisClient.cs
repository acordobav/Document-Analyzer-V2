using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.TextAnalytics;
using DataHandlerMongoDB.Model;

namespace SentimentAnalysisAPI
{
    public class SentimentAnalysisClient
    {
        // Set the credentials of the client
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(SentimentAnalysisConfig.Config.Credential);
        // Set the endpoint of the client
        private static readonly Uri endpoint = new Uri(SentimentAnalysisConfig.Config.Endpoint);

        public static List<Sentiment> SentimentAnalysis(string document)
        {
            // Creates a client of the NLP API
            var nlp_client = new TextAnalyticsClient(endpoint, credentials);

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

                double positives = 0;
                double negatives = 0;
                double neutrals = 0;

                foreach (var item in parts)
                {
                    DocumentSentiment documentSentiment = nlp_client.AnalyzeSentiment(item.Value);

                    //Console.WriteLine("Positive score: {0}", documentSentiment.ConfidenceScores.Positive);
                    //Console.WriteLine("Negative score: {0}", documentSentiment.ConfidenceScores.Negative);
                    //Console.WriteLine("Neutral score: {0}", documentSentiment.ConfidenceScores.Neutral);

                    positives += documentSentiment.ConfidenceScores.Positive;
                    negatives += documentSentiment.ConfidenceScores.Negative;
                    neutrals += documentSentiment.ConfidenceScores.Neutral;
                }

                positives = Math.Truncate((positives / parts.Count) * 10000) / 100;
                negatives = Math.Truncate((negatives / parts.Count) * 10000) / 100;
                neutrals = Math.Truncate((neutrals / parts.Count) * 10000) / 100;

               
                Sentiment positive = new Sentiment("Positive", positives);
                Sentiment negative = new Sentiment("Negative", negatives);
                Sentiment neutral = new Sentiment("Neutral", neutrals);

                List<Sentiment> sentiments = new List<Sentiment>();
                sentiments.Add(positive);
                sentiments.Add(negative);
                sentiments.Add(neutral);

                return sentiments;
            }
            else
            {
                DocumentSentiment documentSentiment = nlp_client.AnalyzeSentiment(document);

                Sentiment positive = new Sentiment("Positive", documentSentiment.ConfidenceScores.Positive);
                Sentiment negative = new Sentiment("Negative", documentSentiment.ConfidenceScores.Negative);
                Sentiment neutral = new Sentiment("Neutral", documentSentiment.ConfidenceScores.Neutral);

                List<Sentiment> sentiments = new List<Sentiment>();
                sentiments.Add(positive);
                sentiments.Add(negative);
                sentiments.Add(neutral);

                return sentiments;
            }
        }
    }
}
