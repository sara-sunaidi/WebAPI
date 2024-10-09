using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Authentication
{
    public class ApplicationRole : IdentityRole
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }
}
