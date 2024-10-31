using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Response
{
    public class PersonDTO
    {
        public int PersonId { get; set; }

        public string Fullname { get; set; } = null!;

        public DateOnly BirthDay { get; set; }

        public string Phone { get; set; } = null!;
        public List<PersonVirusDTO> Viruses { get; set; }
    }
}
