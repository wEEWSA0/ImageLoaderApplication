using Microsoft.AspNetCore.Identity;

namespace ImageLoaderApplication.Model;

public class User : IdentityUser<long>
{
    public List<Image> Images { get; set; } = new();
    public List<Friends> Friends { get; set; } = new();
}
