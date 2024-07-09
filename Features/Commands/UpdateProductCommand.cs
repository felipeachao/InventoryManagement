using MediatR;

namespace InventoryManagement.Features.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
