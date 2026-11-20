using clubmanager.Models;
namespace clubmanager.ViewModels;
public class ClubListVm
{
    public List<Club> Clubs { get; set; } = new();
    public string? Search { get; set; }
    public string? Sport { get; set; }
    public string? Status { get; set; }
    public List<string> Sports { get; set; } = new();
    public int Total { get; set; }
    public int Active { get; set; }
    public int Members { get; set; }
    public int Pending { get; set; }
}
