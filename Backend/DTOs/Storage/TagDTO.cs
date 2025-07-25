namespace Backend.DTOs;

public class TagDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public TagDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }
}