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
    public class RelatedPersonController : ControllerBase
    {
        private readonly IRelatedPersonService _relatedPersonService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RelatedPersonController(IRelatedPersonService relatedPersonService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpPost("create")]
        public async Task<ActionResult<RelatedPersonModel>> CreateRelatedPerson(RelatedPersonModel relatedPersonModel)
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
                var relatedPersonDTO = _mapper.Map<RelatedPersons>(relatedPersonModel);
                var createdRelatedPerson = await _relatedPersonService.CreateRelatedPerson(relatedPersonDTO);

                return Ok(createdRelatedPerson);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["RelatedPersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRelatedPerson(int id, RelatedPersonModel relatedPersonModel)
        {
            if (id != relatedPersonModel.Id)
            {
                return BadRequest();
            }
            var relatedPersonDTO = _mapper.Map<RelatedPersons>(relatedPersonModel);
            try
            {
                await _relatedPersonService.UpdateRelatedPerson(relatedPersonDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["RelatedPersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRelatedPerson(int id)
        {
            try
            {
                await _relatedPersonService.DeleteRelatedPerson(id);
                return Ok(_localizer["RelatedPersonDeletedSuccessfully"].Value);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["RelatedPersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
