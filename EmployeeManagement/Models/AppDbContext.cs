using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ):base(options) 
        {
            
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Admin> Admins { get; set; }
    }
}
