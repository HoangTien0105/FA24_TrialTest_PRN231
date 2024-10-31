using Services.DTO.Response;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Request
{
    public class CreatePersonWithVirusDTO
    {
        [Required]
        public int PersonID { get; set; }

        [Required(ErrorMessage = "Fullname is required.")]
        [RegularExpression(@"^([A-Z][a-zA-Z0-9@# ]+\s?)+$", ErrorMessage = "Each word of the Fullname must begin with the capital letter.")]
        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = "BirthDay is required.")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\+8498\d{7}$", ErrorMessage = "Phone number must be in the format +84989xxxxxx.")]
        public string Phone { get; set; } = null!;

        [Required]
        public List<PersonVirusRequestDTO> Viruses { get; set; } = new List<PersonVirusRequestDTO>();
    }

    public class PersonVirusRequestDTO
    {
        [Required(ErrorMessage = "VirusName is required.")]
        public string VirusName { get; set; } = null!;

        [Range(0, 1, ErrorMessage = "Resistance Rate must be between 0 and 1.")]
        public double? ResistanceRate { get; set; }
    }
}
