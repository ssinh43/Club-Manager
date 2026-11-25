namespace clubmanager.Models
{
    public class RegistrationViewModel
    {
        public int RegistrationID { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }

        public DateTime StartDate { get; set; }

        public string? Location { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}