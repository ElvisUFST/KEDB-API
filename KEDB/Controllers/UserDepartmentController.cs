using KEDB.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDepartmentsController : ControllerBase
    {
        private readonly GraphServiceClient _graphClient;
        private readonly IConfiguration _configuration;

        private readonly string USER_ID_CLAIM = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public UserDepartmentsController(GraphServiceClient graphClient, IConfiguration configuration)
        {
            _graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // [Authorize(Roles = "kedb-super, kedb-read")]
        [HttpGet("myDepartment")]
        public async Task<ActionResult> GetEmployees()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == USER_ID_CLAIM);
            if (userId == null)
            {
                throw new Exception($"Claim {USER_ID_CLAIM} not found.");
            }

            var userDepartments = await _graphClient.Users[userId.Value]
               .GetMemberGroups(false)
               .Request()
               .PostAsync();

            var usersDepartment = _configuration.GetSection("ToldDepartmentIds")
                .GetChildren()
                .Select(departmentId => departmentId.Value)
                .Intersect(userDepartments)
                .FirstOrDefault();

            if (usersDepartment == null)
            {
                return NotFound();
            }

            return Ok(await GetDepartment(usersDepartment));
        }

        private async Task<List<ADUserDto>> GetDepartment(string departmentId)
        {
            var employees = await _graphClient.Groups[departmentId].Members
               .Request()
               .GetAsync();

            return
                employees
                .OfType<User>()
                .Select(employee => new ADUserDto { Id = Guid.Parse(employee.Id), Name = employee.DisplayName })
                .OrderBy(employee => employee.Name)
                .ToList();
        }
    }
}