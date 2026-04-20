using System.Text.Json.Serialization;

public class Account
{
    [JsonPropertyName("accountId")]
    public int AccountId { get; set; }

    [JsonPropertyName("accountUserName")]
    public string AccountUserName { get; set; }

    [JsonPropertyName("accountPassword")]
    public string AccountPassword { get; set; }
    public Account() { }
    public Account(int id, string username, string password)
    {
        AccountId = id;
        AccountUserName = username;
        AccountPassword = password;
    }
}