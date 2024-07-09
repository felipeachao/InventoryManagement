namespace InventoryManagement.Data.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty; 
        public DateTime Date { get; set; }
    }
}
