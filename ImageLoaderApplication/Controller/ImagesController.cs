using ImageLoaderApplication.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLoaderApplication.Controller;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ImageRepository _imageRepository;
    private readonly UserRepository _userRepository;
    private string _path { get; init; }

    public ImagesController(IConfiguration configuration, ImageRepository imageRepository, UserRepository userRepository)
    {
        _configuration = configuration;
        _imageRepository = imageRepository;
        _userRepository = userRepository;

        _path = _configuration.GetValue<string>("ImagesStorage:DirectoryPath")!;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest();
        }

        var id = Guid.NewGuid();
        var filePath = Path.Combine(_path, id.ToString()) + "." + file.FileName.Split('.').Last();

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var user = _userRepository.GetUser(HttpContext.User.Identity!.Name!);

        if (user is null)
        {
            return BadRequest();
        }

        _imageRepository.AddImage(new()
        {
            Id = id,
            UserId = user.Id
        });

        return Ok();
    }

    [HttpGet("{guid}")]
    public IActionResult GetImage(Guid guid)
    {
        var image = _imageRepository.GetImage(guid);

        if (image is null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(_path, guid + ".jpg"); // Предполагается, что изображения хранятся в формате JPG

        if (System.IO.File.Exists(filePath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }

        return NotFound();
    }
}

