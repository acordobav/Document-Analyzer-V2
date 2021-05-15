using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore; 

namespace DataHandlerSQL.Repository
{
    class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext context;
        private DbSet<TEntity> dbSet;

        public BaseRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Method to get all the rows of an specific Entity, they can be filtered and sorted
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null) query = query.Where(filter);

            if (orderBy != null) return orderBy(query).ToList();
            else return query.ToList();
        }

        public virtual void Delete(object id)
        {
            TEntity entity = dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Attach(entity);    
            dbSet.Remove(entity);
        }
    }
}
