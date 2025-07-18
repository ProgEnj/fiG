using Backend.DTOs;
using Backend.ErrorHandling;

namespace Backend.Core;

public interface ISearchService
{
    public Task<Result<SearchResponseDTO>> TagSearch(List<string> tags);
    public Task<Result<SearchResponseDTO>> NameSearch(string name);
}