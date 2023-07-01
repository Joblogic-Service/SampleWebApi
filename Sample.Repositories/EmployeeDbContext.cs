using Sample.Domains;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Reflection.Emit;

public class EmployeeDbContext : DbContext
{
    public virtual DbSet<Employee> Employees { get; set; }

    public EmployeeDbContext() : base("DatabaseConnectionString")
    {
    }
}

public class EmployeeDbConfiguration : DbMigrationsConfiguration<EmployeeDbContext>
{
    public EmployeeDbConfiguration()
    {
        AutomaticMigrationsEnabled = true;
    }
}