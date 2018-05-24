using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Service.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=insyphersql1;Initial Catalog=RightOnBoard;Persist Security Info=True;User ID=rightonboard;Password=rightonboard22"); //p_$@,83L6$z~23mW
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<UserAudit> UserAuditEvents { get; set; }
    }
}
