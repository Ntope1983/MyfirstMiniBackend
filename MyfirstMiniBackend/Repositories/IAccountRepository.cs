public interface IAccountRepository
{
    IEnumerable<Account> GetAccounts();
    Account? GetByUsername(string username);
    void Add(Account account);
}