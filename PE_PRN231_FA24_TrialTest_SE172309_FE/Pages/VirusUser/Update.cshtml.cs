using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PE_PRN231_FA24_TrialTest_SE172309_FE.Models;

namespace PE_PRN231_FA24_TrialTest_SE172309_FE.Pages.VirusUser
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public PersonWithVirus UpdatePerson { get; set; } = new PersonWithVirus();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Kiểm tra quyền truy cập, tạm thời bỏ qua nếu bạn muốn kiểm tra
            var role = HttpContext.Session.GetString("Role");
            if (role != "doctor") return Forbid();

            var url = $"{Common.BaseURL}/api/persons/{id}";
            var response = await Common.SendGetRequest(url, HttpContext.Session.GetString("AccessToken"));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                UpdatePerson = JsonConvert.DeserializeObject<PersonWithVirus>(content) ?? new PersonWithVirus();
                return Page();
            }
            TempData["ErrorMessage"] = "Person not found.";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var url = $"{Common.BaseURL}/api/persons/{UpdatePerson.PersonId}";
            var response = await Common.SendRequestWithBody(UpdatePerson, url, HttpContext.Session.GetString("AccessToken"), "Put");

            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            TempData["ErrorMessage"] = "An error occurred while updating the user. Please try again.";
            return Page();
        }
    }
}
