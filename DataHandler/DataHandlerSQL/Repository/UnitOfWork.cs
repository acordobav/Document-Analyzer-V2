using System;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace DataHandlerSQL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Method that returns the repository of an specific TEntity
        /// </summary>
        /// <typeparam name="TEntity">Database entity</typeparam>
        /// <returns>Repository of the TEntity</returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // Creation of the Hashtable if it doesnt exist
            if (_repositories == null)
                _repositories = new Hashtable();

            // Get the type of the Entity
            var type = typeof(TEntity).Name;

            // If the Repository is created, it is returned
            if (_repositories.ContainsKey(type)) return (IRepository<TEntity>)_repositories[type];

            // Creation of the repository
            var repositoryType = typeof(BaseRepository<>); // Type of repository
            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), _dbContext);

            // The new repository is added to the Hashtable
            _repositories.Add(type, repositoryInstance);

            return (IRepository<TEntity>)_repositories[type];
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
