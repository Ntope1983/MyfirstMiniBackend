using System.Text.Json;
// Program
// ========================
public class Program
{
    public static void Main()
    {
        Authentication_Simulation();
        //StartMenu();
    }

    static void StartMenu()
    {
        string path = @"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\Products.json";
        //string path = @"C:\Users\g_pol\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\Products.json";
        List<Product> productList = LoadProducts(path);

        IProductRepository repository = new InMemoryProductRepository();

        foreach (var item in productList)
        {
            repository.Add(item);
        }

        IProductService productService = new ProductService(repository);

        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Create Product");
            Console.WriteLine("2. Read Products");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            if (choice == 5) break;

            switch (choice)
            {
                case 1:
                    CreateProduct(productService, path);
                    break;

                case 2:
                    ReadProducts(productService);
                    break;

                case 3:
                    UpdateProduct(productService, path);
                    break;

                case 4:
                    DeleteProduct(productService, path);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
    static void Authentication_Simulation()
    {
        string path = @"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\Accounts.json";
        //string path = @"C:\Users\g_pol\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\Accounts.json";
        Console.WriteLine("\nSelect an option:");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        List<Account> accountList = LoadAccounts(path);

        IAccountRepository repository = new InMemoryAccountRepository();
        IAccountService service = new AccountService(repository);

        foreach (var item in accountList)
        {
            repository.Add(item);
        }
        int choice;
        while (true)
        {
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input.Pls Give again");
            }
            if (choice == 3) Environment.Exit(0);

            switch (choice)
            {
                case 1:
                    LoginAccount(path, service);
                    break;

                case 2:
                    CreateAccount(path, service);
                    break;

                default:
                    Console.WriteLine("Invalid choice.Pls Give again");
                    break;
            }
        }

    }

    private static void CreateAccount(string path, IAccountService service)
    {
        string? username;
        string? password;
        Console.WriteLine("2. Give a Username");
        username = Console.ReadLine();
        while (service.GetAccountByUserName(username) is not null)
        {
            Console.WriteLine("Username already exist.Pls give another Username");
            username = Console.ReadLine();
        }
        Console.WriteLine("2. Give a Password");
        password = Console.ReadLine();

        // δημιουργία hash
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        service.AddAccount(username, hash);
        SaveAccounts(path, service.GetAllAccounts());
    }

    private static void SaveAccounts(string path, IEnumerable<Account> accounts)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }

    private static void LoginAccount(string path, IAccountService service)
    {
        string? username;
        string? Inputpassword;
        Console.WriteLine("2. Give a Username");
        username = Console.ReadLine();
        Console.WriteLine("2. Give a Password");
        Inputpassword = Console.ReadLine();
        // έλεγχος
        Account accountInput = service.GetAccountByUserName(username);
        while (accountInput == null)
        {
            Console.WriteLine("2. Give a a Right Username");
            username = Console.ReadLine();
            Console.WriteLine("2. Give a Password");
            Inputpassword = Console.ReadLine();
            accountInput = service.GetAccountByUserName(username);
        }

        if (BCrypt.Net.BCrypt.Verify(Inputpassword, accountInput.AccountPassword))
        {
            Console.WriteLine("Login Success");
        }
        else
        {
            Console.WriteLine("Wrong Username or Password");
        }

    }

    // =========================
    // CREATE
    // =========================

    static void CreateProduct(IProductService service, string path)
    {
        Console.Write("Enter product name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter product price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        service.AddProduct(name, price);
        SaveProducts(path, service.GetAllProducts());
    }

    // =========================
    // READ
    // =========================
    static void ReadProducts(IProductService service)
    {
        var products = service.GetAllProducts();

        foreach (var p in products)
        {
            Console.WriteLine($"Id: {p.ProductId}, Name: {p.ProductName}, Price: {p.ProductPrice}€");
        }
    }

    // =========================
    // UPDATE (PATCH STYLE)
    // =========================
    static void UpdateProduct(IProductService service, string path)
    {
        Console.Write("Enter product id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        Console.Write("Enter new name (or press Enter to skip): ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
            name = null;

        Console.Write("Enter new price (or press Enter to skip): ");
        string priceInput = Console.ReadLine();

        decimal? price = null;

        if (!string.IsNullOrWhiteSpace(priceInput))
        {
            if (decimal.TryParse(priceInput, out decimal parsed))
                price = parsed;
            else
            {
                Console.WriteLine("Invalid price.");
                return;
            }
        }

        service.UpdateProduct(id, name, price);
        SaveProducts(path, service.GetAllProducts());
    }

    // =========================
    // DELETE
    // =========================
    static void DeleteProduct(IProductService service, string path)
    {
        Console.Write("Enter product id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        service.DeleteProduct(id);
        SaveProducts(path, service.GetAllProducts());
    }

    // =========================
    // JSON HELPERS
    // =========================
    static List<Product> LoadProducts(string path)
    {
        if (!File.Exists(path))
            return new List<Product>();

        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }
    static List<Account> LoadAccounts(string path)
    {
        if (!File.Exists(path))
            return new List<Account>();

        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
    }

    static void SaveProducts(string path, IEnumerable<Product> products)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(products, options);
        File.WriteAllText(path, json);
    }
}


