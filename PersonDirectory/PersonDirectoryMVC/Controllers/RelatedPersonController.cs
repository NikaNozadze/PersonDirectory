using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectoryMVC.Controllers
{
    public class RelatedPersonController : Controller
    {
        private readonly IRelatedPersonService _relatedPersonService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IPersonService _personService;

        public RelatedPersonController(IRelatedPersonService relatedPersonService, IPersonService personService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        public async Task<ActionResult<RelatedPersonViewModel>> CreateRelatedPerson(RelatedPersonViewModel relatedPersonViewModel)
        {
            var persons = (await _personService.GetAllPersons())
                .Select(person => _mapper.Map<PersonViewModel>(person))
                .ToList();

            if (relatedPersonViewModel.PersonId == 0) return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons });

            try
            {
                var relatedPersonDTO = _mapper.Map<RelatedPersons>(relatedPersonViewModel);
                var createdRelatedPerson = await _relatedPersonService.CreateRelatedPerson(relatedPersonDTO);

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["RelatedPersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<IActionResult> UpdateRelatedPerson(int id, RelatedPersonViewModel relatedPersonViewModel)
        {
            var persons = (await _personService.GetAllPersons())
                .Select(person => _mapper.Map<PersonViewModel>(person))
                .ToList();

            try
            {
                if (relatedPersonViewModel.PersonId == 0)
                {
                    var relatedPerson = await _relatedPersonService.GetRelatedPerson(id);
                    if (relatedPerson == null)
                    {
                        return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["RelatedPersonNotFoundMessage"].Value });
                    }

                    relatedPersonViewModel = _mapper.Map<RelatedPersonViewModel>(relatedPerson);
                    relatedPersonViewModel.Persons = persons;
                    relatedPersonViewModel.Localizer = _localizer;

                    return View(relatedPersonViewModel);
                }
                else
                {
                    var relatedPersonDTO = _mapper.Map<RelatedPersons>(relatedPersonViewModel);
                    var createdPhoneNumber = await _relatedPersonService.UpdateRelatedPerson(relatedPersonDTO);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["RelatedPersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new RelatedPersonViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<IActionResult> DeleteRelatedPerson(int id)
        {
            try
            {
                await _relatedPersonService.DeleteRelatedPerson(id);
                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["RelatedPersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }
    }
}
