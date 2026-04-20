public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Account> GetAllAccounts() => _repository.GetAccounts();

    public Account? GetAccountByUserName(string username) => _repository.GetByUsername(username);

    public void AddAccount(string username, string password)
    {
        int id = _repository.GetAccounts().Any()
            ? _repository.GetAccounts().Max(p => p.AccountId) + 1
            : 1;

        var account = new Account(id, username, password);
        _repository.Add(account);
    }

}