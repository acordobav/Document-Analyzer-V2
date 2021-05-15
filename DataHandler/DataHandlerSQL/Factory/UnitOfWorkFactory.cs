using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using DataHandlerSQL.Configuration;
using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;

namespace DataHandlerSQL.Factory
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            string connString = DataHandlerSQLConfig.Config.ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<DocAnalyzerContext>();
            optionsBuilder.UseNpgsql(connString);
            DocAnalyzerContext dbContext = new DocAnalyzerContext(optionsBuilder.Options);

            return new UnitOfWork(dbContext);
        }
    }
}
