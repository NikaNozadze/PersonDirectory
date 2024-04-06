using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectoryMVC.Controllers
{
    public class PhoneNumberController : Controller
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IPersonService _personService;

        public PhoneNumberController(IPhoneNumberService phoneNumberService, IPersonService personService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _phoneNumberService = phoneNumberService ?? throw new ArgumentNullException(nameof(phoneNumberService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        public async Task<ActionResult<PhoneNumberViewModel>> CreatePhoneNumber(PhoneNumberViewModel phoneNumberViewModel)
        {
            var persons = (await _personService.GetAllPersons())
                .Select(person => _mapper.Map<PersonViewModel>(person))
                .ToList();

            try
            {
                if (string.IsNullOrEmpty(phoneNumberViewModel.Number)) return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons });

                var phoneNumberDTO = _mapper.Map<PhoneNumber>(phoneNumberViewModel);
                var createdPhoneNumber = await _phoneNumberService.CreatePhoneNumber(phoneNumberDTO);

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["PhoneNumberNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<ActionResult<PhoneNumberViewModel>> UpdatePhoneNumber(int id, PhoneNumberViewModel phoneNumberViewModel)
        {
            var persons = (await _personService.GetAllPersons())
                .Select(person => _mapper.Map<PersonViewModel>(person))
                .ToList();

            try
            {
                if (string.IsNullOrEmpty(phoneNumberViewModel.Number))
                {
                    var phoneNumber = await _phoneNumberService.GetPhoneNumber(id);
                    if (phoneNumber == null)
                    {
                        return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["PhoneNumberNotFoundMessage"].Value });
                    }

                    phoneNumberViewModel = _mapper.Map<PhoneNumberViewModel>(phoneNumber);
                    phoneNumberViewModel.Persons = persons;
                    phoneNumberViewModel.Localizer = _localizer;

                    return View(phoneNumberViewModel);
                }
                else
                {
                    var phoneNumberDTO = _mapper.Map<PhoneNumber>(phoneNumberViewModel);
                    var createdPhoneNumber = await _phoneNumberService.UpdatePhoneNumber(phoneNumberDTO);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer["PhoneNumberNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new PhoneNumberViewModel { Localizer = _localizer, Persons = persons, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<IActionResult> DeletePhoneNumber(int id)
        {
            try
            {
                await _phoneNumberService.DeletePhoneNumber(id);
                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["PhoneNumberNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }
    }
}
