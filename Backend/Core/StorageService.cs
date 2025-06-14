using System.Security.Cryptography;
using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Identity;
using Backend.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Backend.Core;

public class StorageService : IStorageService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public static string PATH = "";

    public StorageService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context)
    {
        _userManager = userManager;
        PATH = configuration.GetSection("CoreSettings").GetValue<string>("PATH");
        _context = context;
    }
    
    /*
     * Recieve file, check metadata, Initialize object, check for already exists, create db entry, save on storage
     * Remove file, Remove db entry
     * Find file by name, tags
     * Check file header for gif
     *
     * Order: Receive from frontend -> check isGif -> check if user exists -> check if gif already exists ->
     * check if tags already exists -> add new tags -> store on disk -> create db entry
     */

    // public Task<Result<bool>> isFileAlreadyExists()
    // {
    // }
    
    // public async Task<Result> CreateDBEntryAsync(StorageItemRequestDTO storageItemDTO)
    // {
    // }
    
    // public async Task<Result> StoreFileAsync(StorageItem item)
    // {
    // }
    
    public async Task<Result> UploadGIFAsync(StorageItemRequestDTO storageItemDTO)
    {
        var check = MetadataExtract.IsGIF(storageItemDTO.File);
        if(!check.IsSuccess) return Result.Failure(check.Error);
        
        var user = await _userManager.FindByNameAsync(storageItemDTO.Username);
        if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);
        
        string hash = string.Empty;
        using (var stream = storageItemDTO.File.OpenReadStream())
        {
            using (var sha256 = SHA256.Create())
            {
                hash = string.Join("", await sha256.ComputeHashAsync(stream));
            }
        }
        
        // Check if gif already exists by hash
        
        var created = DateTime.UtcNow;
    
        var dimensions = MetadataExtract.ExtractGIFDimensions(storageItemDTO.File);
        var path = Path.Combine(PATH, hash, storageItemDTO.Name);

        var storageItem = new StorageItem()
        {
            UserID = user.Id, Hash = hash, Name = storageItemDTO.Name,
            Tags = storageItemDTO.tags, Path = path, Created = created, Width = dimensions.width, Height = dimensions.height 
        };

        await _context.StorageItems.AddAsync(storageItem);
        await _context.SaveChangesAsync();
        Console.WriteLine("I'm here!");
        
        return Result.Success();
    }
}