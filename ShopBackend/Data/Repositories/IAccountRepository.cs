namespace ShopBackend.Data.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
    }
}
