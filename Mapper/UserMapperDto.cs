using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Mapper;

public class UserMapperDto
{
    public static User ToUser(CreateUserRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Password = request.Password,
            Email = request.Email,
            Age =request.Age
        };
        
       
        return user;
    }
}