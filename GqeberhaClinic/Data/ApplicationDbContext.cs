using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GqeberhaClinic.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<GqebheraUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserInformation> users { get; set; }
    }
}