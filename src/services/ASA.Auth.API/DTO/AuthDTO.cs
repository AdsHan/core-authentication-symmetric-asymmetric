namespace ASA.Auth.API.DTO;

public class AccessCredentialsDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserTokenDTO
{
    public string Id { get; set; }
    public string Email { get; set; }

}

public class AuthTokenDTO
{
    public bool Authenticated { get; set; }
    public DateTime Expiration { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Message { get; set; }
    public UserTokenDTO UserToken { get; set; }
}
