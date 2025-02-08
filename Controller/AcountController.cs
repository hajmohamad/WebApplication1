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
    public readonly SignInManager<AppUser> _signInManager;

    public readonly ITokenService tokenService;

public AcountController(UserManager<AppUser> userManager, ITokenService tokenService,SignInManager<AppUser> signInManager){

    this.tokenService = tokenService;
    _userManager = userManager;
    _signInManager = signInManager;


}

//login

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto request){
    var user = await _userManager.FindByNameAsync(request.UserName.ToLower());
    if(user == null){
        return Unauthorized("User not found");
    }
    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
    if(!result.Succeeded){
        return Unauthorized("username or password is incorrect");
    }
    return Ok(new NewUserDto{
        UserName = user.UserName,
        Email = user.Email,
        Token = tokenService.CreateToken(user)
    });
}


//register 

[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto request){
    
try
{
    if(!ModelState.IsValid){
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
