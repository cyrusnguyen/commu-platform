using API.Entities;

namespace API;
public class FriendRequest
{
    public AppUser SourceUser { get; set; }
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; }
    public int TargetUserId { get; set; }
}
