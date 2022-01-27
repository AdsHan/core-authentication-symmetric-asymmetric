using Microsoft.IdentityModel.Tokens;

namespace ASA.Core.Authentication;

public interface IGenerateSecurityKey
{
    SymmetricSecurityKey GetSymmetricKey(string secretJWTKey);
    JsonWebKey GetAsymmetricKeyPublic();
    JsonWebKey GetAsymmetricKeyPrivate();
}

