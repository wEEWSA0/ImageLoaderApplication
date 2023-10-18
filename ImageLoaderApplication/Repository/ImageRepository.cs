using ImageLoaderApplication.Data;
using ImageLoaderApplication.Model;

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

    public Image? GetImage(Guid guid)
    {
        return _dbContext.Images.Where(i => i.Id == guid).FirstOrDefault();
    }

    public List<Image> GetUserImages(long userId)
    {
        return _dbContext.Images.Where(i => i.UserId == userId).ToList();
    }
}
