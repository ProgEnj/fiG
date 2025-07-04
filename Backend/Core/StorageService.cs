using System.Security.Cryptography;
using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Identity;
using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
     * Recieve file[x], check metadata[x], initialize object[x], check for already exists[x], create db entry[x], save on storage[x]
     * Remove file, Remove db entry
     * Retrieve gifs
     * Find file by name, tags
     * Check file header for gif[x]
     */

    public async Task<Result<List<MainPageGifDTO>>> RetrieveGIFAsync()
    {
        var gifs = await _context.StorageItems.Take(10).Select(x =>
            // TODO: Decide if its better to do on frontend (separator replace)
            new MainPageGifDTO(x.Name, x.Path.Replace("\\", "/"), x.Hash.Substring(0, 10), x.Tags)).ToListAsync();
        
        return Result.Success(gifs);
    }
    
    // TODO: Refactor this, the name should be picked up from db by refresh token, we can't trust frontend
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
        
        // Check if tags already exist
        List<Tag> tags = new List<Tag>();
        foreach (var tagName in storageItemDTO.tags)
        {
            var foundTag = _context.Tags.FirstOrDefault(x => x.Name == tagName);
            if (foundTag == null)
            {
                tags.Add(new Tag() { Name = tagName }); 
                continue;
            }
            tags.Add(foundTag);
        }
        
        var created = DateTime.UtcNow;
    
        var dimensions = MetadataExtract.ExtractGIFDimensions(storageItemDTO.File);
        var path = Path.Combine(PATH, hash.Substring(0, 10), storageItemDTO.Name + ".gif");

        var storageItem = new StorageItem()
        {
            UserID = user.Id, Hash = hash, Name = storageItemDTO.Name,
            Tags = tags, Path = path, Created = created, Width = dimensions.width, Height = dimensions.height 
        };

        await _context.StorageItems.AddAsync(storageItem);
        
        var saveResult = await SaveInStorageAsync(storageItem, storageItemDTO.File);
        if (!saveResult.IsSuccess) return Result.Failure(StorageServiceErrors.FailedSaveOnStorage);
        
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> SaveInStorageAsync(StorageItem storageItem, IFormFile file)
    {
        var isExists = File.Exists(storageItem.Path);
        if (isExists) Result.Failure(StorageServiceErrors.FileAlreadyExistsOnStorage);
        Directory.CreateDirectory(Path.GetDirectoryName(storageItem.Path));
        
        using (var fileStream = File.Create(storageItem.Path))
        {
            await file.CopyToAsync(fileStream);
        }
        
        return Result.Success();
    }
}