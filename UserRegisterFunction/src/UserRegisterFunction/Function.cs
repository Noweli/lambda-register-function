using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using UserRegisterFunction.Register;
using UserRegisterFunction.SecretsManager;
using UserRegisterFunction.SecretsManager.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserRegisterFunction;

public class Function
{
    private readonly string? _matchUpSiteAppSecretName = Environment.GetEnvironmentVariable("UserPoolAppSecret");
    private ILambdaContext _lambdaContext = null!;

    public async Task FunctionHandler(RegisterModel registerModel, ILambdaContext context)
    {
        _lambdaContext = context;

        if (string.IsNullOrWhiteSpace(_matchUpSiteAppSecretName))
        {
            context.Logger.LogCritical("Failed to retrieve secret name from environment variables.");

            return;
        }

        var userPoolCredentials = await GetUserPoolCredentials();

        if (userPoolCredentials is null)
        {
            return;
        }

        var registerHandler = new RegisterHandler();

        var isRegistrationSuccessful = await registerHandler.Register(registerModel, userPoolCredentials);

        if (isRegistrationSuccessful)
        {
            context.Logger.LogInformation("Successfully registered new user.");

            return;
        }
        
        context.Logger.LogError("Failed to register new user. Register response was not 200 OK.");
    }

    private async Task<UserPoolAppCredentials?> GetUserPoolCredentials()
    {
        var secretsManagerHandler = new SecretsManagerHandler();

        var secretResponse = await secretsManagerHandler.RetrieveSecret(_matchUpSiteAppSecretName!);

        if (string.IsNullOrWhiteSpace(secretResponse))
        {
            _lambdaContext.Logger.LogCritical("Failed to retrieve secret. Secret is null or empty.");
        }

        var deserializedSecret = JsonConvert.DeserializeObject<UserPoolAppCredentials>(secretResponse);

        if (!string.IsNullOrWhiteSpace(deserializedSecret?.AppClientId))
        {
            return deserializedSecret;
        }

        _lambdaContext.Logger.LogCritical(
            "Failed to deserialize secret. AppClientId is null or empty.");

        return null!;
    }
}