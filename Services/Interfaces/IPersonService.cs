using Services.DTO.Request;
using Services.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPersonService
    {
        Task<List<PersonDTO>> GetAllPersons();
        Task<PersonDTO> GetPersonById(int id);
        Task Create(CreatePersonWithVirusDTO createPersonWithVirusDTO);
        Task Update(int id, UpdatePersonWithVirusDTO updatePersonWithVirusDTO);
        Task Delete(int id);
    }
}
