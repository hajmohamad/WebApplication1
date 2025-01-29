using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Admin
{ 
    [Key] // Annotation for primary key
    public int Id { get; set; }
    public string UserName  { get; set; } 
    public string Password {get;set;} 

    public List<User> AllUser {get;set;} 

}