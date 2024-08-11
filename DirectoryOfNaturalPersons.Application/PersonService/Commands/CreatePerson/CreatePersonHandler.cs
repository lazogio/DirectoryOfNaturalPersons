using AutoMapper;
using DirectoryOfNaturalPersons.Application.Models;
using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Interface;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonModel>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonHandler(IPersonRepository personRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = personRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<PersonDTO>(request);
        await _repository.InsertAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<PersonModel>(person);
    }
}