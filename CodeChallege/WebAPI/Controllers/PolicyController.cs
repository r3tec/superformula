using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using PolicyAPI.Data;
using PolicyAPI.Data.Repository;
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
        private readonly PolicyContext dbContext;
        public PolicyController(
            ILogger<PolicyController> logger,
            PolicyContext ctx
            )
        {
            _logger = logger;
            dbContext = ctx;
        }

        [HttpGet("Ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ping()
        {
            try
            {
                if (!dbContext.Database.EnsureCreated()) 
                    throw new ApplicationException("no db created");

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
