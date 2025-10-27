namespace _123Vendas.Domain.Sales.Domain.ValueObjects;

/// <summary>
/// External Identity para referenciar fial do domínio ESTOque
/// </summary>
public class Branch 
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Branch(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID da filial não pode ser vazio", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da filial não pode ser vazio", nameof(name));

        Id = id;
        Name = name.Trim();
    }
}