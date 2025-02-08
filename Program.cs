using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Interface;
using WebApplication1.Models;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>{
    options.Password.RequireDigit=true;
    options.Password.RequireLowercase=true;
    options.Password.RequireUppercase=true;
    options.Password.RequireNonAlphanumeric=true;
    options.Password.RequiredLength=6;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddAuthentication(options=>{
        options.DefaultAuthenticateScheme= 
        options.DefaultScheme =
        options.DefaultChallengeScheme = 
        options.DefaultForbidScheme =
        options.DefaultSignInScheme = 
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;


    }).AddJwtBearer(Option =>{
        Option.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Signinkey"])), 


        };
    });
    builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
