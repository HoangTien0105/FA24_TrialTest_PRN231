using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.DTO;
using Services.DTO.Request;
using Services.Interfaces;

namespace PE_PRN231_FA24_TrialTest_VanHoangTien_BE.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IVireCureUserService _vireCoreUserService;
        private readonly IOptions<JWT> _jwt;

        public LoginController(IVireCureUserService vireCoreUserService, IOptions<JWT> jwt)
        {
            _vireCoreUserService = vireCoreUserService;
            _jwt = jwt;
        }
        [HttpPost()]
        public IActionResult Login([FromBody] LoginRequestDTO account)
        {
            if (account == null || string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            try
            {
                var result = _vireCoreUserService.Login(account, _jwt.Value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
