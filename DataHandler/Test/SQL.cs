using System;
using DataHandlerSQL.Configuration;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;
using System.Collections.Generic;
using System.Linq;



using Microsoft.EntityFrameworkCore;

namespace Test
{
    class SQL
    {
        /*
        static void Main(string[] args)
        {
            string connString = "Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;";
            DataHandlerSQLConfig.Config.ConnectionString = connString;

            IUnitOfWorkFactory factory = new UnitOfWorkFactory();
            IUnitOfWork unitOfWork = factory.Create();

            IRepository<UserCredential> rpUsercredential = unitOfWork.GetRepository<UserCredential>();

            UserCredential usercredential = new UserCredential();
            usercredential.FullName = "IUnitOfWorkFactory Test";
            usercredential.UserPassword = "test";
            usercredential.Email = "IUnitOfWorkFactory@gmail.com";

            rpUsercredential.Insert(usercredential);

            unitOfWork.Commit();
        }
        */
    }
}
