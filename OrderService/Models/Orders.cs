namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }   // FK to UserService
        public int BookId { get; set; }   // FK to BookService
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
