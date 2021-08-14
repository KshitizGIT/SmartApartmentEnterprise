using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Data;
using PropertyManagement.API.Models;
using PropertyManagement.API.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.Controllers
{
    /// <summary>
    /// API's related to query task status.
    /// </summary>
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly SmartApartmentDbContext _context;

        public TasksController(SmartApartmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Index([FromQuery] Guid taskId, CancellationToken token)
        {
            try
            {
                var task = await _context.TaskDetails.FindAsync(new object[] { taskId }, cancellationToken: token);

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message));
            }
        }
    }
}
