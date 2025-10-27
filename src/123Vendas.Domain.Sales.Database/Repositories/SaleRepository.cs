using _123Vendas.Domain.Sales.Domain.Entities;
using _123Vendas.Domain.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Domain.Sales.Database.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesDbContext _context;

        public SaleRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            return Task.CompletedTask;
        }

        //Isso poderia vir de um Unit of work, achei mais simples fazer isso, mas um projeto da vida real, eu provavelmente iria para algo como Unit of work!
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
