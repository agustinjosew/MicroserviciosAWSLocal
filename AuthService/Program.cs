using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
var app = builder.Build();
var logger = LogManager.Setup().LoadConfigurationFromAppSettings("nlog.config").GetCurrentClassLogger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapPost("/login", async (LoginModel loginModel, UserManager<ApplicationUser> userManager, IConfiguration configuration) =>
    {
        if (loginModel.Email != null)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (loginModel.Password != null && user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                logger.Info("Inicio de sesión exitoso para el usuario con ID: {UserId}", user.Id); // Usa el ID en lugar del email
                var token = GenerateJwtToken(user, configuration);
                return Results.Ok(new { Token = token });
            }
        }

        logger.Warn("Intento de inicio de sesión fallido para un usuario con Email: {Email}", loginModel.Email);
        return Results.Unauthorized();
    })
    .WithName("Login")
    .WithTags("Authentication");

static string GenerateJwtToken(ApplicationUser user, IConfiguration configuration)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new InvalidOperationException()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:ExpireMinutes"]!));

    var token = new JwtSecurityToken(
        configuration["Jwt:Issuer"],
        configuration["Jwt:Audience"],
        claims,
        expires: expires,
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.Run();