namespace ShopBackend.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<TEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken);
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
    }
}
