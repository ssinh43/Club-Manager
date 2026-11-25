using clubmanager.Models;
namespace clubmanager.ViewModels;
public class ClubDetailsVm
{
    public Club Club { get; set; } = new();
    public List<ClubMember> Members { get; set; } = new();
    public List<JoinRequest> Requests { get; set; } = new();
}
