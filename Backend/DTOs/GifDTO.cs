using Backend.Model;

namespace Backend.DTOs;

public class GifDTO
{
    public int Id { get; set; } 
    public string Name { get; set; } 
    public string Username { get; set; } 
    public string Path { get; set; } 
    public string Hash { get; set; } 
    public List<TagDTO> Tags { get; set; } 
    
    public GifDTO(int id, string name, string username, string path, string hash, List<TagDTO> tags)
    {
        Id = id;
        Name = name;
        Username = username;
        Path = path;
        Hash = hash;
        Tags = tags;
    }
}