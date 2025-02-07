using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controller;
[Route("api/[controller]")]
[ApiController]
public class AcountController : ControllerBase
{ 
    public readonly UserManager<AppUser> _userManager;
public AcountController(UserManager<AppUser> userManager){


    _userManager = userManager;

}


//register 

[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] CreateUserRequest request){
    
try
{

    var appUser = new AppUser{
        UserName = request.UserName,
        Email = request.Email,
        
    };  
    
    if(!TryValidateModel(appUser)){
        return BadRequest(ModelState);

    }
    var creatUser = await _userManager.CreateAsync(appUser, request.Password);
    if(creatUser.Succeeded)
    {
        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
        if(roleResult.Succeeded){
                return Ok("User Created");   

        }else {
            return statusCode(500,roleResult.Errors);
        }
    }
    else{
          return statusCode(500,creatUser.Errors);
    }
    
}
catch (Exception  e)
{
    
    return StatusCode(500,e.Message);
    }



}

    private IActionResult statusCode(int v, IEnumerable<IdentityError> errors)
    {
        throw new NotImplementedException();
    }

}
