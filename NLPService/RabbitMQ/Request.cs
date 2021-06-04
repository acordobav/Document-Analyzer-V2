using System;
using System.Collections.Generic;
using DataHandlerMongoDB.Model;

namespace RabbitMQ
{
    public class Request
    {
        public Request(string title, string url, string id, string owner, List<Reference> references)
        {
            Title = title;
            Url = url;
            Owner = owner;
            References = references;
            Id = id;
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Owner { get; set; }
        public List<Reference> References { get; set; }
        public string Id { get; set; }

    }
}

