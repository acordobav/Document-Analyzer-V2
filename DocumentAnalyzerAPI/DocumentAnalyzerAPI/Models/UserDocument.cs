using DataHandlerMongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class UserDocument
    {
        public UserDocument(string name, string url, string id, bool status, Sentiment[] sentiments, string[] offensive)
        {
            Title = name;
            Status = status;
            Url = url;
            Id = id;
            UserDocumentReferences = new List<Reference>();
            Feelings = sentiments;
            OffensiveContent = offensive;
        } 

        public string Title { get; set; }
        public bool Status { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public List<Reference> UserDocumentReferences { get; set; }
        public Sentiment[] Feelings { get; set; }
        public string[] OffensiveContent { get; set; }
    }
}
