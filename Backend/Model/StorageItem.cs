using Microsoft.EntityFrameworkCore;

namespace Backend.Model;

[Index(nameof(Hash), IsUnique = true)]
public class StorageItem
{
    public int Id { get; set; }
    public string UserID  { get; set; }
    public string Hash { get; set; }
    public string Name { get; set; }
    public List<Tag> Tags { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}