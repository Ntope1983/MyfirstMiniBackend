public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Product> GetAllProducts() => _repository.GetProducts();

    public Product? GetProductById(int id) => _repository.GetById(id);

    public void AddProduct(string name, decimal price)
    {
        int id = _repository.GetProducts().Any()
            ? _repository.GetProducts().Max(p => p.ProductId) + 1
            : 1;

        var product = new Product(id, name, price);
        _repository.Add(product);
    }

    public void DeleteProduct(int id) => _repository.DeleteById(id);

<<<<<<< HEAD
    public void UpdateProduct(int id)
    {
        _repository.UpdateById(id);
    }
=======
    public void UpdateProduct(int id, string? name, decimal? price)
    {
        var product = _repository.GetById(id);
        if (product == null) return;

        if (name != null)
            product.ProductName = name;

        if (price.HasValue)
            product.ProductPrice = price.Value;

        _repository.Update(product);
    }


>>>>>>> 6e5d0ae2bdfb067ffd30dae7721957be82b75663
}