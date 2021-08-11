using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.Controllers
{
    /// <summary>
    /// Access market services
    /// </summary>
    [Route("api/markets/")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class MarketController : ControllerBase
    {
        private readonly SmartApartmentDbContext _context;

        public MarketController(SmartApartmentDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
        public async Task<List<string>> All(CancellationToken token)
        {
            return await Task.FromResult(_context.Properties.Select(s => s.Market.ToUpper()).Distinct().ToList());
        }
    }
}
