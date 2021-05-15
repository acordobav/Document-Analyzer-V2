using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataHandlerMongoDB.Repository;

namespace DataHandlerMongoDB.Factory
{
    public interface IMongoRepositoryFactory
    {
        public IMongoRepository<TDocument> Create<TDocument>() where TDocument : IDocument;
    }
}
