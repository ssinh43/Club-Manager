using System.ComponentModel.DataAnnotations;
namespace clubmanager.Models;
public class Club
{
    public int Id { get; set; }
    [Required(ErrorMessage="Vui lòng nhập mã CLB")][StringLength(20)] [Display(Name="Mã câu lạc bộ")] public string Code { get; set; } = "";
    [Required(ErrorMessage="Vui lòng nhập tên CLB")][StringLength(120)] [Display(Name="Tên câu lạc bộ")] public string Name { get; set; } = "";
    [Required(ErrorMessage="Vui lòng chọn môn thể thao")][StringLength(60)] [Display(Name="Môn thể thao")] public string Sport { get; set; } = "";
    [StringLength(1000)] [Display(Name="Giới thiệu")] public string? Description { get; set; }
    [StringLength(150)] [Display(Name="Địa điểm sinh hoạt")] public string? Location { get; set; }
    [StringLength(150)] [Display(Name="Lịch sinh hoạt")] public string? Schedule { get; set; }
    [Range(1,10000,ErrorMessage="Sức chứa phải lớn hơn 0")] [Display(Name="Số thành viên tối đa")] public int Capacity { get; set; } = 50;
    [StringLength(20)] [Display(Name="Trạng thái")] public string Status { get; set; } = "Hoạt động";
    [Display(Name="Ngày thành lập")] [DataType(DataType.Date)] public DateTime FoundedOn { get; set; } = DateTime.Today;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ClubMember> Members { get; set; } = new();
    public List<JoinRequest> JoinRequests { get; set; } = new();
}
