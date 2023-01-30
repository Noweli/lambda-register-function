using Newtonsoft.Json;

namespace UserRegisterFunction.Register;

public class RegisterModel
{
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("password")] public string? Password { get; set; }
}