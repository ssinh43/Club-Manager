using System.ComponentModel.DataAnnotations;
namespace clubmanager.Models;
public class JoinRequest
{
    public int Id { get; set; }
    [Required] public int ClubId { get; set; }
    public Club? Club { get; set; }
    [Required(ErrorMessage="Nhập mã học sinh")][StringLength(30)] [Display(Name="Mã học sinh")] public string StudentCode { get; set; } = "";
    [Required(ErrorMessage="Nhập họ tên")][StringLength(100)] [Display(Name="Họ và tên")] public string FullName { get; set; } = "";
    [StringLength(30)] [Display(Name="Lớp")] public string? ClassName { get; set; }
    [EmailAddress][StringLength(150)] public string? Email { get; set; }
    [StringLength(500)] [Display(Name="Lý do tham gia")] public string? Message { get; set; }
    [StringLength(20)] public string Status { get; set; } = "Chờ duyệt";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}
