using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthService;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    
}