using System.Text.Json.Serialization;

namespace InventoryManagement.Data.Entities
{
    public class ConsumptionLog
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityConsumed { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }

        [JsonIgnore]
        public Product Product { get; set; } = null!;
    }
}
