using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Property.API.BackgroundTasks;
using Property.API.Responses;
using Property.API.Search;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Property.API.Controllers
{
    /// <summary>
    /// Resources dedicated to data exchanges across system boundaries.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DataExchangeController : ControllerBase
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IPropertyImportBackgroundTaskQueue _taskQueue;

        public DataExchangeController(ISearchProvider provider, IPropertyImportBackgroundTaskQueue taskQueue)
        {
            _searchProvider = provider;
            _taskQueue = taskQueue;
        }

        /// <summary>
        /// Import a json file containing properties.
        /// </summary>
        [HttpPost]
        [Route("api/dataExchange/property/import")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
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
            await _taskQueue.QueueBackgroundWorkItemAsync(new ImportPropertyTask(filePath));

            return Ok(new SuccessResponse() { Message = "Property indexed successfully." });

        }

        /// <summary>
        /// Import a json file containing management companies.
        /// </summary>
        [HttpPost]
        [Route("api/dataExchange/management/import")]
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

            // Spinning up a background task to import management companies
            await _taskQueue.QueueBackgroundWorkItemAsync(new ImportManagementCompanyTask(filePath));
            return Ok(new SuccessResponse() { Message = "Management Company indexed successfully." });
        }

    }
}
