using DataHandlerMongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinderAPI.Models
{
    class Request
    {
        public String Title { get; set; }
        public String Url { get; set; }
        public string Owner { get; set; }
        public bool Status { get; set; }
        public Reference[] References { get; set; } 
        public String Id { get; set; }
    }
}

