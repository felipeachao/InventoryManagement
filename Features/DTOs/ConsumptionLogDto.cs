namespace InventoryManagement.Features.DTOs
{
    public class ConsumptionLogDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
        public int QuantityConsumed { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
    }
}
