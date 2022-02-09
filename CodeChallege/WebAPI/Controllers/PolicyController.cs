using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(ILogger<PolicyController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ping()
        {
            try
            {
                //using (SqlConnection conn = await DatabaseAccess.CreateSqlConnection(_databaseSettings.Value.ConnectionString).ConfigureAwait(false))
                //{
                //    // just make sure we can retrieve data from the database
                //    int count = SqlUtils.ExecuteScalar<int>(conn, "SELECT COUNT(*) FROM [dbo].[DocumentTypes]", CommandType.Text);
                //}
                throw new Exception("No db yet"); 
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Application failed to communicate with database.");
                ModelState.AddModelError("Error", ex.Message);
                return new BadRequestObjectResult(new CustomErrorResponse(ControllerContext));
            }
        }

    }
}
