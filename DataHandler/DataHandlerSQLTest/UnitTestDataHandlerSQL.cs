using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;

namespace DataHandlerSQLTest
{
    [TestClass]
    public class UnitTestDataHandlerSQL
    {
        private IUnitOfWork unitOfWork;
        IRepository<Employee> repository;

        [TestInitialize]
        public void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DocAnalyzerContext>().
                UseInMemoryDatabase(databaseName: "EmployeeDatabase");
            DocAnalyzerContext dbContext = new DocAnalyzerContext(optionsBuilder.Options);

            unitOfWork = new UnitOfWork(dbContext);

            repository = unitOfWork.GetRepository<Employee>();
        }

        [TestMethod]
        public void TestInsert()
        {
            Employee employee = new Employee();
            employee.FullName = "Test employee #sYNdsDFK74=4?";
            repository.Insert(employee);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TestGetById()
        {
            Employee employee = new Employee();
            employee.FullName = "Test employee #sYNdsDFK74=4?";
            repository.Insert(employee);
            unitOfWork.Commit();

            Employee searchedEmployee = repository.GetById(1);
        }

        [TestMethod]
        public void TestGetAll()
        {
            Employee employee1 = new Employee();
            employee1.FullName = "Test employee #sYNdsDFK74=4?";
            repository.Insert(employee1);
            unitOfWork.Commit();

            Employee employee2 = new Employee();
            employee2.FullName = "Test employee #sgrgrsdsDFK74=4?";
            repository.Insert(employee2);
            unitOfWork.Commit();

            List<Employee> employees = repository.Get().ToList();

            for (int i = 0; i < 2; i++)
            {
                Assert.IsNotNull(employees[i]);
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            string genericName = "Generic employee";
            Employee employee1 = new Employee();
            employee1.FullName = genericName;
            repository.Insert(employee1);
            unitOfWork.Commit();

            string newName = "Test employee Name Updated";
            Employee employee = repository.Get(employee => employee.FullName == genericName).FirstOrDefault();
            employee.FullName = newName;
            repository.Update(employee);
            unitOfWork.Commit();

            Employee updatedEmployee = repository.Get(employee => employee.FullName == newName).FirstOrDefault();

            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual(employee.EmployeeId, updatedEmployee.EmployeeId);
        }

        [TestMethod]
        public void TestDelete()
        {
            int id = 100000;
            Employee employee = new Employee();
            employee.FullName = "Test employee #3434sd3f5s3fd";
            employee.EmployeeId = id;

            repository.Insert(employee);
            unitOfWork.Commit();

            repository.Delete(id);
            unitOfWork.Commit();
        }
    }
}
