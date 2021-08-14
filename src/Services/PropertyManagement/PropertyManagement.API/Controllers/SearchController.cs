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
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q,
                                                     [FromQuery] string market,
                                                     [FromQuery] int limit = 25)
        {
            var searchResults = await _searchService.SearchEntities(q, market, limit);
            return Ok(searchResults.ToList());
        }

    }
}
