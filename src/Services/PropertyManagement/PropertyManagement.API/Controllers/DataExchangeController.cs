using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertyManagement.API.BackgroundTasks;
using PropertyManagement.API.Data;
using PropertyManagement.API.Responses;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.Controllers
{
    /// <summary>
    /// Resources dedicated to data exchanges across system boundaries.
    /// </summary>
    [Route("api/dataexchange")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class DataExchangeController : ControllerBase
    {
        private readonly IPropertyImportBackgroundTaskQueue _taskQueue;
        private readonly SmartApartmentDbContext _context;

        public DataExchangeController(IPropertyImportBackgroundTaskQueue taskQueue,
            SmartApartmentDbContext context)
        {
            _taskQueue = taskQueue;
            _context = context;
        }

        /// <summary>
        /// Import a json file containing properties.
        /// </summary>
        [HttpPost]
        [Route("property/import")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(TaskCreatedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> PropertyImport([BindRequired] IFormFile propertyFile, CancellationToken token)
        {
            if (propertyFile is null)
            {
                return BadRequest(new ErrorResponse("Property file cannot be empty"));
            }

            // Saving the IFormFile to a local file to be used later
            var filePath = Path.GetTempFileName();
            using var fileStream = System.IO.File.Create(filePath);
            await propertyFile.CopyToAsync(fileStream);


            // Spinning up a background task for long running import
            var task = new ImportPropertyTask(filePath);
            await _context.TaskDetails.AddAsync(task.TaskDetails, token);
            await _context.SaveChangesAsync(token);
            //Queue the task
            await _taskQueue.QueueBackgroundWorkItemAsync(task);

            var statusUrl = Url.Action("Index", "Tasks", new { taskId = task.Id });
            return Accepted(statusUrl,
                new TaskCreatedResponse(task.Id)
                {
                    Message = $"Property import process initiated. Check progress at {statusUrl}",
                });

        }

        /// <summary>
        /// Import a json file containing management companies.
        /// </summary>
        [HttpPost]
        [Route("management/import")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ManagementImport([BindRequired] IFormFile managementFile, CancellationToken token)
        {
            if (managementFile is null)
            {
                return BadRequest(new ErrorResponse("Management file cannot be empty"));
            }
            // Saving the IFormFile to a local file to be used later
            var filePath = Path.GetTempFileName();
            using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            await managementFile.CopyToAsync(fileStream);

            // Spinning up a background task for long running import
            var task = new ImportManagementCompanyTask(filePath);
            await _context.TaskDetails.AddAsync(task.TaskDetails, token);
            await _context.SaveChangesAsync();

            // Queue the task
            await _taskQueue.QueueBackgroundWorkItemAsync(task);
            var statusUrl = Url.Action("Index", "Tasks", new { taskId = task.Id });
            return Accepted(statusUrl,
                new TaskCreatedResponse(task.Id)
                {
                    Message = "Management company import process initiated. Check progress using tasks api.",
                });
        }

    }
}
