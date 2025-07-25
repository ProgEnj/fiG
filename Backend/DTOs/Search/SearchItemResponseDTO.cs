namespace Backend.DTOs;

public class SearchItemResponseDTO
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Uri { get; set; }
    
    public SearchItemResponseDTO(int id, string name, string uri)
    {
        Id = id;
        Name = name;
        Uri = uri;
    }
}