public interface IProductRepository
{
    IEnumerable<Product> GetProducts();
    Product? GetById(int id);
    void Add(Product product);
    void DeleteById(int id);
    void UpdateById(int id);
}