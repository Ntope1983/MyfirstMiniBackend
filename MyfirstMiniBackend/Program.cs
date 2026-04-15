using System.Text.Json;
public class Middleware
{
    private readonly Func<Task> _next;

    public Middleware(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        Console.WriteLine("Before");      // Εκτύπωση πριν την εκτέλεση
        await _next();                    // Εκτέλεση του delegate
        Console.WriteLine("After");       // Εκτύπωση μετά την εκτέλεση
    }
}

// Παράδειγμα χρήσης
// ========================
// Program
// ========================
public class Program
{
    public static void Main()
    {


        Menu();

    }
    static void Menu()
    {
        //PcNtope string jsonData = File.ReadAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\deserializeProduct.json");

        string jsonData = File.ReadAllText(@"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json");//Json File Product records
        var productList = JsonSerializer.Deserialize<List<Product>>(jsonData);//save in a Product List
        IProductRepository MyRepository = new InMemoryProductRepository();
        foreach (Product item in productList)//Add products in InMemoryProductRepository 
        {
            MyRepository.Add(item);
        }
        IProductService productService = new ProductService(MyRepository);

        string menu = "Please select an operation:\n" +
                      "1. Create Product\n" +
                      "2. Read Products\n" +
                      "3. Update Product By id\n" +
                      "4. Delete Product By id\n" +
                      "5. Exit\n";

        int menuValue;

        while (true)
        {
            Console.WriteLine(menu);
            string choice = Console.ReadLine();

            while (!int.TryParse(choice, out menuValue))
            {
                Console.WriteLine("Please give an integer 1-5");
                choice = Console.ReadLine();
            }

            if (menuValue == 5) break;// Exit Choice

            switch (menuValue)
            {
                case 1: // Create Product

                    Console.WriteLine("Enter product name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter product price:");
                    decimal price;
                    while (!decimal.TryParse(Console.ReadLine(), out price))
                    {
                        Console.WriteLine("Please enter a valid decimal number for price.");
                    }

                    productService.AddProduct(name, price);
                    break;
                case 2: // Read Products
                    foreach (var product in productService.GetAllProducts())
                    {
                        Console.WriteLine($"Id: {product.ProductId}, Name: {product.ProductName}, Price: {product.ProductPrice} Euro");
                    }
                    break;


                    caqse 3: // Update Product with id
                    Console.WriteLine("Enter the id of the product to delete:");
                    int idToDelete;
                    while (!int.TryParse(Console.ReadLine(), out idToDelete))
                    {
                        Console.WriteLine("Please enter a valid integer.");
                    }

                    productService.DeleteProduct(idToDelete);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter 1-4.");
                    break;
            }
        }
    }
}


