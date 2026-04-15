

using System.Text.Json.Serialization;

public class Product
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("productName")]
    public string ProductName { get; set; }
    [JsonPropertyName("productPrice")]
    public decimal ProductPrice { get; set; }
    public Product() { }
    public Product(int id, string name, decimal price)
    {
        ProductId = id;
        ProductName = name;
        ProductPrice = price;
    }
}