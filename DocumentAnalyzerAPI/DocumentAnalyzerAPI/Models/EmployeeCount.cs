using DataHandlerMongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class EmployeeCount
    {
        public EmployeeCount(string name, int? ctr, List<Reference> docs)
        {
            EmployeeName = name;
            Count = ctr;
            Documents = docs;
        }

        public string EmployeeName { get; set; }
        public int? Count { get; set; }
        public List<Reference> Documents { get; set; }
    }
}
