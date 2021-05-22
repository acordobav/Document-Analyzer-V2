using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using FileMongo = DataHandlerMongoDB.Model.File;
using DataHandlerSQL.Model;
using DocumentAnalyzerAPI.Models;
using DataHandlerSQL.Repository;

namespace DocumentASnalyzerAPI.Models
{
    public class EmployeeFinder
    {
        public static List<Match> FindEmployeeReferences(NotificationData req, int owner,
                                                         IMongoRepository<FileMongo> repository,
                                                         IUnitOfWork unit_of_work)
        {
            FileMongo userFile = repository.FilterBy(file => file.Title == req.Title && file.Owner == owner).ToList().First();

            while(!userFile.Status)
            {
                userFile = repository.FilterBy(file => file.Title == req.Title && file.Owner == owner).ToList().First();
            }

            IRepository<Employee> employeeRepo = unit_of_work.GetRepository<Employee>();
            List<Employee> employeeList = employeeRepo.Get().ToList();
            List<Match> employeesIdentified = new List<Match>();
 
            foreach (Reference fileRef in userFile.References)
            {
                (bool, int, string) isReferenced = IsEmployeeReferenced(employeeList, fileRef.Name);
                if (isReferenced.Item1)
                {
                    employeesIdentified.Add(new Match(isReferenced.Item3, userFile.Id.ToString(), fileRef.Qty, isReferenced.Item2));
                }
            }

            AddUserReferences(employeesIdentified, unit_of_work);
            return employeesIdentified;
        }

        private static (bool, int, string) IsEmployeeReferenced(List<Employee> employeeLst, string employeeName)
        {
            string[] splitSearchName = employeeName.ToLower().Split(' ');

            if (splitSearchName.Length > 1) // if it is only a name identification is not possible
            {
                foreach (Employee emp in employeeLst)
                {
                    string[] currentDBEmployeeName = emp.FullName.ToLower().Split(' ');

                    int matchCount = 0;

                    if (emp.FullName.Equals(employeeName)) // first verify if it's a total match
                    {
                        return (true, emp.EmployeeId, emp.FullName);
                    }
                    else if (currentDBEmployeeName.First().Equals(splitSearchName.First()))
                    {
                        int similarCount = CountEmployeeOccurrences(currentDBEmployeeName, employeeLst);

                        foreach (string s in splitSearchName)
                        {
                            if (currentDBEmployeeName.Contains(s)) matchCount++;
                        }

                        int diff = currentDBEmployeeName.Length - matchCount;

                        if (diff == 1 && similarCount == 1) // if there is one lastname missing but there is only one employee with that name, it counts as a match
                        {
                            return (true, emp.EmployeeId, emp.FullName);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return (false, -1, String.Empty);
        }

        private static int CountEmployeeOccurrences(string[] employeeSplit, List<Employee> employeeLst)
        {
            int count = 0;
            foreach (Employee employee in employeeLst)
            {
                string[] currentEmployee = employee.FullName.ToLower().Split(' ');

                if (currentEmployee.First().Equals(employeeSplit.First()) && currentEmployee[1].Equals(employeeSplit[1]))
                {
                    // compare first name and first last name.
                    count++;
                }
            }
            return count;
        }

        public static List<UserDocument> FindEmployeeDocuments(int employeeId, IMongoRepository<FileMongo> repository)
        {
            List<FileMongo> userFiles = repository.FilterBy(file => file.Owner == employeeId).ToList();
            List<UserDocument> result = new List<UserDocument>();

            foreach (FileMongo file in userFiles)
            {
                result.Add(new UserDocument(file.Title, file.Url, file.Id.ToString(), file.Status));
            }
            return result;
        }

        public static void AddUserReferences(List<Match> processingResult,
                                             IUnitOfWork unit_of_work)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unit_of_work.GetRepository<EmployeeReferenceByDocument>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();

            foreach (Match match in processingResult)
            {
                EmployeeReferenceByDocument newMatch = new EmployeeReferenceByDocument();
                newMatch.DocumentId = match.documentId;
                newMatch.EmployeeId = match.employeeId;
                newMatch.Ocurrences = match.qty;

                if (!ReferenceExists(userRefs, match.employeeId, match.documentId))
                {
                    referencesRepo.Insert(newMatch);
                } else
                {
                    //referencesRepo.Update(newMatch);
                }
                unit_of_work.Commit();
            }
        }

        private static bool ReferenceExists(List<EmployeeReferenceByDocument> currentRefs, int empId, string docId)
        {
            foreach (EmployeeReferenceByDocument eRef in currentRefs)
            {
                if (eRef.EmployeeId == empId && eRef.DocumentId.Equals(docId)) return true;
            }
            return false;
        }

        public static List<UserDocument> GetDocumentReferences(List<UserDocument> userDocs, IUnitOfWork unit_of_work)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unit_of_work.GetRepository<EmployeeReferenceByDocument>();
            IRepository<Employee> empRepo = unit_of_work.GetRepository<Employee>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();
            List<Reference> docReferences = new List<Reference>();


            foreach (EmployeeReferenceByDocument docRef in userRefs)
            {
                foreach (UserDocument uDoc in userDocs)
                {
                    if (docRef.DocumentId.Equals(uDoc.Id))
                    {
                        Employee emp = empRepo.GetById(docRef.EmployeeId);
                        Reference newRef = new Reference();
                        newRef.Name = emp.FullName;
                        newRef.Qty = (int)docRef.Ocurrences;
                        uDoc.UserDocumentReferences.Add(newRef);
                    }
                }
            }

            return userDocs;
        }

        public static List<EmployeeCount> GetEmployeeGlobalCounter(IUnitOfWork unit_of_work, IMongoRepository<FileMongo> mongo_repo)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unit_of_work.GetRepository<EmployeeReferenceByDocument>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();
            IRepository<Employee> empRepo = unit_of_work.GetRepository<Employee>();
            List<Employee> employeeList = empRepo.Get().ToList();

            List<EmployeeCount> result = new List<EmployeeCount>();

            foreach (Employee emp in employeeList) 
            {
                int? currentCount = 0;
                List<Reference> userDocs = new List<Reference>();
                foreach(EmployeeReferenceByDocument docRef in userRefs)
                {
                    if (docRef.EmployeeId == emp.EmployeeId)
                    {
                        currentCount += docRef.Ocurrences;

                        FileMongo userFile = mongo_repo.FindById(docRef.DocumentId);
                        Reference newRef = new Reference();
                        newRef.Name = userFile.Title;
                        newRef.Qty = (int)docRef.Ocurrences;
                        userDocs.Add(newRef);
                    }
                }
                result.Add(new EmployeeCount(emp.FullName, currentCount, userDocs));
            }
            return result;
        }
    }
}
