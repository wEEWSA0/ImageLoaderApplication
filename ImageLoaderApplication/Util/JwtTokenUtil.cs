using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace ImageLoaderApplication.Util;

public class JwtTokenUtil
{
    private string _jwtTokenKey;
    private string _jwtTokenIssuer;
    private string _jwtTokenAudience;
    public int _jwtTokenExpiredTime;

    public JwtTokenUtil(string jwtTokenKey, int jwtTokenExpiredTime, string jwtTokenIssuer, string jwtTokenAudience)
    {
        _jwtTokenKey = jwtTokenKey;
        _jwtTokenExpiredTime = jwtTokenExpiredTime;
        _jwtTokenIssuer = jwtTokenIssuer;
        _jwtTokenAudience = jwtTokenAudience;

    }

    public string GenerateToken(List<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
                issuer: _jwtTokenIssuer,
                audience: _jwtTokenAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtTokenExpiredTime)),
                signingCredentials: new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private SymmetricSecurityKey GetKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenKey));
    }
}
