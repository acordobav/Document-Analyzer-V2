namespace DataHandlerSQL.Repository
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        void Commit();
    }
}
