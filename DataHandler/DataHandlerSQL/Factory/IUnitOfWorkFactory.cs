using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataHandlerSQL.Repository;

namespace DataHandlerSQL.Factory
{
    public interface IUnitOfWorkFactory
    {
        public IUnitOfWork Create();
    }
}
