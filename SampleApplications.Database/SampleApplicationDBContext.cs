using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SampleApplications.Database
{
    public class SampleApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public SampleApplicationDBContext() : base()
        {

        }
        public SampleApplicationDBContext(DbContextOptions<SampleApplicationDBContext> options) : base(options) { 
        
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}