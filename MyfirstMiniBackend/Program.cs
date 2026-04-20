using System.Text.Json;

public class Program
{
    public static void Main()
    {
        // Ξεκινάμε με authentication
        bool isAuthenticated = Authentication_Simulation();

        // Μόνο αν κάνει login μπαίνει στο σύστημα προϊόντων
        if (isAuthenticated)
        {
            StartMenu();
        }
    }

    static void StartMenu()
    {
        // ✔ ΤΩΡΑ: portable path
        string path = Path.Combine(AppContext.BaseDirectory, "Products.json");
        List<Product> productList = LoadFromJson<Product>(path);

        IProductRepository repository = new InMemoryProductRepository();

        // Φορτώνουμε δεδομένα στο repository
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

    static bool Authentication_Simulation()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Accounts.json");
        List<Account> accountList = LoadFromJson<Account>(path);
        IAccountRepository repository = new InMemoryAccountRepository();
        IAccountService service = new AccountService(repository);

        foreach (var item in accountList)
        {
            repository.Add(item);
        }

        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    if (LoginAccount(service))
                        return true; // ✔ επιστρέφει στο Main → StartMenu
                    break;

                case 2:
                    CreateAccount(path, service);
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void CreateAccount(string path, IAccountService service)
    {
        Console.WriteLine("Give a Username:");
        string? username = Console.ReadLine();

        // ✔ validation
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Invalid username.");
            return;
        }

        while (service.GetAccountByUserName(username) != null)
        {
            Console.WriteLine("Username already exists. Try again:");
            username = Console.ReadLine();
        }

        Console.WriteLine("Give a Password:");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Invalid password.");
            return;
        }

        // ✔ Hash password (σωστό)
        string hash = BCrypt.Net.BCrypt.HashPassword(password);

        service.AddAccount(username, hash);

        SaveToJson(path, service.GetAllAccounts());

        Console.WriteLine("Account created successfully!");
    }

    private static bool LoginAccount(IAccountService service)
    {
        Console.WriteLine("Give a Username:");
        string? username = Console.ReadLine();

        Console.WriteLine("Give a Password:");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Invalid input.");
            return false;
        }

        var account = service.GetAccountByUserName(username);

        // ✔ clean login check
        if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.AccountPassword))
        {
            Console.WriteLine("Wrong Username or Password");
            return false;
        }

        Console.WriteLine("Login Success!");
        return true;
    }

    // =========================
    // PRODUCT METHODS
    // =========================

    static void CreateProduct(IProductService service, string path)
    {
        Console.Write("Enter product name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        Console.Write("Enter product price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        service.AddProduct(name, price);
        SaveToJson(path, service.GetAllProducts());
    }

    static void ReadProducts(IProductService service)
    {
        var products = service.GetAllProducts();

        foreach (var p in products)
        {
            Console.WriteLine($"Id: {p.ProductId}, Name: {p.ProductName}, Price: {p.ProductPrice}€");
        }
    }

    static void UpdateProduct(IProductService service, string path)
    {
        Console.Write("Enter product id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        Console.Write("Enter new name (or Enter to skip): ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = null;

        Console.Write("Enter new price (or Enter to skip): ");
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
        SaveToJson(path, service.GetAllProducts());
    }

    static void DeleteProduct(IProductService service, string path)
    {
        Console.Write("Enter product id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        service.DeleteProduct(id);
        SaveToJson(path, service.GetAllProducts());
    }

    // =========================
    // GENERIC JSON HELPERS (✔ refactor)
    // =========================

    static List<T> LoadFromJson<T>(string path)
    {
        if (!File.Exists(path))
            return new List<T>();

        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    static void SaveToJson<T>(string path, IEnumerable<T> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(path, json);
    }
}