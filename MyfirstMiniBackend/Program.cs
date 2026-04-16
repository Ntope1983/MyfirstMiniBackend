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
        //HomePc path
        string path = @"C:\Users\g_pol\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json";
        //PcWork Path
        // string path = @"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json";
        List<Product> productList = DeSerializeListProductsFromJson(path);
        IProductRepository MyRepository = new InMemoryProductRepository();
        foreach (Product item in productList)//Add products in InMemoryProductRepository 
        {
            MyRepository.Add(item);
        }
        IProductService productService = new ProductService(MyRepository);

        string startMenu = "Please select an operation:\n" +
                      "1. Create Product\n" +
                      "2. Read Products\n" +
                      "3. Update Product By id\n" +
                      "4. Delete Product By id\n" +
                      "5. Exit\n";

        int menuValue;

        while (true)
        {
            Console.WriteLine(startMenu);
            string choiceStartMenu = Console.ReadLine();

            while (!int.TryParse(choiceStartMenu, out menuValue))
            {
                Console.WriteLine("Please give an integer 1-5");
                choiceStartMenu = Console.ReadLine();
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
                    SerializeListProductsAndSaveToJson(path, productService.GetAllProducts());
                    break;
                case 2: // Read Products
                    foreach (var product in productService.GetAllProducts())
                    {
                        Console.WriteLine($"Id: {product.ProductId}, Name: {product.ProductName}, Price: {product.ProductPrice} Euro");
                    }
                    break;


                case 3: // Update Product with id
                    Console.WriteLine("Enter the id of the product to Update:");
                    int idToUpdate;
                    while (!int.TryParse(Console.ReadLine(), out idToUpdate))
                    {
                        Console.WriteLine("Please enter a valid integer.");
                    }
                    Product productToUpdate = productService.GetProductById(idToUpdate);
                    if (productToUpdate is null)
                    {
                        Console.WriteLine("The Product id you entered doesnt exist");
                    }
                    else
                    {
                        string menuUpdate = "Please select a field to update:\n" +
                      "1. Update Name\n" +
                      "2. Update Price\n" +
                      "3. Both\n" +
                      "4. Return To Start Menu\n";
                    }

                    break;

                case 4: // Delete Product with id
                    Console.WriteLine("Enter the id of the product to delete:");
                    int idToDelete;
                    while (!int.TryParse(Console.ReadLine(), out idToDelete))
                    {
                        Console.WriteLine("Please enter a valid integer.");
                    }

                    productService.DeleteProduct(idToDelete);
                    SerializeListProductsAndSaveToJson(path, productService.GetAllProducts());
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter 1-4.");
                    break;
            }
        }
    }

    public static List<Product> DeSerializeListProductsFromJson(string path)
    {

        string jsonData = File.ReadAllText(path);
        List<Product> productList = JsonSerializer.Deserialize<List<Product>>(jsonData);//save in a Product List
        return productList;

    }
    public static void SerializeListProductsAndSaveToJson(string path, IEnumerable<Product> products)
    {

        var options = new JsonSerializerOptions
        {
            WriteIndented = true//Save in different line be more readable
        };

        string json = JsonSerializer.Serialize(products, options);

        File.WriteAllText(path, json);
    }

}


