using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLoaderApplication.Model;

public class User : IdentityUser<long>
{
    private List<Image> _images = new();
    public IReadOnlyCollection<Image> Images => _images.AsReadOnly();

    public List<Friends> Friends { get; set; } = new();
}
