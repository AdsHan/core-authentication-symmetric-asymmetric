using ASA.Auth.API.DTO;
using ASA.Core.Authentication;
using ASA.Core.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASA.Auth.API.Service;

public class AuthService
{
    private readonly TokenSettings _tokenSettings;
    private readonly IGenerateSecurityKey _GenerateSecurityKey;

    public AuthService(TokenSettings tokenSettings, IGenerateSecurityKey GenerateSecurityKey)
    {
        _tokenSettings = tokenSettings;
        _GenerateSecurityKey = GenerateSecurityKey;
    }

    public AuthTokenDTO GetTokenSymmetric(string email)
    {
        var signingCredentials = new SigningCredentials(_GenerateSecurityKey.GetSymmetricKey(_tokenSettings.SecretJWTKey), SecurityAlgorithms.HmacSha256);
        return GenerateToken(email, signingCredentials);
    }
    public AuthTokenDTO GetTokenAsymmetric(string email)
    {
        var signingCredentials = new SigningCredentials(_GenerateSecurityKey.GetAsymmetricKeyPrivate(), SecurityAlgorithms.RsaSsaPssSha256);
        return GenerateToken(email, signingCredentials);
    }

    public AuthTokenDTO GenerateToken(string email, SigningCredentials signingCredentials)
    {
        // Define as claims do usuário (não é obrigatório mas cria mais chaves no Payload)
        var claims = new[]
        {
             new Claim(JwtRegisteredClaimNames.UniqueName, email),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Tempo de expiracão do token            
        var expiration = DateTime.UtcNow.AddSeconds(_tokenSettings.ExpireSeconds);

        // Monta as informações do token
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        // Retorna o token e demais informações
        var response = new AuthTokenDTO
        {
            Authenticated = true,
            Expiration = expiration,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
            Message = "Token JWT OK",
            UserToken = new UserTokenDTO
            {
                Email = email
            }
        };

        return response;
    }
}

