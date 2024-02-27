using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    [HttpPost("register")] // POST: /api/account/register?userName&password
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
        {
            return BadRequest("UserName is Taken");
        }

        // The using keyword is used when we ourselves want to dispose off the object after its been used.
        // The using statement defines a scope at the end of which an object is disposed.
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == loginDto.UserName);

        if (user == null)
        {
            return Unauthorized("Invalid Username");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password");
            }
        }
        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    // Returns true if the user with the userName already exists, or else returns false
    private async Task<bool> UserExists(string userName)
    {
        return await _context.Users.AnyAsync(user => user.UserName == userName.ToLower());
    }
}