public interface IAccountService
{
    IEnumerable<Account> GetAllAccounts();
    Account? GetAccountByUserName(string username);
    void AddAccount(string username, string password);
}