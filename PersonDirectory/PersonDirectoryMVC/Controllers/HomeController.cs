using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectoryMVC.Controllers
{
    public class HomeController : Controller
    {
        private string? _currentCulture;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IPersonDetailsService _personDetailsService;
        private readonly ISearchService _searchService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public HomeController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ICityService cityService,
                              IPersonDetailsService personDetailsService, ISearchService searchService, IStringLocalizer<SharedResources> localizer)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _personDetailsService = personDetailsService ?? throw new ArgumentNullException(nameof(personDetailsService));
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public ActionResult<HomeViewModel> Index(HomeViewModel homeViewModel)
        {
            homeViewModel.Localizer = _localizer;
            if (string.IsNullOrEmpty(homeViewModel.CultureName))
            {
                var currentCulture = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? "en-US";
                if (currentCulture == "en-US") ChangeCultureToEN();
                _currentCulture = currentCulture;
                homeViewModel.CurrentCulture = _currentCulture;

                return View(homeViewModel);
            }

            try
            {
                if (homeViewModel.CultureName == "en-US")
                {
                    _currentCulture = homeViewModel.CultureName;
                    ChangeCultureToEN();
                }
                else if (homeViewModel.CultureName == "ka-GE")
                {
                    _currentCulture = homeViewModel.CultureName;
                    ChangeCultureToGE();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                homeViewModel.ErrorMessage = ex.Message;

                return View(homeViewModel);
            }
        }

        public async Task<ActionResult<HomeViewModel>> Search(HomeViewModel homeViewModel)
        {
            try
            {
                homeViewModel.Localizer = _localizer;
                if (string.IsNullOrEmpty(homeViewModel.SearchTerm)) return View(homeViewModel);

                switch (homeViewModel.SearchType)
                {
                    case SearchType.Person:
                        return View(await SearchPersons(homeViewModel));
                    case SearchType.City:
                        return View(await SearchCities(homeViewModel));
                    case SearchType.PhoneNumber:
                        return View(await SearchPhoneNumbers(homeViewModel));
                    default:
                        homeViewModel.ErrorMessage = _localizer["Invalid search type"].Value;
                        homeViewModel.Localizer = _localizer;

                        return View(homeViewModel);
                }
            }
            catch (Exception ex)
            {
                homeViewModel.ErrorMessage = _localizer[ex.Message].Value;
                homeViewModel.Localizer = _localizer;

                return View(homeViewModel);
            }
        }


        public async Task<ActionResult<SearchParametersViewModel>> DetailedSearch(SearchParametersViewModel parameters)
        {
            parameters.Localizer = _localizer;
            try
            {
                if (IsNullOrEmptyAllFields(parameters)) return View(await GetCityViewModels(parameters));

                var searchedPersonViewModels = (await DetailedSearchPersonsAsync(_mapper.Map<SearchParametersModel>(parameters)))
                                                     .Select(person =>
                                                     {
                                                         var personViewModel = _mapper.Map<PersonViewModel>(person);
                                                         personViewModel.Localizer = _localizer;
                                                         return personViewModel;
                                                     }).ToList();

                parameters.Persons = searchedPersonViewModels;

                return View("DetailedSearchResult", parameters.Persons);
            }
            catch (Exception ex)
            {
                parameters.ErrorMessage = _localizer[ex.Message].Value;
                parameters.Localizer = _localizer;
                parameters = await GetCityViewModels(parameters);

                return View(parameters);
            }
        }

        public ActionResult<IEnumerable<PersonViewModel>> DetailedSearchResult(IEnumerable<PersonViewModel> persons) => View(persons);

        private async Task<HomeViewModel> SearchPersons(HomeViewModel homeViewModel)
        {
            var searchedPersons = (await SearchPersonsAsync(homeViewModel.SearchTerm))
                                                        .Select(person => _mapper.Map<PersonViewModel>(person))
                                                        .ToList();
            homeViewModel.SearchedPersons = searchedPersons;
            homeViewModel.Localizer = _localizer;

            return homeViewModel;
        }

        private async Task<HomeViewModel> SearchCities(HomeViewModel homeViewModel)
        {
            var searchedCities = (await SearchCitiesAsync(homeViewModel.SearchTerm))
                                                        .Select(city => _mapper.Map<CityViewModel>(city))
                                                        .ToList();
            homeViewModel.SearchedCities = searchedCities;
            homeViewModel.Localizer = _localizer;

            return homeViewModel;
        }

        private async Task<HomeViewModel> SearchPhoneNumbers(HomeViewModel homeViewModel)
        {
            var searchedPhoneNumbers = (await SearchPhoneNumbersAsync(homeViewModel.SearchTerm))
                                                        .Select(phoneNumber => _mapper.Map<PhoneNumberViewModel>(phoneNumber))
                                                        .ToList();
            homeViewModel.SearchedPhoneNumbers = searchedPhoneNumbers;
            homeViewModel.Localizer = _localizer;

            return homeViewModel;
        }

        private void ChangeCultureToEN()
        {
            if (!string.IsNullOrEmpty("en-US"))
            {
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US")),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
        }

        private void ChangeCultureToGE()
        {
            if (!string.IsNullOrEmpty("ka-GE"))
            {
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("ka-GE")),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
        }

        private async Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm)
        {
            return HandleSearchException(await _personDetailsService.GetPersonDetailsWithSTermAsync(searchTerm), "Person");
        }

        private async Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm)
        {
            return HandleSearchException(await _searchService.GetCityWithSTermAsync(searchTerm), "City");
        }

        private async Task<IEnumerable<PhoneNumber>> SearchPhoneNumbersAsync(string searchTerm)
        {
            return HandleSearchException(await _searchService.GetPhoneNumbersWithSTermAsync(searchTerm), "PhoneNumber");
        }

        private async Task<IEnumerable<Person>> DetailedSearchPersonsAsync(SearchParametersModel parameters)
        {
            return HandleSearchException(await _personDetailsService.GetPersonDetailsWithDetailedSearchAsync(parameters), "Person");
        }

        private IEnumerable<T> HandleSearchException<T>(IEnumerable<T> collection, string exceptionName)
        {
            try
            {
                if (!collection.Any())
                {
                    throw new ArgumentException(_localizer[$"{exceptionName}NotFoundMessage"].Value);
                }

                return collection;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(_localizer[ex.Message].Value);
            }
        }

        private bool IsNullOrEmptyAllFields(SearchParametersViewModel parameters)
        {
            return parameters.Id == null && string.IsNullOrEmpty(parameters.FirstName) && string.IsNullOrEmpty(parameters.LastName)
                                         && parameters.Gender == null && string.IsNullOrEmpty(parameters.PersonalNumber)
                                         && parameters.DateOfBirth == null && parameters.CityId == null;
        }

        private async Task<SearchParametersViewModel> GetCityViewModels(SearchParametersViewModel parameters)
        {
            var cities = (await _cityService.GetAllCities())
                        .Select(city => _mapper.Map<CityViewModel>(city))
                        .ToList();
            parameters.CityModels = cities;

            return parameters;
        }
    }
}
