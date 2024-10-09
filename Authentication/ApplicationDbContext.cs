using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ClientModel> Clients { get; set; }
        //public DbSet<ClientModel> Clients { get; set; }
        public string ConnectionString { get; } = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TestAPI;Integrated Security=True";


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        //public bool AddClient(ClientModel client)
        //{
        //    using (var context = new this.cl)
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);

            //base.OnConfiguring(optionsBuilder);
        }

        public ApplicationDbContext()
        {
            ConnectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TestAPI;Integrated Security=True";
        }
    }
}
