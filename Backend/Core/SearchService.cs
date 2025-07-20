using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core;

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _context;

    public SearchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SearchResponseDTO>> NameSearch(string name)
    {
        if (name.Length == 0) return Result.Failure<SearchResponseDTO>(SearchErrors.EmptyQuery);
        
        var foundItems = await _context.StorageItems
            .Where(item => item.Name.ToLower().Contains(name.ToLower()))
            .Select(item => new SearchItemResponseDTO(item.Id, item.Name, item.Path))
            .ToListAsync();

        if (foundItems.Count == 0) return Result.Failure<SearchResponseDTO>(SearchErrors.NotFound);

        return new SearchResponseDTO(foundItems);
    }
    
    // Probably should just store tags in lowercase instead of .ToLower()
    // Nevermind, EF Core is too smart and uses db functions to lowercase things
    public async Task<Result<SearchResponseDTO>> TagSearch(List<string> tags)
    {
        if (tags.Count == 0) return Result.Failure<SearchResponseDTO>(SearchErrors.EmptyQuery);
        
        var foundItems = await _context.StorageItems
            .Where(
                item => item.Tags.Select(b => b.Name.ToLower())
                    .Any(tag => tags.Any(x => tag.Contains(x)))
                )
            .Select(item => new SearchItemResponseDTO(item.Id, item.Name, item.Path))
            .ToListAsync();

        if (foundItems.Count == 0) return Result.Failure<SearchResponseDTO>(SearchErrors.NotFound);

        return new SearchResponseDTO(foundItems);
    }
}