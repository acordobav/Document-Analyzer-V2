using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataHandlerSQL.Repository
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        void Insert(TEntity entity);

        void Update(TEntity entity);

        TEntity GetById(object id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        void Delete(object id);

        void Delete(TEntity entity);

    }
}
