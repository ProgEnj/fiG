using Backend.Model;

namespace Backend.DTOs;

public class StorageItemRequestDTO
{
    public string Username { get; set; }
    public string Name { get; set; }
    public List<Tag> tags = [];
    public IFormFile File { get; set; }
}