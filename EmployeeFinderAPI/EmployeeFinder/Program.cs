using DataHandlerMongoDB.Repository;
using DataHandlerSQL.Factory;
using FileMongo = DataHandlerMongoDB.Model.File;
using System;
using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Factory;
using DataHandlerSQL.Configuration;
using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;
using System.Collections.Generic;
using System.Linq;
using Finder.Models;

namespace EmployeeFinderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeFinderAPI.EmployeeFinder.setUpDBConnection();
            EmployeeFinderAPI.EmployeeFinder.StartListening();

            /*NotificationData data = new NotificationData();
            data.Title = "FilterTest.txt";
            data.Url = "https://soafiles.blob.core.windows.net/files/FilterTest.txt";

            List<Match> matches = EmployeeFinderAPI.EmployeeFinder.FindEmployeeReferences(data, 3);

            foreach (Match m in matches)
            {
                Console.WriteLine(m.employeeName);
            }*/
        }
    }
}
