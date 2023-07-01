using Sample.Domains;
using Sample.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sample.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public EmployeeService() { }

        public void Save(Employee employee)
        {
            if (employee.Id == 0)
            {
                // Employee is new, insert into the database
                employeeRepository.Insert(employee);
            }
            else
            {
                // Employee already exists, update in the database
                employeeRepository.Update(employee);
            }
        }

        public IEnumerable<Employee> GetAll(string firstNameFilter = null, string lastNameFilter = null, string genderFilter = null)
        {
            IEnumerable<Employee> employees = employeeRepository.GetAll();

            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                employees = employees.Where(e => e.FirstName.ToLower().Contains(firstNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                employees = employees.Where(e => e.LastName.ToLower().Contains(lastNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(genderFilter))
            {
                employees = employees.Where(e => e.Gender.ToLower() == genderFilter.ToLower());
            }

            return employees;
        }


    }
}
