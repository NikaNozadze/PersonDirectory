using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectoryMVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IRelatedPersonService _relatedPersonService;

        public PersonController(IPersonService personService, ICityService cityService, IMapper mapper, IWebHostEnvironment webHostEnvironment,
                                IRelatedPersonService relatedPersonService, IPhoneNumberService phoneNumberService, IStringLocalizer<SharedResources> localizer)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _phoneNumberService = phoneNumberService ?? throw new ArgumentNullException(nameof(phoneNumberService));
            _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
        }

        public async Task<ActionResult<PersonViewModel>> CreatePerson(PersonViewModel personViewModel, IFormFile imageFile)
        {
            var cities = await GetCityViewModels();

            try
            {
                if (string.IsNullOrEmpty(personViewModel.FirstName))
                {
                    return View(new PersonViewModel { Localizer = _localizer, CityModels = cities });
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = await SaveImageFile(imageFile);
                    personViewModel.ImagePath = fileName;
                }

                var personDTO = MapPersonViewModelToDTO(personViewModel, cities);
                var createdPerson = await _personService.CreatePerson(personDTO);

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                return View(new PersonViewModel { Localizer = _localizer, CityModels = cities, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new PersonViewModel { Localizer = _localizer, CityModels = cities, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<ActionResult<PersonViewModel>> UpdatePerson(int id, PersonViewModel personViewModel, IFormFile imageFile)
        {
            var cities = await GetCityViewModels();

            try
            {
                if (string.IsNullOrEmpty(personViewModel.FirstName))
                {
                    var person = await _personService.GetPerson(id);
                    if (person == null)
                    {
                        return View(new PersonViewModel { Localizer = _localizer, CityModels = cities, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
                    }

                    personViewModel = _mapper.Map<PersonViewModel>(person);
                    personViewModel.CityModels = cities;
                    personViewModel.Localizer = _localizer;

                    return View(personViewModel);
                }
                else
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = await SaveImageFile(imageFile);
                        personViewModel.ImagePath = fileName;
                    }

                    var personDTO = MapPersonViewModelToDTO(personViewModel, cities);
                    var createdPerson = await _personService.UpdatePerson(personDTO);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return View(new PersonViewModel { CityModels = cities, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new PersonViewModel { CityModels = cities, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<ActionResult<HomeViewModel>> GetPersonsPhoneNumbers(int Id)
        {
            try
            {
                var phoneNumbers = (await _phoneNumberService.GetPersonsAllPhoneNumbers(Id))
                    .Select(phoneNumber => _mapper.Map<PhoneNumberViewModel>(phoneNumber))
                    .ToList();

                return View(new HomeViewModel { Localizer = _localizer, SearchedPhoneNumbers = phoneNumbers });
            }
            catch (KeyNotFoundException ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<ActionResult<HomeViewModel>> GetPersonsRelatedPersons(int Id)
        {
            try
            {
                var relatedPersons = (await _relatedPersonService.GetPersonsAllRelatedPerson(Id))
                    .Select(relatedPerson => _mapper.Map<RelatedPersonViewModel>(relatedPerson))
                    .ToList();

                return View(new HomeViewModel { Localizer = _localizer, SearchedRelatedPersons = relatedPersons });
            }
            catch (KeyNotFoundException ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var person = await _personService.GetPerson(id);
                if (person != null)
                {
                    if (!string.IsNullOrEmpty(person.ImagePath))
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", person.ImagePath);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    await _personService.DeletePerson(person);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["PersonNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        private async Task<IEnumerable<CityViewModel>> GetCityViewModels()
        {
            return (await _cityService.GetAllCities())
                    .Select(city => _mapper.Map<CityViewModel>(city))
                    .ToList();
        }

        private async Task<string> SaveImageFile(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return fileName;
        }

        private Person MapPersonViewModelToDTO(PersonViewModel personViewModel, IEnumerable<CityViewModel> cities)
        {
            var personDTO = _mapper.Map<Person>(personViewModel);
            if (personViewModel.City == null) personDTO.City = null;
            personDTO.CityId = cities.FirstOrDefault(city => city.Name == personViewModel.CityName)?.Id
                                            ?? throw new ArgumentException(_localizer["CityNotFoundMessage"].Value);

            return personDTO;
        }
    }
}
