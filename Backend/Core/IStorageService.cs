using Backend.DTOs;
using Backend.ErrorHandling;

namespace Backend.Core;

public interface IStorageService
{
    public Task<Result<MainPageGifResponseDTO>> RetrieveMainPageGifsAsync();
    public Task<Result> UploadGIFAsync(StorageItemRequestDTO storageItemDTO);
    public Task<Result<GifDTO>> GetGifAsync(int id);
}