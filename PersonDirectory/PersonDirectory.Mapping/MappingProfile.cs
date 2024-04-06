using AutoMapper;
using PersonDirectory.DTO;
using PersonDirectory.Model;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectory.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<Person, PersonModel>().ReverseMap();
            CreateMap<PhoneNumber, PhoneNumberModel>().ReverseMap();
            CreateMap<RelatedPersons, RelatedPersonModel>().ReverseMap();

            CreateMap<City, CityViewModel>().ReverseMap();
            CreateMap<Person, PersonViewModel>().ReverseMap();
            CreateMap<Person, SearchParametersViewModel>().ReverseMap();
            CreateMap<SearchParametersModel, SearchParametersViewModel>().ReverseMap();
            CreateMap<PhoneNumber, PhoneNumberViewModel>().ReverseMap();
            CreateMap<RelatedPersons, RelatedPersonViewModel>().ReverseMap();
        }
    }
}
