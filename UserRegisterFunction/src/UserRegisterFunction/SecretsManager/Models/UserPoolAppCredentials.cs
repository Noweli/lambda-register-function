using Newtonsoft.Json;

namespace UserRegisterFunction.SecretsManager.Models;

public class UserPoolAppCredentials
{
    [JsonProperty("appClientId")] public string? AppClientId { get; set; }
}