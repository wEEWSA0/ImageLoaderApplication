using ImageLoaderApplication.Data;
using ImageLoaderApplication.Model;

using Microsoft.EntityFrameworkCore;

namespace ImageLoaderApplication.Repository;

public class UserRepository
{
    private ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? GetUser(string name)
    {
        return _dbContext.Users.Where(u => u.NormalizedUserName == name.ToUpper()).SingleOrDefault();
    }

    public User? GetUserWithFriendsAndImages(string name)
    {
        return _dbContext.Users.Where(u => u.NormalizedUserName == name.ToUpper())
            .Include(c => c.Friends)
            .Include(c => c.Images).SingleOrDefault();
    }
}
