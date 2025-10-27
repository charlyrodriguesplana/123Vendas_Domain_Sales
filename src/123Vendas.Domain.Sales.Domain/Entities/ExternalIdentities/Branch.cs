namespace _123Vendas.Domain.Sales.Domain.Entities.ExternalIdentities;

/// <summary>
/// External Identity para referenciar fial do domínio ESTOque
/// </summary>
public class Branch 
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }

    public Branch(Guid id, string name, string code)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Branch ID cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Branch code cannot be empty", nameof(code));

        Id = id;
        Name = name.Trim();
        Code = code.Trim().ToUpper();
    }
}