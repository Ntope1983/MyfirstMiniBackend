using System.Text.Json;
// Program
// ========================
public class Program
{
    public static void Main()
    {
        Authentication_Simulation();
        StartMenu();
    }

    static void StartMenu()
    {
        //string path = @"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json";
        string path = @"C:\Users\g_pol\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json";
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
        string path = @"C:\Users\g_pol\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json";
        Console.WriteLine("\nSelect an option:");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");

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


