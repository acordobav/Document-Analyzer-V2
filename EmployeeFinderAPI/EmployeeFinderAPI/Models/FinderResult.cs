using Finder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinderAPI.Models
{
    class FinderResult
    {
        public List<Match> Matches { get; set; }
        public String Title { get; set; }
        public String Url { get; set; }
        public String Id { get; set; }
    }
}
