public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    void AddProduct(string name, decimal price);
    void DeleteProduct(int id);
<<<<<<< HEAD
    void UpdateProduct(int id);
=======
    void UpdateProduct(int id, string? name, decimal? price);
>>>>>>> 6e5d0ae2bdfb067ffd30dae7721957be82b75663
}