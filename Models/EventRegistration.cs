namespace clubmanager.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }   // đổi kiểu nếu khóa của tblUser không phải int
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}