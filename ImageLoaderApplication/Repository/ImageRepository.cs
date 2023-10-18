using ImageLoaderApplication.Data;
using ImageLoaderApplication.Dto;
using ImageLoaderApplication.Model;

using Microsoft.EntityFrameworkCore;

namespace ImageLoaderApplication.Repository;

public class ImageRepository
{
    private ApplicationDbContext _dbContext;

    public ImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddImage(Image image)
    {
        _dbContext.Images.Add(image);

        _dbContext.SaveChanges();
    }

    public RepositoryResult<Image> GetImage(Guid guid, long userId)
    {
        Image? image = _dbContext.Images.Where(i => i.Id == guid)
            .Include(u => u.User)
            .ThenInclude(f => f.Friends).FirstOrDefault();

        if (image == null)
        {
            return new() { StatusCode = 404 };
        }

        // Владелец
        if (image.UserId == userId)
        {
            return new() { StatusCode = 200, Data = image };
        }

        // Друг
        if (image.User.Friends.Select(x => x.FriendId).Contains(userId))
        {
            return new() { StatusCode = 200, Data = image };
        }

        // Нет доступа
        return new() { StatusCode = 403 };
    }

    public List<Image> GetUserImages(long userId)
    {
        return _dbContext.Images.Where(i => i.UserId == userId).ToList();
    }
}
