using ImageLoaderApplication.Data;
using ImageLoaderApplication.Model;

namespace ImageLoaderApplication.Repository;

public class FriendsRepository
{
    private ApplicationDbContext _dbContext;

    public FriendsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddFriends(Friends friends)
    {
        _dbContext.Friends.Add(friends);

        _dbContext.SaveChanges();
    }

    public bool HasFriend(long userId, long friendId)
    {
        return _dbContext.Friends.Where(f => f.UserId == userId && f.FriendId == friendId).FirstOrDefault() is null ? false : true;
    }

    public List<User> GetUserFriends(long userId)
    {
        return _dbContext.Friends.Where(f => f.UserId == userId)
            .Select(f => f.Friend).ToList();
    }
}
