using Backend.Model;

namespace Backend.DTOs;

public class StorageItemRequestDTO
{
    public string Username { get; set; }
    public string Name { get; set; }
    public List<string> serializedTags { get; set; }
    public IFormFile File { get; set; }
}