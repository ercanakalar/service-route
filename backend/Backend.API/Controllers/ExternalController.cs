using System.Threading.Tasks;
using Backend.Service.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/external")]
public class ExternalController : ControllerBase
{
    private readonly ExternalApiService _externalApiService;

    public ExternalController(ExternalApiService externalApiService)
    {
        _externalApiService = externalApiService;
    }

    [HttpGet("fetch")]
    public async Task<IActionResult> FetchExternalData()
    {
        string apiUrl = "https://jsonplaceholder.typicode.com/posts";
        var data = await _externalApiService.FetchDataAsync(apiUrl);
        return Ok(data);
    }
}
