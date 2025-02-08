using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Dtos.Acount;
using WebApplication1.Interface;
using WebApplication1.Models;

namespace WebApplication1.Controller;
[Route("api/[controller]")]
[ApiController]
public class AcountController : ControllerBase
{ 
    public readonly UserManager<AppUser> _userManager;

    public readonly ITokenService tokenService;

public AcountController(UserManager<AppUser> userManager, ITokenService tokenService){

    this.tokenService = tokenService;
    _userManager = userManager;

}


//register 

[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto request){
    
try
{
    if(!TryValidateModel(request)){
            return BadRequest(ModelState);

        }
    var appUser = new AppUser{
        UserName = request.UserName,
        Email = request.Email,
        
    };  
    
    
    var creatUser = await _userManager.CreateAsync(appUser, request.Password);
    if(creatUser.Succeeded)
    {
        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
        if(roleResult.Succeeded){
                return Ok(new NewUserDto{
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = tokenService.CreateToken(appUser)
                });   

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
