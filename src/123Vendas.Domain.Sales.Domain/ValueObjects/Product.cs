namespace _123Vendas.Domain.Sales.Domain.ValueObjects;

/// <summary>
/// External Identity para referenciar produtos do domínio de Estoque
/// </summary>
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }

    public Product(Guid id, string name, string code)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Product code cannot be empty", nameof(code));

        Id = id;
        Name = name.Trim();
        Code = code.Trim().ToUpper();
    }
}