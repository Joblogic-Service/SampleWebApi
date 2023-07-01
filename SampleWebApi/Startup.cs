using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Repositories;
using Sample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Sample.Domains;
using System.Linq.Expressions;

namespace SampleWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<EmployeeDbContext>(provider =>
            {
                return new EmployeeDbContext();
            });

            services.AddControllers();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<EmployeeService>();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                DataSeeder.SeedData(scope.ServiceProvider);
            }
        }

        public static class DataSeeder
        {
            public static void SeedData(IServiceProvider serviceProvider)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

                    // Perform your database seeding logic here
                    if (!dbContext.Employees.Any())
                    {
                        dbContext.Employees.AddRange(new[]{
                                       new Employee { Id = 1, FirstName = "Sudhakar", LastName = "Mangalarapu", Age = 30, Gender = "Male" },
                                       new Employee { Id = 2, FirstName = "Geetha", LastName = "Kokkirala", Age = 25, Gender = "Female" }
                        });

                        dbContext.SaveChanges();
                    }
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
