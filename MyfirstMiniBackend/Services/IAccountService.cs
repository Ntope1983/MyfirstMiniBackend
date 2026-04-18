public interface IAccountService
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    void AddProduct(string name, decimal price);
    void DeleteProduct(int id);
    void UpdateProduct(int id, string? name, decimal? price);
}