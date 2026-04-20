public class InMemoryAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = [];
    public IEnumerable<Account> GetAccounts() => _accounts;
    public Account? GetByUsername(string username) => _accounts.FirstOrDefault(p => p.AccountUserName == username);
    public void Add(Account account)
    {
        _accounts.Add(account);
        Console.WriteLine($"The account {account.AccountUserName} with id {account.AccountId} has been added.");
    }

}