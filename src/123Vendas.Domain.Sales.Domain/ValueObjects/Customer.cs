namespace _123Vendas.Domain.Sales.Domain.ValueObjects;

/// <summary>
/// External Identity para referenciar clientes do domínio CRM
/// </summary>
public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }

    public Customer(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Customer ID cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        Id = id;
        Name = name.Trim();
    }
}