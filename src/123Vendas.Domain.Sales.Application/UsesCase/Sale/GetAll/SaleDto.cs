namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetAll
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
