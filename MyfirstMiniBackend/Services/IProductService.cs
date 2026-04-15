public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    void AddProduct(string name, decimal price);
    void DeleteProduct(int id);
}