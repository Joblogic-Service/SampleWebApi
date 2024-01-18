using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sample.Services;
using Sample.Domains;
using System.Collections.Generic;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost("Save")]
        [ProducesResponseType(typeof(ResponseViewModel<bool>), 200)]
        public async Task<IActionResult> Save([FromBody]Employee employee)
        {
            var res = await _service.Save(employee);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ResponseViewModel<List<Employee>>), 200)]
        public IActionResult GetAll([FromBody]EmployeeSearchModel employee)
        {
            var res = _service.GetAll(employee);
            return Ok(res);
        }
    }
}
