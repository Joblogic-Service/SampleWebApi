using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Domains;

namespace Sample.Repositories
{
    public interface IEmployeeRepository
    {
        void Insert(Employee employee);
        void Update(Employee employee);
        void Delete(int employeeId);

        IEnumerable<Employee> GetAll();

    }
    public class EmployeeRepository : IEmployeeRepository
    {

        public EmployeeRepository()
        {
        }

        private readonly EmployeeDbContext dbContext;

        public EmployeeRepository(EmployeeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Insert(Employee employee)
        {
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
        }

        public void Update(Employee employee)
        {
            var existingEmployee = dbContext.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Age = employee.Age;
                existingEmployee.Gender = employee.Gender;
                dbContext.SaveChanges();
            }
        }

        public void Delete(int employeeId)
        {
            var existingEmployee = dbContext.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (existingEmployee != null)
            {
                dbContext.Employees.Remove(existingEmployee);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            return dbContext.Employees.ToList();
        }
    }
}
