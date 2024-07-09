using MediatR;
using InventoryManagement.Data.Entities;

namespace InventoryManagement.Features.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
