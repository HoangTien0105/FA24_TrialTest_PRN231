using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace PE_PRN231_FA24_TrialTest_SE172309_FE
{
    public class Common
    {
        public static string BaseURL = "https://localhost:7223";
        public static async Task<HttpResponseMessage> SendGetRequest(string url, string? jwt = null)
        {
            var httpClient = new HttpClient();
            if (jwt != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
            return await httpClient.GetAsync(url);
        }

        public static async Task<HttpResponseMessage?> SendRequestWithBody<T>(T? body, string url, string? jwt = null, string? method = "Post")
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (jwt != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
            using StringContent jsonContent = new(
                                JsonSerializer.Serialize(body),
                                Encoding.UTF8,
                                "application/json");
            if (method == "Post") return await httpClient.PostAsync(url, jsonContent);
            if (method == "Put") return await httpClient.PutAsync(url, jsonContent);
            if (method == "Delete") return await httpClient.DeleteAsync(url);
            return null;
        }
    }
}
