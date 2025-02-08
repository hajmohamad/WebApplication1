using System;
using WebApplication1.Models;

namespace WebApplication1.Interface;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
