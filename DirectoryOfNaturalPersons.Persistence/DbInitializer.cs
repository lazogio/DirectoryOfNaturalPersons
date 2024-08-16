using System.Text.Json;
using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Enums;
using DirectoryOfNaturalPersons.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryOfNaturalPersons.Persistence
{
    public class DbInitializer
    {
        public async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);

            try
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
                await SeedPersonsAsync(dbContext, cancellationToken);
                await SeedCitiesAsync(dbContext, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"An error occurred during database seeding: {ex.Message}");
            }
        }

        private static async Task SeedCitiesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!await dbContext.Cities.AnyAsync(cancellationToken))
            {
                using var jsonStream = File.OpenRead("Cities.json");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var result = await JsonSerializer.DeserializeAsync<CityListModel>(jsonStream, options);

                if (result?.Cities != null)
                {
                    result.Cities.ForEach(x => x.SetCreateDate());
                    await dbContext.Cities.AddRangeAsync(result.Cities, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private static async Task SeedPersonsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            var persons = PersonsData();
            var existingIds = await dbContext.Persons.Select(p => p.PersonalId).ToListAsync(cancellationToken);
            var personsToAdd = persons.Where(p => !existingIds.Contains(p.PersonalId)).ToList();

            if (personsToAdd.Any())
            {
                await dbContext.Persons.AddRangeAsync(personsToAdd, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static IEnumerable<PersonDTO> PersonsData()
        {
            var persons = new List<PersonDTO>
            {
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "John",
                    LastName = "Smith",
                    PersonalId = "01987654321",
                    BirthDate = new DateTime(1985, 10, 15),
                    CityId = 2,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-123-4567",
                            NumberType = PhoneNumberType.Office
                        },
                        new PhoneNumberDTO
                        {
                            Number = "555-987-6543",
                            NumberType = PhoneNumberType.Mobile
                        }
                    }
                },
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PersonalId = "01987654322",
                    BirthDate = new DateTime(1990, 5, 25),
                    CityId = 1,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-555-5555",
                            NumberType = PhoneNumberType.Mobile
                        }
                    }
                },
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "Michael",
                    LastName = "Brown",
                    PersonalId = "01987354321",
                    BirthDate = new DateTime(1982, 8, 8),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-888-8888",
                            NumberType = PhoneNumberType.Mobile
                        }
                    }
                },
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "Emily",
                    LastName = "Davis",
                    PersonalId = "01987354328",
                    BirthDate = new DateTime(1995, 3, 20),
                    CityId = 1,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-999-9999",
                            NumberType = PhoneNumberType.Mobile
                        }
                    }
                },
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "David",
                    LastName = "Wilson",
                    PersonalId = "01987314328",
                    BirthDate = new DateTime(1988, 12, 10),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-777-7777",
                            NumberType = PhoneNumberType.Mobile
                        },
                        new PhoneNumberDTO
                        {
                            Number = "555-222-2222",
                            NumberType = PhoneNumberType.Mobile
                        }
                    }
                },
                new PersonDTO
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FirstName = "Jacob",
                    LastName = "Blue",
                    PersonalId = "01987328328",
                    BirthDate = new DateTime(1988, 12, 10),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumberDTO>
                    {
                        new PhoneNumberDTO
                        {
                            Number = "555-888-7777",
                            NumberType = PhoneNumberType.Mobile
                        },
                        new PhoneNumberDTO
                        {
                            Number = "555-222-2222",
                            NumberType = PhoneNumberType.Office
                        }
                    }
                }
            };

            var relations = new List<PersonRelationDTO>
            {
                new PersonRelationDTO
                {
                    PersonDto = persons[3],
                    RelatedPersonDto = persons[1],
                    RelationType = RelationType.Familiar
                },

                new PersonRelationDTO
                {
                    PersonDto = persons[1],
                    RelatedPersonDto = persons[3],
                    RelationType = RelationType.Colleague
                },
                new PersonRelationDTO
                {
                    PersonDto = persons[2],
                    RelatedPersonDto = persons[5],
                    RelationType = RelationType.Colleague
                },

                new PersonRelationDTO
                {
                    PersonDto = persons[2],
                    RelatedPersonDto = persons[4],
                    RelationType = RelationType.Colleague
                },

                new PersonRelationDTO
                {
                    PersonDto = persons[3],
                    RelatedPersonDto = persons[4],
                    RelationType = RelationType.Relative
                },

                new PersonRelationDTO
                {
                    PersonDto = persons[4],
                    RelatedPersonDto = persons[5],
                    RelationType = RelationType.Other
                }
            };

            foreach (var relation in relations)
            {
                var person = relation.PersonDto;
                var relatedPerson = relation.RelatedPersonDto;

                if (person.RelatedPersons.All(r => r.PersonDto.PersonalId != relatedPerson.PersonalId))
                {
                    person.RelatedPersons.Add(relation);
                }
            }

            return persons;
        }
    }
}