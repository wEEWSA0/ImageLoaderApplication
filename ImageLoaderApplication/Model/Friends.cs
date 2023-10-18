using Microsoft.EntityFrameworkCore;

namespace ImageLoaderApplication.Model;

[PrimaryKey(nameof(UserId), nameof(FriendId))]
public class Friends
{
    public long UserId { get; set; }
    public User User { get; set; }

    public long FriendId { get; set; }
    public User Friend { get; set; }
}
