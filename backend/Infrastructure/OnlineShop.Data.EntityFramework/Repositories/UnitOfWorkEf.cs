using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Repositories
{
    public class UnitOfWorkEf : IUnitOfWork, IAsyncDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public ICartRepository CartRepository { get; }
        public IConfirmationCodeRepository ConfirmationCodeRepository { get; }

        private readonly AppDbContext _dbContext;

        public UnitOfWorkEf(
            AppDbContext dbContext,
            IAccountRepository accountRepository,
            ICartRepository cartRepository,
            IConfirmationCodeRepository confirmationCodeRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            AccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            CartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            ConfirmationCodeRepository = confirmationCodeRepository ?? throw new ArgumentNullException(nameof(confirmationCodeRepository));
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => _dbContext.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
    }
}
