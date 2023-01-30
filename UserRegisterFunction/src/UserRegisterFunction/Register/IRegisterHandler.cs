using System.Threading.Tasks;
using UserRegisterFunction.SecretsManager.Models;

namespace UserRegisterFunction.Register;

public interface IRegisterHandler
{
    public Task<bool> Register(RegisterModel registerModel, UserPoolAppCredentials userPoolAppCredentials);
}