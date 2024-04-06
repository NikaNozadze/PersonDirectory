using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;
using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;

        public PersonController(IPersonService personService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpPost("create")]
        public async Task<ActionResult<PersonModel>> CreatePerson(PersonModel personModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => _localizer[e.ErrorMessage].Value)
                                                  .ToList();

                    throw new ValidationException(string.Join(Environment.NewLine, errors));
                }
                var personDTO = _mapper.Map<Person>(personModel);
                var createdPerson = await _personService.CreatePerson(personDTO);

                return Ok(createdPerson);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["PersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        public IActionResult CreatePerson()
        {
            return View();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonModel personModel)
        {
            if (id != personModel.Id)
            {
                return BadRequest();
            }
            var personDTO = _mapper.Map<Person>(personModel);
            try
            {
                await _personService.UpdatePerson(personDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(_localizer["PersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                await _personService.DeletePerson(await _personService.GetPerson(id));
                return Ok(_localizer["PersonDeletedSuccessfully"].Value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(_localizer["PersonNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
