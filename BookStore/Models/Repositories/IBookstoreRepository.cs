namespace BookStore.Models.Repositories
{
    public interface IBookstoreRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<TEntity> Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);

    }
}
