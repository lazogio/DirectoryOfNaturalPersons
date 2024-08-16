using AutoMapper;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;
using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Models;

namespace DirectoryOfNaturalPersons.Application.Profiles;

public class CreateProfile : Profile
{
    public CreateProfile()
    {
        CreateMap<PersonDTO, CreatePersonCommand>()
            .ForMember(dest => dest.PhoneNumbers,
                opt => opt.MapFrom(src => src.PhoneNumbers));
        CreateMap<CreatePersonCommand, PersonDTO>()
            .ForMember(dest => dest.PhoneNumbers,
                opt => opt.MapFrom(src => src.PhoneNumbers));
        CreateMap<PersonDTO, PersonModel>();
        CreateMap<PersonModel, PersonDTO>();
        CreateMap<PersonDTO, PersonModel>()
            .ForMember(dest => dest.City, opt
                => opt.MapFrom(src => src.CityId));
        CreateMap<PhoneNumberModel, PhoneNumberDTO>();
        CreateMap<PhoneNumberDTO, PhoneNumberModel>();
        CreateMap<PersonDTO, PersonDetailedModel>();
        CreateMap<PersonDetailedModel, PersonDTO>();
        CreateMap<PersonRelationDTO, RelatedPersonsModel>()
            .ForMember(dest => dest.RelatedPersonId, opt => opt.MapFrom(src => src.RelatedPersonId))
            .ForMember(dest => dest.RelationType, opt => opt.MapFrom(src => src.RelationType));
    }
}