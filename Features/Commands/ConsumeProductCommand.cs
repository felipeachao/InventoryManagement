using MediatR;

namespace InventoryManagement.Features.Commands
{
    public class ConsumeProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
