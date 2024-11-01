using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.Request;
using Services.Interfaces;

namespace PE_PRN231_FA24_TrialTest_VanHoangTien_BE.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPersons()
        {
            var results = await _personService.GetAllPersons();

            return Ok(results);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var results = await _personService.GetPersonById(id);

            return Ok(results);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonWithVirusDTO person)
        {
            try
            {
                await _personService.Update(id, person);

                var response = new
                {
                    message = "Person and viruses updated successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> Create([FromBody] CreatePersonWithVirusDTO person)
        {
            try
            {
                await _personService.Create(person);

                var reponse = new
                {
                    personId = person.PersonID,
                    message = "Person and viruses added successfully"
                };

                return StatusCode(201, reponse);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            try
                {
                await _personService.Delete(id);

                var response = new
                {
                    message = "Person and viruses deleted successfully"
                };

                return Ok(response);
            }

            catch (Exception ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
        }
    }
}
