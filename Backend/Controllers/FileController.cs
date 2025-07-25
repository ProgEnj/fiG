using System.Text.Json;
using Backend.Core;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FileController(IStorageService _storageService) : ControllerBase
{
    [HttpGet("mainpage")]
    public async Task<IActionResult> RetrieveMainPageGifs()
    {
        var result = await _storageService.RetrieveMainPageGifsAsync();
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Error.Message);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGif([FromRoute] int id)
    {
        var result = await _storageService.GetGifAsync(id);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(404, result.Error.Message);
    }
    
    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] StorageItemRequestDTO storageItemDTO, [FromForm] string tags)
    {
        // the "[FromForm] tags" are needed because
        // asp.net core can't properly serialize json from Mulipart form
        // even with the "application/json" header or at least i failed
        storageItemDTO.serializedTags = JsonSerializer.Deserialize<List<string>>(tags);
        var result = await _storageService.UploadGIFAsync(storageItemDTO);
        return result.IsSuccess ? Ok() : StatusCode(500, result.Error.Message);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> RemoveGif([FromRoute] int id)
    {
        var result = await _storageService.DeleteGIFAsync(id);
        return result.IsSuccess ? Ok() : StatusCode(500, result.Error.Message);
    }
}