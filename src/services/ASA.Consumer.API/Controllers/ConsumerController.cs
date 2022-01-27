using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASA.Consumer.API.Controllers;

[Route("api/consumer")]
[ApiController]
public class ConsumerController : ControllerBase
{

    // GET api/auth-symmetric
    /// <summary>
    /// Teste Simétrico
    /// </summary>   
    /// <returns>Token de autenticação</returns>                
    /// <response code="200">Foi realizado o login corretamente</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpGet("symmetric")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = "symmetric")]
    public IActionResult SignInSymmetric()
    {
        return Ok();
    }

    // GET api/auth-asymmetric
    /// <summary>
    /// Teste Assimétrico
    /// </summary>       
    /// <response code="200">Foi realizado o login corretamente</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpGet("asymmetric")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = "asymmetric")]
    public IActionResult SignInAsymmetricAsync()
    {
        return Ok();
    }

}

