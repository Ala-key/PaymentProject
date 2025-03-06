namespace DataLayer.Models
{
    /// <summary>
    /// Платежи.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Сумма платежа.
        /// </summary>
        public decimal Sum { get; set; }
        
        /// <summary>
        /// Дата платежа.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Ид пользователя.
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Пользователь совершивший платеж.
        /// </summary>
        public User User { get; set; }
    }
}
