using InventoryManagement.Features.DTOs;
using MediatR;

namespace InventoryManagement.Features.Queries
{
    public class GetDailyConsumptionQuery : IRequest<IEnumerable<ConsumptionLogDto>>
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
