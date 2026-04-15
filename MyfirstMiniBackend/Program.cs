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
        InMemoryProductRepository MyRepository = new InMemoryProductRepository();
        //PcNtope string jsonData = File.ReadAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\deserializeProduct.json");
        string jsonData = File.ReadAllText(@"C:\Users\NDF-MO\source\repos\Ntope1983\MyfirstMiniBackend\MyfirstMiniBackend\deserializeProduct.json");
        var productList = JsonSerializer.Deserialize<List<Product>>(jsonData);
        foreach (Product item in productList)
        {
            Console.WriteLine(item.ProductName);
        }
    }

}


