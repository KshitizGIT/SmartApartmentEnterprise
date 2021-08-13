using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Search;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchProvider _searchProvider;

        public SearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q,
                                                     [FromQuery] string market,
                                                     [FromQuery] int limit = 25)
        {
            var searchResults = await _searchProvider.SearchEntities(q, market, limit);
            return Ok(searchResults.ToList());
        }

    }
}
