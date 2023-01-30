using System.Threading.Tasks;

namespace UserRegisterFunction.SecretsManager;

public interface ISecretsManagerHandler
{
    public Task<string> RetrieveSecret(string secretName);
}