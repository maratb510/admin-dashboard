namespace AdminDashboardAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal AmountT { get; set; }
        public Client? Client { get; set; } // Вложено имя/email клиента
    }
}
