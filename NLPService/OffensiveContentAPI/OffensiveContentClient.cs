using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.ContentModerator;


namespace OffensiveContentAPI
{
    public class OffensiveContentClient
    {
        // Set the suscription key of the client
        private static readonly string SubscriptionKey = OffensiveContentConfig.Config.SuscriptionKey;
        // Set the endpoint of the client
        private static readonly string Endpoint = OffensiveContentConfig.Config.Endpoint;

        public static List<string> OffensiveContent(string document)
        {
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(SubscriptionKey));
            client.Endpoint = Endpoint;

            // Maximun length of text
            int maxSize = 1000;
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

                List<string> result = new List<string>();
                foreach (var item in parts)
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(item.Value);
                    MemoryStream stream = new MemoryStream(textBytes);

                    var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
                    if (screenResult.Terms != null)
                    {
                        foreach (var profanity in screenResult.Terms) 
                        {
                            result.Add(profanity.Term.ToString());
                        }
                    }
                }

                return result;
            }
            else
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(document);
                MemoryStream stream = new MemoryStream(textBytes);

                var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);

                List<string> result = new List<string>();

                if (screenResult.Terms != null)
                {
                    foreach (var profanity in screenResult.Terms)
                    {
                        result.Add(profanity.Term.ToString());
                    }
                }

                return result;
            }

      
        }
    }
}
