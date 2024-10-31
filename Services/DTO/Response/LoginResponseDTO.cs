
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.Response
{
    public class LoginResponseDTO
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public ViroCureUserDTO User { get; set; } 
    }
}
