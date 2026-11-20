using System.ComponentModel.DataAnnotations;

namespace clubmanager.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sự kiện không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sự kiện tối đa 200 ký tự")]
        public string EventName { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime? EventDate { get; set; }

        [StringLength(200, ErrorMessage = "Địa điểm tối đa 200 ký tự")]
        public string? Location { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? Description { get; set; }
    }
}