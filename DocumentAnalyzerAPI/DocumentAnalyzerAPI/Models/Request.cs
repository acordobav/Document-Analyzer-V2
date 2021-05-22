using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class Request
    {
        public Request(String title, String url, int owner)
        {
            Title = title;
            Url = url;
            Owner = owner;
        }

        public String Title { get; set; }
        public String Url { get; set; }
        public int Owner { get; set; }
    }
}
