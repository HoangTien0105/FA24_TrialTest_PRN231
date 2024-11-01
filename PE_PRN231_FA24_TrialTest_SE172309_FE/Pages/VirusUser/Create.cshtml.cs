using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PE_PRN231_FA24_TrialTest_SE172309_FE.Models;

namespace PE_PRN231_FA24_TrialTest_SE172309_FE.Pages.VirusUser
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public PersonWithVirus NewPerson { get; set; } = new PersonWithVirus
        {
            Viruses = new List<PersonVirusDTO>() 
        };

        public void OnGet()
        {
            if (NewPerson.Viruses == null)
            {
                NewPerson.Viruses = new List<PersonVirusDTO>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var url = $"{Common.BaseURL}/api/persons";
            var response = await Common.SendRequestWithBody(NewPerson, url, HttpContext.Session.GetString("AccessToken"), "Post");

            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            TempData["ErrorMessage"] = "An error occurred while creating the user. Please try again.";
            return Page();
        }
    }
}
