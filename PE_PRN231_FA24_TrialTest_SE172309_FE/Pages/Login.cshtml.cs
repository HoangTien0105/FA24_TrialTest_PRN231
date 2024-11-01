using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PE_PRN231_FA24_TrialTest_SE172309_FE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        public async Task<IActionResult> OnPostLoginAsync()
        {
            var account = new { Email, Password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7223/api/login", account);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

                if (apiResponse?.Result == null || string.IsNullOrEmpty(apiResponse.Result.Token) || apiResponse.Result.User == null)
                {
                    ModelState.AddModelError(string.Empty, "The server did not return a valid response.");
                    return Page();
                }

                HttpContext.Session.SetString("AccessToken", apiResponse.Result.Token);
                HttpContext.Session.SetString("Role", apiResponse.Result.User.Role.ToString());

                return RedirectToPage("/VirusUser/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }

    public class ApiResponse
    {
        public Result Result { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
    }

    public class Result
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }



}
