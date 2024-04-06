using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectoryMVC.Controllers
{
    public class CityController : Controller
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

        public async Task<ActionResult<CityViewModel>> CreateCity(CityViewModel cityViewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(cityViewModel.Name)) return View(new CityViewModel { Localizer = _localizer });

                var cityDTO = _mapper.Map<City>(cityViewModel);
                var createdCity = await _cityService.CreateCity(cityDTO);

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                return View(new CityViewModel { Localizer = _localizer, ErrorMessage = _localizer["CityNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new CityViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<ActionResult<CityViewModel>> UpdateCity(int id, CityViewModel cityViewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(cityViewModel.Name))
                {
                    var city = _mapper.Map<CityViewModel>(await _cityService.GetCity(id));
                    city.Localizer = _localizer;

                    return View(city);
                }

                var cityDTO = _mapper.Map<City>(cityViewModel);
                var createdCity = await _cityService.UpdateCity(cityDTO);

                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                return View(new CityViewModel { Localizer = _localizer, ErrorMessage = _localizer["CityNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new CityViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }

        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                await _cityService.DeleteCity(id);
                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer["CityNotFoundMessage"].Value });
            }
            catch (Exception ex)
            {
                return View(new HomeViewModel { Localizer = _localizer, ErrorMessage = _localizer[ex.Message].Value });
            }
        }
    }
}
