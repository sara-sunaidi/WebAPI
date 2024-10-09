using WebApplication2.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //for entity framework service
            builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));


            //for identity service
            builder.Services.AddIdentity <ApplicationUser, IdentityRole > (options => 
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = false,
                }
            )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            //Add authentication
            builder.Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                //adding JWT bearer
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidateAudience"],
                        ValidIssuer = builder.Configuration["JWT:ValidateIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                    };
                });


           var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
