using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class Match
    {
        public Match(string empName, string docId, int quantity, int id)
        {
            employeeName = empName;
            qty = quantity;
            employeeId = id;
            documentId = docId;
        }

        public string employeeName { get; set; }
        public int qty { get; set; }
        public int employeeId { get; set; }
        public string documentId { get; set; }
    }
}
