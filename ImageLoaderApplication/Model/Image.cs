using System.ComponentModel.DataAnnotations;

namespace ImageLoaderApplication.Model;

public class Image
{
    [Key]
    public Guid Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
