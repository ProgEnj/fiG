using Backend.Model;

namespace Backend.DTOs;

public class StorageItemRequestDTO
{
    public string Name { get; set; }
    public List<string> serializedTags { get; set; } = new List<string>();
    public IFormFile File { get; set;  }
}