using _123Vendas.Domain.Sales.Domain.Entities;

namespace _123Vendas.Domain.Sales.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
