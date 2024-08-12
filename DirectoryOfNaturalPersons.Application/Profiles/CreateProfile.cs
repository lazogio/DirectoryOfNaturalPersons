using AutoMapper;
using DirectoryOfNaturalPersons.Application.Models;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;
using DirectoryOfNaturalPersons.Domain.Entities;

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

        CreateMap<PhoneNumberModel, PhoneNumberDTO>();
        CreateMap<PhoneNumberDTO, PhoneNumberModel>();

    }
}