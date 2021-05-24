using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class Request
    {
        public Request(String title, String url, string owner)
        {
            Title = title;
            Url = url;
            Owner = owner;
        }

        public String Title { get; set; }
        public String Url { get; set; }
        public string Owner { get; set; }
    }
}
