using Backend.Core;
using Backend.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SearchController(ISearchService _searchService) : ControllerBase
{
    [HttpGet("byname")]
    public async Task<IActionResult> NameSearch([FromQuery(Name = "name")] string name)
    {
        var result = await _searchService.NameSearch(name);
        if (result.Error == SearchErrors.EmptyQuery) return StatusCode(400, result.Error.Message);
        if (result.Error == SearchErrors.NotFound) return StatusCode(404, result.Error.Message);

        return Ok(result.Value);
    }
    
    [HttpGet("bytags")]
    public async Task<IActionResult> TagSearch([FromQuery(Name = "tags")] string[] tags)
    {
        var result = await _searchService.TagSearch(tags.ToList());
        
        if (result.Error == SearchErrors.EmptyQuery) return StatusCode(400, result.Error.Message);
        if (result.Error == SearchErrors.NotFound) return StatusCode(404, result.Error.Message);

        return Ok(result.Value);
    }
}