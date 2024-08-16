using AutoMapper;
using DirectoryOfNaturalPersons.Domain.GenericModel;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPerson;

public class GetPersonsSearchQueryHandler : IRequestHandler<GetPersonsSearchQuery, PagedResult<PersonModel>>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public GetPersonsSearchQueryHandler(IPersonRepository repository, IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PagedResult<PersonModel>> Handle(GetPersonsSearchQuery searchCriteria,
        CancellationToken cancellationToken)
    {
        var sourcePagedResult = await _repository.SearchPersonsAsync(
            searchCriteria.QuickSearch,
            searchCriteria.FirstName,
            searchCriteria.LastName,
            searchCriteria.PersonalId,
            searchCriteria.Page,
            searchCriteria.PageSize,
            cancellationToken);
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        var pagedResultPersonModel = new PagedResult<PersonModel>
        {
            TotalCount = sourcePagedResult.PageSize,
            Page = sourcePagedResult.Page,
            PageSize = sourcePagedResult.PageSize,
            Results = sourcePagedResult.Results.Select(x =>
            {
                var city = _repository.GerCityIdAsync(x.CityId, cancellationToken);
                var personModel = _mapper.Map<PersonModel>(x);
                personModel.City = headers!["Accept-Language"].Contains("ka-GE")
                    ? city.Result!.NameKa : city.Result!.NameEn;

                return personModel;
            }).ToList()
        };

        return pagedResultPersonModel;
    }
}