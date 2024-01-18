using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sample.Domains;

namespace Sample.Repositories
{
	public class DBContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DBContext(IConfiguration configuration)
		{
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect database
            options.UseSqlServer(Configuration.GetConnectionString("SampleWebAPI"));
        }

        //DB tables
        public DbSet<Employee> Employee { get; set; }
    }
}

