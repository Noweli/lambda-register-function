using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using UserRegisterFunction.SecretsManager.Models;

namespace UserRegisterFunction.Register;

public class RegisterHandler : IRegisterHandler
{
    public async Task<bool> Register(RegisterModel registerModel, UserPoolAppCredentials userPoolAppCredentials)
    {
        var identityProviderClient = new AmazonCognitoIdentityProviderClient();

        var userAttributes = new List<AttributeType>
        {
            new() { Name = "email", Value = registerModel.Email }
        };

        var signUpRequest = new SignUpRequest
        {
            ClientId = userPoolAppCredentials.AppClientId,
            UserAttributes = userAttributes,
            Username = registerModel.Email,
            Password = registerModel.Password
        };

        var response = await identityProviderClient.SignUpAsync(signUpRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}