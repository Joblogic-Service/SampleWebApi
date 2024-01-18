using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Domains;
using Sample.Repositories;

namespace Sample.Services
{
    public interface IEmployeeService
    {
        Task<ResponseViewModel<bool>> Save(Employee employee);
        ResponseViewModel<List<Employee>> GetAll(EmployeeSearchModel employeeSearchModel);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly DBContext _context;
        private readonly IEmployeeRepository _repository;
        public EmployeeService(IEmployeeRepository repository,DBContext context)
        {
            _repository = repository;
            _context = context;
        }

        public ResponseViewModel<List<Employee>> GetAll(EmployeeSearchModel employeeSearchModel)
        {
            var listOfEmployees = _context.Employee.ToList();

            if (!string.IsNullOrEmpty(employeeSearchModel.firstNameFilter))
                listOfEmployees = listOfEmployees.Where(e => e.FirstName.Contains(employeeSearchModel.firstNameFilter, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(employeeSearchModel.lastNameFilter))
                listOfEmployees = listOfEmployees.Where(e => e.LastName.Contains(employeeSearchModel.lastNameFilter, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(employeeSearchModel.genderFilter))
                listOfEmployees = listOfEmployees.Where(e => e.Gender.Equals(employeeSearchModel.genderFilter, StringComparison.OrdinalIgnoreCase)).ToList();

            return new ResponseViewModel<List<Employee>> { Data = listOfEmployees, Message = "Employee Data", StatusCode = System.Net.HttpStatusCode.OK };
        }

        public async Task<ResponseViewModel<bool>> Save(Employee employee)
        {
            if (employee == null || employee.Id == 0)
                return new ResponseViewModel<bool> { Data = false, Message = "Wrong Data", StatusCode = System.Net.HttpStatusCode.BadRequest };

            var existingEmployee = await _repository.GetById(employee.Id);

            var savingResponse = existingEmployee == null ? await _repository.Insert(employee) : await _repository.Update(employee);

            return new ResponseViewModel<bool> { Data = savingResponse, Message = "Data Saved", StatusCode = System.Net.HttpStatusCode.OK };
        }
    }
}
