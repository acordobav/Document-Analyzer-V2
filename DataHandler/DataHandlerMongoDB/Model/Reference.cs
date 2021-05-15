using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataHandlerMongoDB.Model
{
    public class Reference
    {
        [BsonElement(elementName: "name")]
        public string Name { get; set; }

        [BsonElement(elementName: "qty")]
        public int Qty { get; set; }

        public Reference() { }

        public Reference(string name, int qty)
        {
            this.Name = name;
            this.Qty = qty;
        }
    }
}
