using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CityController(ICityService cityService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpPost("create")]
        public async Task<ActionResult<CityModel>> CreateCity(CityModel cityModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => _localizer[e.ErrorMessage].Value)
                                                  .ToList();

                    return BadRequest(errors);
                }
                var cityDTO = _mapper.Map<City>(cityModel);
                var createdCity = await _cityService.CreateCity(cityDTO);

                return Ok(createdCity);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["CityNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityModel cityModel)
        {
            if (id != cityModel.Id)
            {
                return BadRequest();
            }
            var cityDTO = _mapper.Map<City>(cityModel);
            try
            {
                await _cityService.UpdateCity(cityDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(_localizer["CityNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                await _cityService.DeleteCity(id);
                return Ok(_localizer["CityDeletedSuccessfully"].Value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(_localizer["CityNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
