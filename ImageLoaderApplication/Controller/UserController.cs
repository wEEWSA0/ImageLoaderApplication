using ImageLoaderApplication.Model;
using ImageLoaderApplication.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLoaderApplication.Controller;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly FriendsRepository _friendsRepository;

    public UserController(UserRepository userRepository, FriendsRepository friendsRepository)
    {
        _userRepository = userRepository;
        _friendsRepository = friendsRepository;
    }

    [HttpGet("add-friend/{friendId}")]
    public IActionResult AddFriend(long friendId)
    {
        var user = _userRepository.GetUser(HttpContext.User.Identity!.Name!);

        if (user is null)
        {
            return Unauthorized();
        }

        _friendsRepository.AddFriends(new Friends()
        {
            UserId = user.Id,
            FriendId = friendId
        });

        return Ok();
    }
}
