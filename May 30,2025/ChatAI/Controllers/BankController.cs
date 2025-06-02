using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ChatAI.Controllers
{
    public class BankController
    {
        [HttpGet("/chat")]
        public async Task<IActionResult> Chat(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return new BadRequestObjectResult("Message cannot be empty.");
            }
            var apiResult = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "http://127.0.0.1:5000/ask";
                var content = new StringContent($"{{\"question\":\"{message}\"}}", System.Text.Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync(apiUrl, content);
                apiResult = await apiResponse.Content.ReadAsStringAsync();
            }
            return new OkObjectResult(apiResult);
        }
    }
}