using Repositories.Interfaces;
using Repositories.Models;
using Repositories.Repositories;
using Services.DTO.Request;
using Services.DTO.Response;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonVirusRepository _personVirusRepository;
        private readonly IVirusRepository _virusRepository;

        public PersonService(IPersonRepository personRepository, IPersonVirusRepository personVirusRepository, IVirusRepository virusRepository)
        {
            _personRepository = personRepository;
            _personVirusRepository = personVirusRepository;
            _virusRepository = virusRepository;
        }

        public async Task Create(CreatePersonWithVirusDTO createPersonWithVirusDTO)
        {
            try
            {
                if (createPersonWithVirusDTO.BirthDay >= new DateTime(2007, 1, 1))
                {
                    throw new Exception("Value for Birthday < 01-01-2007");
                }

                foreach (var virus in createPersonWithVirusDTO.Viruses)
                {
                    var virusExist = _virusRepository.GetAll(filter: x => x.VirusName == virus.VirusName);
                    if (!virusExist.Any())
                    {
                        throw new Exception("Invalid virus");
                    }
                }

                Person person = new Person
                {
                    PersonId = createPersonWithVirusDTO.PersonID,
                    Fullname = createPersonWithVirusDTO.Fullname,
                    BirthDay = DateOnly.FromDateTime(createPersonWithVirusDTO.BirthDay),
                    Phone = createPersonWithVirusDTO.Phone
                };

                _personRepository.Add(person);

                foreach (var personVirus in createPersonWithVirusDTO.Viruses)
                {
                    var virusExist = _virusRepository.GetAll(filter: x => x.VirusName == personVirus.VirusName).FirstOrDefault();

                    PersonVirus personVirusDTO = new PersonVirus
                    {
                        PersonId = person.PersonId,
                        VirusId = virusExist.VirusId,
                        ResistanceRate = personVirus.ResistanceRate
                    };

                    _personVirusRepository.Add(personVirusDTO);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PersonDTO>> GetAllPersons()
        {
            var personList = _personRepository.GetAll(includeProperties: "PersonViruses.Virus");

            List<PersonDTO> response = new List<PersonDTO>();

            foreach (var person in personList)
            {
                PersonDTO personDTO = new PersonDTO
                {
                    PersonId = person.PersonId,
                    Fullname = person.Fullname,
                    BirthDay = person.BirthDay,
                    Phone = person.Phone,
                    Viruses = person.PersonViruses.Select(v => new PersonVirusDTO
                    {
                        VirusName = v.Virus.VirusName,
                        ResistanceRate = v.ResistanceRate,
                    }).ToList()
                };

                response.Add(personDTO);
            }

            return response;
        }

        public async Task<PersonDTO> GetPersonById(int id)
        {
            var person = _personRepository.GetAll(filter: x => x.UserId == id, includeProperties: "PersonViruses.Virus").FirstOrDefault();

            PersonDTO personDTO = new PersonDTO
            {
                PersonId = person.PersonId,
                Fullname = person.Fullname,
                BirthDay = person.BirthDay,
                Phone = person.Phone,
                Viruses = person.PersonViruses.Select(v => new PersonVirusDTO
                {
                    VirusName = v.Virus.VirusName,
                    ResistanceRate = v.ResistanceRate,
                }).ToList()
            };

            return personDTO;
        }

        public async Task Update(int id, UpdatePersonWithVirusDTO updatePersonWithVirusDTO)
        {
            try
            {
                var userExist = _personRepository.GetAll(filter: x => x.UserId == id).FirstOrDefault();
                if (userExist == null)
                {
                    throw new Exception("User not found!");
                }

                if (updatePersonWithVirusDTO.BirthDay >= new DateTime(2007, 1, 1))
                {
                    throw new Exception("Value for Birthday < 01-01-2007");
                }

                foreach (var virus in updatePersonWithVirusDTO.Viruses)
                {
                    var virusExist = _virusRepository.GetAll(filter: x => x.VirusName == virus.VirusName);
                    if (!virusExist.Any())
                    {
                        throw new Exception("Invalid virus");
                    }
                }

                var person = _personRepository.GetAll(filter: x => x.PersonId == id).FirstOrDefault();

                if (person == null)
                {
                    throw new Exception("Person not found");
                }

                person.Fullname = updatePersonWithVirusDTO.Fullname;
                person.BirthDay = DateOnly.FromDateTime(updatePersonWithVirusDTO.BirthDay);
                person.Phone = updatePersonWithVirusDTO.Phone;

                _personRepository.Update(person);

                var existingPersonViruses = _personVirusRepository.GetAll(filter: x => x.PersonId == id, includeProperties: "Virus");

                // Delete existing viruses not present in the updated list
                foreach (var existingPersonVirus in existingPersonViruses)
                {
                    if (!updatePersonWithVirusDTO.Viruses.Any(v => v.VirusName == existingPersonVirus.Virus.VirusName))
                    {
                        _personVirusRepository.Delete(existingPersonVirus);
                    }
                }

                // Add or update viruses in the updated list
                foreach (var personVirus in updatePersonWithVirusDTO.Viruses)
                {
                    var virusExist = _virusRepository.GetAll(filter: x => x.VirusName == personVirus.VirusName).FirstOrDefault();

                    var existingPersonVirus = existingPersonViruses.FirstOrDefault(v => v.Virus.VirusName == personVirus.VirusName);

                    if (existingPersonVirus != null)
                    {
                        existingPersonVirus.ResistanceRate = personVirus.ResistanceRate;
                        _personVirusRepository.Update(existingPersonVirus);
                    }
                    else
                    {
                        // Add new virus for the person
                        PersonVirus personVirusDTO = new PersonVirus
                        {
                            PersonId = id,
                            VirusId = virusExist.VirusId,
                            ResistanceRate = personVirus.ResistanceRate
                        };
                        _personVirusRepository.Add(personVirusDTO);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
