using System;
using System.Threading.Tasks;
using Sample.Domains;


namespace Sample.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> Insert(Employee employee);
        Task<bool> Update(Employee employee);
        Task<bool> Delete(int employeeId);

        Task<Employee> GetById(int employeeId);
    }

    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        private readonly DBContext _context;
        public EmployeeRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int employeeId)
        {
            if (employeeId == 0)
                return false;

            var deleteResponse = await Delete(employeeId);

            return deleteResponse == true ? true : false;
        }

        public async Task<Employee> GetById(int employeeId)
        {
            return await GetById(employeeId);
        }

        public async Task<bool> Insert(Employee employee)
        {
            if (employee == null || employee.Id == 0)
                return false;

            var addResponse = await Add(employee);
            return addResponse == true ? true : false;
        }

        public async Task<bool> Update(Employee employee)
        {
            if (employee == null || employee.Id == 0)
                return false;
            var employeeDetails = await GetById(employee.Id);

            employeeDetails.FirstName = employee.FirstName;
            employeeDetails.LastName = employee.LastName;
            employeeDetails.Age = employee.Age;
            employeeDetails.Gender = employee.Gender;

            await Update(employeeDetails);

            return true;
        }
    }
}
