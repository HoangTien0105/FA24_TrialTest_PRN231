using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PE_PRN231_FA24_TrialTest_SE172309_FE.Models;

namespace PE_PRN231_FA24_TrialTest_SE172309_FE.Pages.VirusUser
{
    public class IndexModel : PageModel 
    { 
        public IList<PersonWithVirus> Persons { get; set; } = new List<PersonWithVirus>();
        public async Task<IActionResult> OnGetAsync()
        {
            await Init();

            return Page();
        }
        private async Task Init()
        {
            var url = $"{Common.BaseURL}/api/persons";
            var response = await Common.SendGetRequest(url, HttpContext.Session.GetString("AccessToken"));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                this.Persons = JsonConvert.DeserializeObject<List<PersonWithVirus>>(content) ?? new List<PersonWithVirus>();
            }
            else
            {
                Persons = new List<PersonWithVirus>();
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "doctor") return Forbid();

            var url = $"{Common.BaseURL}/api/persons/{id}";
            var response = await Common.SendRequestWithBody<object>(null, url, HttpContext.Session.GetString("AccessToken"), "Delete");

            if (response != null && response.IsSuccessStatusCode)
            {
                await Init();
            }

            TempData["ErrorMessage"] = "An error occurred while deleting the user. Please try again.";
            return RedirectToPage();
        }

    }
}
