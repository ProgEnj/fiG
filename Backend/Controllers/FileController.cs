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
    public async Task<IActionResult> UploadFile([FromForm] StorageItemRequestDTO storageItem, [FromForm] string tags)
    {
        storageItem.tags = JsonSerializer.Deserialize<List<string>>(tags).Select(x => new Tag { Name = x }).ToList();
        // Console.WriteLine(storageItem.Name + storageItem.Username + " tags: " + string.Join(", ", storageItem.tags.Select(x => x.Name)));
        
        return await _storageService.UploadGIFAsync(storageItem) == Result.Success() ? Ok() : StatusCode(500);
    }
}