using PMS.Models;

namespace PMS.Data;

public sealed class ProductStore
{
    private readonly object sync = new();
    private readonly Dictionary<int, Product> products = new()
    {
        [1] = new Product(1, "Notebook", "A ruled paper notebook", 4.99m, 120),
        [2] = new Product(2, "Desk Lamp", "LED desk lamp", 24.50m, 35),
        [3] = new Product(3, "Pen Set", "Pack of five blue pens", 3.25m, 80)
    };

    private int nextId = 4;

    public IReadOnlyList<Product> GetAll()
    {
        lock (sync)
        {
            return products.Values.OrderBy(product => product.Id).ToArray();
        }
    }

    public Product? GetById(int id)
    {
        lock (sync)
        {
            return products.GetValueOrDefault(id);
        }
    }

    public Product Create(ProductRequest request)
    {
        lock (sync)
        {
            var product = new Product(
                nextId++,
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity);

            products.Add(product.Id, product);
            return product;
        }
    }

    public bool Update(int id, ProductRequest request)
    {
        lock (sync)
        {
            if (!products.ContainsKey(id))
            {
                return false;
            }

            products[id] = new Product(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity);

            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (sync)
        {
            return products.Remove(id);
        }
    }
}
