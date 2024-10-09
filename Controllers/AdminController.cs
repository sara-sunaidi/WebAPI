using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.JavaScript;
using WebApplication2.Authentication;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        [Authorize(Roles =  ApplicationRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientModel model)
        {

            using (var context = new ApplicationDbContext())
            {
                bool exist = context.Clients.Any(row => row.PersonalID == model.PersonalID);
                if (exist)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Client already exist" });
                }
                else
                {
                    await context.Clients.AddAsync(model);
                    var result = await context.SaveChangesAsync();

                    return Ok(StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Client Created successfully" }));

                }
            }

        }

        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {

            using (var context = new ApplicationDbContext())
            {
                return Ok(StatusCode(StatusCodes.Status200OK, context.Clients.ToList()));
            }
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetSortedClients([FromBody] Dictionary<string,string> sortedProperty)
        {

            using (var context = new ApplicationDbContext())
            {
                List<ClientModel> returnedList = default;
                string sortedValue = sortedProperty["SortedProperty"];
                if (sortedValue != null)
                {

                switch (sortedValue)
                {
                    case "FirstName":
                        returnedList = context.Clients.OrderBy(row => row.FirstName).ToList();
                        break;

                    case "LastName":
                        returnedList = context.Clients.OrderBy(row => row.LastName).ToList();
                        break;

                    case "PersonalID":
                        returnedList = context.Clients.OrderBy(row => row.PersonalID).ToList();
                        break;
                    case "Gender":
                        returnedList = context.Clients.OrderBy(row => row.Gender).ToList();
                        break;
                        
                    default:
                         returnedList = context.Clients.ToList();
                         break;

                    }
                }

                return Ok(StatusCode(StatusCodes.Status200OK, returnedList));
            }
        }

    }
}
