namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetById
{
    public class SaleDetailDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<SaleItemDto> Items { get; set; } = new();
    }

    public class SaleItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalValue { get; set; }
    }
}
