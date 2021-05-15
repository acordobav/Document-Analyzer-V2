using System;

namespace RabbitMQ
{
    public class Request
    {
        public Request(string title, string url, int owner)
        {
            Title = title;
            Url = url;
            Owner = owner;
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public int Owner { get; set; }
    }
}

