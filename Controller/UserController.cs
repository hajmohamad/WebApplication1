using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Mapper;
using WebApplication1.Models;

namespace WebApplication1.Controller;

[Route("api/User")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext dbContext)
    {
        _context = dbContext;


    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        
        var users = Mapper.UserMapperDto.ToUser(request);
         if (!TryValidateModel(users))
    {
        return BadRequest(ModelState);
    }
        await _context.Users.AddRangeAsync(users); 
        await _context.SaveChangesAsync();
        return Ok(users);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateUserRequest request){
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        user.UserName = request.UserName;
        user.Password = request.Password;
        user.Email = request.Email;
        _context.SaveChanges();
        return Ok(user); 
        
        
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id){
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
        return Ok(user); 
        
        
    }



}