using System.ComponentModel.DataAnnotations;
namespace clubmanager.Models;
public class ClubMember
{
    public int Id { get; set; }
    [Required] public int ClubId { get; set; }
    public Club? Club { get; set; }
    [Required(ErrorMessage="Nhập mã học sinh")][StringLength(30)] [Display(Name="Mã học sinh")] public string StudentCode { get; set; } = "";
    [Required(ErrorMessage="Nhập họ tên")][StringLength(100)] [Display(Name="Họ và tên")] public string FullName { get; set; } = "";
    [StringLength(30)] [Display(Name="Lớp")] public string? ClassName { get; set; }
    [EmailAddress(ErrorMessage="Email không hợp lệ")][StringLength(150)] public string? Email { get; set; }
    [Phone(ErrorMessage="Số điện thoại không hợp lệ")][StringLength(20)] [Display(Name="Số điện thoại")] public string? Phone { get; set; }
    [Required] [StringLength(30)] [Display(Name="Vai trò")] public string Role { get; set; } = "Thành viên";
    [Display(Name="Ngày tham gia")] public DateTime JoinedOn { get; set; } = DateTime.Today;
}
