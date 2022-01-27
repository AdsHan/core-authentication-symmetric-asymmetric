using ASA.Core.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ASA.Core.Authentication;

public class GenerateSecurityKey : IGenerateSecurityKey
{
    private readonly IDistributedCache _cache;
    private readonly TokenSettings _tokenSettings;

    private JsonWebKey _keyPublic;
    private JsonWebKey _keyPrivate;

    public GenerateSecurityKey(IDistributedCache cache, TokenSettings tokenSettings)
    {
        _tokenSettings = tokenSettings;
        _cache = cache;
        _cache.Remove("PublicKey");
    }

    public SymmetricSecurityKey GetSymmetricKey(string secretJWTKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretJWTKey));
    }

    public JsonWebKey GetAsymmetricKeyPublic()
    {
        _keyPublic = GetPublicKey();

        return _keyPublic;
    }

    public JsonWebKey GetAsymmetricKeyPrivate()
    {
        if (GetPublicKey() != null) return _keyPrivate;

        GenerateNewKeys();

        return _keyPrivate;
    }

    private void GenerateNewKeys()
    {
        var rsa = new RsaSecurityKey(RSA.Create(2048))
        {
            KeyId = Guid.NewGuid().ToString()
        };

        var parametrosPrivados = rsa.Rsa.ExportParameters(true);
        _keyPrivate = JsonWebKeyConverter.ConvertFromRSASecurityKey(new RsaSecurityKey(RSA.Create(parametrosPrivados)));

        var parametrosPublicos = rsa.Rsa.ExportParameters(false);
        _keyPublic = JsonWebKeyConverter.ConvertFromRSASecurityKey(new RsaSecurityKey(RSA.Create(parametrosPublicos)));

        // Validade da cahve pública (O Redis irá invalidar o registro automaticamente de acordo com a validade)            
        TimeSpan finalExpiration = TimeSpan.FromSeconds(_tokenSettings.FinalExpirationPublicKeySeconds);

        DistributedCacheEntryOptions optionsCache = new DistributedCacheEntryOptions();
        optionsCache.SetAbsoluteExpiration(finalExpiration);

        // Grava a chave pública
        _cache.SetString("PublicKey", JsonConvert.SerializeObject(_keyPublic), optionsCache);
    }

    private JsonWebKey GetPublicKey()
    {
        string key = _cache.GetString("PublicKey");

        if (!String.IsNullOrWhiteSpace(key))
        {
            return JsonConvert.DeserializeObject<JsonWebKey>(key);
        }

        return null;

    }

}

