using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.API.Controllers
{
    public class PersonDetailsController : ControllerBase
    {
        private readonly IPersonDetailsService _personDetailsService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;

        public PersonDetailsController(IPersonDetailsService personDetailsService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _personDetailsService = personDetailsService ?? throw new ArgumentNullException(nameof(personDetailsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpGet("getPersonWithImage/{id}")]
        public async Task<ActionResult<PersonWithImageModel>> GetPersonWithImage(int id)
        {
            try
            {
                var personDTO = await _personDetailsService.GetPersonDetailsAsync(id);
                if (personDTO == null)
                {
                    return NotFound(_localizer["PersonNotFoundMessage"].Value);
                }

                var personWithImageModel = new PersonWithImageModel
                {
                    Person = _mapper.Map<PersonModel>(personDTO),
                    ImageUrl = !string.IsNullOrEmpty(personDTO.ImagePath) ? new PhysicalFileResult(personDTO.ImagePath, "image/jpeg") : null
                };

                return personWithImageModel;
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
