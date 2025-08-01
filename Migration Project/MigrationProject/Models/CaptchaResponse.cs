using Newtonsoft.Json;

namespace MigrationProject.Models;

public class CaptchaResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("error-codes")]
    public List<string> ErrorCodes { get; set; } = new();
}