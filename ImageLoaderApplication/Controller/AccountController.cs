using ImageLoaderApplication.Dto;
using ImageLoaderApplication.Model;
using ImageLoaderApplication.Util;

using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace ImageLoaderApplication.Controller;

[ApiController]
[Route("[controller]/")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtTokenUtil _jwtTokenUtil;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, JwtTokenUtil jwtTokenUtil)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenUtil = jwtTokenUtil;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var user = new User
        {
            UserName = request.Name,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return BadRequest();
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, request.UserName) };

        var token = _jwtTokenUtil.GenerateToken(claims);

        return Ok(token);
    }
}
