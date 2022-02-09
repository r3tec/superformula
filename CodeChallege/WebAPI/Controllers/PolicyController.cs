using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PolicyAPI.Data;
using PolicyAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PolicyController : ControllerBase
    {
        private readonly ILogger<PolicyController> logger;
        private readonly PolicyContext dbContext;
        private readonly IPolicyService service;
        public PolicyController(
            ILogger<PolicyController> lg,
            PolicyContext ctx,
            IPolicyService ps
            )
        {
            logger = lg;
            dbContext = ctx;
            service = ps;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ping()
        {
            try
            {
                dbContext.Database.EnsureCreated();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Application failed to communicate with database.");
                ModelState.AddModelError("Error", ex.Message);
                return new BadRequestObjectResult(new CustomErrorResponse(ControllerContext));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Policy p)
        {
            try
            {
                var policy = await service.AddPolicy(p);
                if(policy != null)
                    return CreatedAtAction("Create", policy);

                ModelState.AddModelError("Error", "Internal error");
                return new BadRequestObjectResult(new CustomErrorResponse(ControllerContext));

            }
            catch (PolicyException pex)
            {
                ModelState.AddModelError("Error", pex.Message);
                return new BadRequestObjectResult(new CustomErrorResponse(ControllerContext));
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Policy>> Get(long id, string license)
        {
            try
            {
                var policy = await service.GetPolicy(id, license);
                if (policy != null)
                    return policy;

                return NotFound();

            }
            catch (PolicyException pex)
            {
                ModelState.AddModelError("Error", pex.Message);
                return new BadRequestObjectResult(new CustomErrorResponse(ControllerContext));
            }
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Policy>>> GetAll(string license, string sortOrder = "None", bool returnExpired = false)
        {
            var sortOrdeValr = Enum.Parse<PolicyAPI.Data.Repository.SortOrder>(sortOrder);
            return await service.GetAll(license, sortOrdeValr, returnExpired);
        }

    }
}
