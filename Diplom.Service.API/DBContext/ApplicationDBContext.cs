using Diplom.Service.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.API.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
        
    }
}
