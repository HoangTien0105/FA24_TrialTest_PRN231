using Services.DTO;
using Services.DTO.Request;
using Services.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVireCureUserService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest, JWT Key);
    }
}
