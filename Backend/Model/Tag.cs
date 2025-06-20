using Microsoft.EntityFrameworkCore;

namespace Backend.Model;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StorageItem> StorageItems { get; set; }
}