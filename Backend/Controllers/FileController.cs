using System.Text.Json;
using Backend.Core;
using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(IStorageService _storageService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] StorageItemRequestDTO storageItemDTO, [FromForm] string tags)
    {
        storageItemDTO.tags = JsonSerializer.Deserialize<List<string>>(tags);
        var result = await _storageService.UploadGIFAsync(storageItemDTO);
        return result.IsSuccess ? Ok() : StatusCode(500, result.Error.Message);
    }
    
    [HttpGet("mainpage")]
    public async Task<IActionResult> RetrieveMainPageGifs()
    {
        var result = await _storageService.RetrieveGIFAsync();
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Error.Message);
    }
}