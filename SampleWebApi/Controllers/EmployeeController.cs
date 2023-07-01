using Microsoft.AspNetCore.Mvc;
using Sample.Domains;
using Sample.Services;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAsync([FromBody] Employee employeeDto)
        {
            // Input validation
            if (employeeDto == null)
            {
                return BadRequest("Invalid employee data");
            }

            // Sanitize inputs
            employeeDto.FirstName = ValidateInput(employeeDto.FirstName);
            employeeDto.LastName = ValidateInput(employeeDto.LastName);
            employeeDto.Gender = ValidateInput(employeeDto.Gender);

            // Create Employee object from DTO
            var employee = new Employee
            {
                Id = employeeDto.Id,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Age = employeeDto.Age,
                Gender = employeeDto.Gender
            };

            // Save the employee using the service
            await Task.Run(() => employeeService.Save(employeeDto));

            return Ok("Employee saved successfully");
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync(string firstNameFilter, string lastNameFilter, string genderFilter)
        {
            // Sanitize inputs
            firstNameFilter = ValidateInput(firstNameFilter);
            lastNameFilter = ValidateInput(lastNameFilter);
            genderFilter = ValidateInput(genderFilter);

            // Retrieve employees using the service
            var employees = await Task.Run(() => employeeService.GetAll(firstNameFilter, lastNameFilter, genderFilter));

            return Ok(employees);
        }

        private string ValidateInput(string input)
        {
            // Remove HTML tags using regular expression
            string resultInput = Regex.Replace(input, "<.*?>", string.Empty);

            // Remove special characters using regular expression
            resultInput = Regex.Replace(resultInput, "[^a-zA-Z0-9]", string.Empty);

            return resultInput;
        }
    }
}
