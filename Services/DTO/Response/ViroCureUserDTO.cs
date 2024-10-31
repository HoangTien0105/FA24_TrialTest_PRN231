using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Response
{
    public class ViroCureUserDTO
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; }
    }
}
