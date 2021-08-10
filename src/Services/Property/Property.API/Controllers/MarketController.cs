using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Property.API.Controllers
{
    /// <summary>
    /// Access market services
    /// </summary>
    [Route("api/markets/")]
    [ApiController]
    [Produces("application/json")]
    public class MarketController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
        public async Task<List<string>> All(CancellationToken token)
        {
            return new List<string>()
            {
                "San francisco",
                "DFW",
                "Abilene"
            };
        }
    }
}
