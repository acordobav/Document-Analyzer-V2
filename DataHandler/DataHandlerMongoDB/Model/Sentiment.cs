using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataHandlerMongoDB.Model
{
    public class Sentiment
    {
        [BsonElement(elementName: "name")]
        public string Name { get; set; }

        [BsonElement(elementName: "score")]
        public double Score { get; set; }

        public Sentiment() { }

        public Sentiment(string name, double score)
        {
            this.Name = name;
            this.Score = score;
        }
    }
}
