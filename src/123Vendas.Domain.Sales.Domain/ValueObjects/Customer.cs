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
            throw new ArgumentException("ID do cliente não pode ser vazio", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do cliente não pode ser vazio", nameof(name));

        Id = id;
        Name = name.Trim();
    }
}