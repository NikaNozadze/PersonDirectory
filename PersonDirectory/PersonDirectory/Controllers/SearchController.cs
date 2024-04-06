using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SearchController(ISearchService searchService, IStringLocalizer<SharedResources> localizer)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpGet("quickSearch")]
        public async Task<ActionResult<IEnumerable<Person>>> SearchPersonsAsync([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm)) throw new ArgumentException(_localizer["SearchTermRequired"].Value);

                var results = await _searchService.SearchPersonsAsync(searchTerm);

                if (results.Any())
                {
                    return Ok(results);
                }
                else
                {
                    return NotFound(_localizer["PersonNotFoundMessage"].Value);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpGet("detailedSearch")]
        public async Task<ActionResult<IEnumerable<Person>>> DetailedSearchPersonsAsync([FromQuery] SearchParametersModel parameters)
        {
            try
            {
                var results = await _searchService.DetailedSearchPersonsAsync(parameters);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
