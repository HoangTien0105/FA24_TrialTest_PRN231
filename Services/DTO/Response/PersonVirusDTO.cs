using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Response
{
    public class PersonVirusDTO
    {
        public string VirusName { get; set; } = null!;
        public double? ResistanceRate { get; set; }
    }
}
