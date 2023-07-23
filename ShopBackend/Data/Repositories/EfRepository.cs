using Microsoft.EntityFrameworkCore;

namespace ShopBackend.Data.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public virtual Task<TEntity> GetById(Guid id, CancellationToken cancellationToken)
      => Entities.FirstAsync(it => it.Id == id, cancellationToken: cancellationToken);

        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync(cancellationToken: cancellationToken);

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Entities.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

		public virtual async Task Delete(TEntity entity, CancellationToken cancellationToken)
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));

			Entities.Remove(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

	}
}
