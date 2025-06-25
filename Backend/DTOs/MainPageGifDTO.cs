using Backend.Model;

namespace Backend.DTOs;

public class MainPageGifDTO
{
    public string Name { get; set; } 
    public string Path { get; set; } 
    public string Hash { get; set; } 
    public List<Tag> Tags { get; set; } 
    
    public MainPageGifDTO(string name, string path, string hash, List<Tag> tags)
    {
        Name = name;
        Path = path;
        Hash = hash;
        Tags = tags;
    }
}