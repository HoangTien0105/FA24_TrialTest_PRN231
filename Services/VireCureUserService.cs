using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.DTO;
using Services.DTO.Request;
using Services.DTO.Response;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VireCureUserService : IVireCureUserService
    {
        private readonly IVireCureUserRepository _vireCoreUserRepository;

        public VireCureUserService(IVireCureUserRepository vireCoreUserRepository)
        {
            _vireCoreUserRepository = vireCoreUserRepository;
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest, JWT Key)
        {
            try
            {
                var user = _vireCoreUserRepository.GetAll().Where(e => e.Email == loginRequest.Email && e.Password == loginRequest.Password).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("Invalid email or password");
                }

                var role = GetRoleString(user.Role);

                ViroCureUserDTO userDTO = new ViroCureUserDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = role,
                };

                return GenerateToken(userDTO, Key.Key);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<LoginResponseDTO> GenerateToken(ViroCureUserDTO account, string keyAuth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyAuth));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, account.UserId.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);

            LoginResponseDTO login = new LoginResponseDTO
            {
                Message = "Login successful",
                Token =  accessToken,
                User = account
            };

            return login;
        }

        private string GetRoleString(int role)
        {
            if (role == 1)
            {
                return "admin";
            }
            else if (role == 2)
            {
                return "patients";
            }
            else if (role == 3)
            {
                return "doctor";
            }
            else
            {
                return "unknown";
            }
        }

    }
}
