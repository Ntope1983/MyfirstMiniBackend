public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public IEnumerable<Product> GetProducts() => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.ProductId == id);

    public void Add(Product product)
    {
        _products.Add(product);
        Console.WriteLine($"The product {product.ProductName} with id {product.ProductId} has been added.");
    }

    public void DeleteById(int id)
    {
        var product = GetById(id);
        if (product != null)
        {
            _products.Remove(product);
            Console.WriteLine($"The product with id {id} has been removed.");
        }
        else
        {
            Console.WriteLine($"The product with id {id} was not found.");
        }
    }
<<<<<<< HEAD

    public void UpdateById(int id)
    {
        var product = GetById(id);
        if (product != null)
        {
            //_products.Re;
            Console.WriteLine($"The product with id {id} has been removed.");
        }
        else
        {
            Console.WriteLine($"The product with id {id} was not found.");
        }
    }
=======
    public void Update(Product product)
    {
        var existing = GetById(product.ProductId);
        if (existing == null) return;
        existing.ProductName = product.ProductName;
        existing.ProductPrice = product.ProductPrice;
        Console.WriteLine($"The product with id {existing.ProductId} has been Updated.");
    }

>>>>>>> 6e5d0ae2bdfb067ffd30dae7721957be82b75663
}