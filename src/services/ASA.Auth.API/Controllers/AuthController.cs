using ASA.Auth.API.DTO;
using ASA.Auth.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace RTO.Auth.API.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth-symmetric
        /// <summary>
        /// Geração Simétrica
        /// </summary>   
        /// <returns>Token de autenticação</returns>                
        /// <response code="200">Foi realizado o login corretamente</response>                
        /// <response code="400">Falha na requisição</response>         
        [HttpPost("symmetric")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SignInSymmetric([FromBody] AccessCredentialsDTO credentials)
        {
            return Ok(_authService.GetTokenSymmetric(credentials.Email));
        }

        // POST api/auth-asymmetric
        /// <summary>
        /// Geração Assimétrica
        /// </summary>   
        /// <returns>Token de autenticação</returns>                
        /// <response code="200">Foi realizado o login corretamente</response>                
        /// <response code="400">Falha na requisição</response>         
        [HttpPost("asymmetric")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SignInAsymmetricAsync([FromBody] AccessCredentialsDTO credentials)
        {
            return Ok(_authService.GetTokenAsymmetric(credentials.Email));
        }

    }
}
