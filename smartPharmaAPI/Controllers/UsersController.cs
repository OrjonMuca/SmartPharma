using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;
using smartPharmaAPI.Services;

namespace smartPharmaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class UsersController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;
    private readonly PasswordService _passwordService;

    public UsersController(SmartPharmaDbContext db, PasswordService passwordService)
    {
        _db = db;
        _passwordService = passwordService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _db.Users
            .OrderBy(user => user.FullName)
            .Select(user => new UserResponse(user.Id, user.FullName, user.Email, user.Role.ToString()))
            .ToListAsync(cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Full name, email, and password are required.");
        }

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            return BadRequest("Role must be Admin, Pharmacist, or Customer.");
        }

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _db.Users.AnyAsync(user => user.Email == email, cancellationToken))
        {
            return Conflict("A user with this email already exists.");
        }

        var user = new AppUser
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = _passwordService.Hash(request.Password),
            Role = role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        var response = new UserResponse(user.Id, user.FullName, user.Email, user.Role.ToString());
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, response);
    }

    [HttpPatch("{id:guid}/role")]
    public async Task<ActionResult<UserResponse>> UpdateRole(Guid id, UpdateUserRoleRequest request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            return BadRequest("Role must be Admin, Pharmacist, or Customer.");
        }

        var user = await _db.Users.FindAsync([id], cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        user.Role = role;
        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new UserResponse(user.Id, user.FullName, user.Email, user.Role.ToString()));
    }
}
