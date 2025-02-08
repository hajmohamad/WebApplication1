using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.Acount;

public class RegisterDto
{

    [Required]
    public String?  UserName { get; set; }

    [Required]
    [EmailAddress]
    public String? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    //upercase and lowercase and number and special character
    [RegularExpression(@"^(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$")]
    
    public String? Password { get; set; }


    

    
}
