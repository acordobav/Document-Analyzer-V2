using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace DataHandlerMongoDB.Repository
{
    public abstract class Document : IDocument
    {
        public DateTime CreatedAt 
        {
            get
            {
                return Id.CreationTime;
            }
        }
    }
}
