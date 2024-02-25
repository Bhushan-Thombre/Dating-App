using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UsersController : ControllerBase
{

    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
        _context = context;
    }

    // The ActionResult types represent various HTTP status codes.
    [HttpGet] // /api/users/
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        var users = _context.Users.ToList();
        return users;
    }

    [HttpGet("{id}")] // /api/users/:id
    public ActionResult<AppUser> GetUser(int id)
    {
        var user = _context.Users.Find(id);
        return user;
    }
}