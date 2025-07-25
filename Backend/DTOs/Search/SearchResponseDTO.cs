namespace Backend.DTOs;

public class SearchResponseDTO
{
    public List<SearchItemResponseDTO> SearchItems { get; set; }
    
    public SearchResponseDTO(List<SearchItemResponseDTO> searchItems)
    {
        SearchItems = searchItems;
    }
}