using Microsoft.AspNetCore.Mvc;
using NewsScope.Api.Models;
using NewsScope.Api.Services;

namespace NewsScope.Api.Controllers;

[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
    private readonly INewsScoreService _newsScoreService;

    public NewsController(INewsScoreService newsScoreService)
    {
        _newsScoreService = newsScoreService;
    }

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] NewsRequestDto request)
    {
        try
        {
            var score = _newsScoreService.CalculateScore(request);

            return Ok(new NewsResponseDto
            {
                Score = score
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }
}
