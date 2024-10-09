using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Authentication;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

       public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if(userExist != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exist"});
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, model.Password);
       
            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
            }

            return Ok(StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User Created successfully" }));
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var useRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid .NewGuid().ToString()),
                };



                foreach (var userRole in useRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidateIssuer"],
                    audience: _configuration["JWT:ValidateAudience"],
                    expires: DateTime.Now.AddHours(6),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    status = "Successfully logged in",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }
            return Unauthorized();
            {

            }
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Admin already exist" });
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin creation failed" });
            }
            if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(ApplicationRole.Admin));
            
            
            

            if (!await _roleManager.RoleExistsAsync(ApplicationRole.User))
                await _roleManager.CreateAsync(new IdentityRole(ApplicationRole.User));

            if (await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
                await _userManager.AddToRoleAsync(user, ApplicationRole.Admin);



            return Ok(StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Admin Created successfully" }));
        }
    }
}
