using System.Text.Json;
using Backend.Core;
using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Model;
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
    public async Task<IActionResult> GetGif(int id)
    {
        var result = await _storageService.GetGifAsync(id);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(404, result.Error.Message);
    }
    
    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] StorageItemRequestDTO storageItemDTO, [FromForm] string tags)
    {
        storageItemDTO.serializedTags = JsonSerializer.Deserialize<List<string>>(tags);
        var result = await _storageService.UploadGIFAsync(storageItemDTO);
        return result.IsSuccess ? Ok() : StatusCode(500, result.Error.Message);
    }
}