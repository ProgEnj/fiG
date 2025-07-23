using System.Security.Claims;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly int _mainPageGifNumber = 8;
    private readonly int _folderNameLength = 10;
    private static string PATHDEVLOCAL = "";
    private static string PATH = "";

    public StorageService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        PATH = configuration.GetSection("CoreSettings").GetValue<string>("PATH");
        PATHDEVLOCAL = configuration.GetSection("CoreSettings").GetValue<string>("PATHDEVLOCAL");
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    /*
     * Recieve file[x], check metadata[x], initialize object[x], check for already exists[x], create db entry[x], save on storage[x]
     * Remove file, Remove db entry
     * Retrieve gifs [x]
     * Find file by name, tags [x]
     * Check file header for gif[x]
     */
    
    public async Task<Result<GifDTO>> GetGifAsync(int id)
    {
        // For some reason foundItem returned
        // from query with FirstOrDefaultAsync includes Storageitem's in List<Tag>
        // they have many-to-many relationship so on serialization it goes to endless
        // cycle GifDTO -> List<Tags> -> StorageItem<Tags> -> ... and crash
        // this is not happening when dto is created from select.
        
        // Investigate or/and fix
        
        // EF core populates navigation properties on demand
        // Returned StorageItem object will have cycle thing
        // The one projected to DTO will have navigation properties on demand. 
        // To include more use .Include(...).ThenInclude(...)

        var foundItem = await _context.StorageItems
            .Where(foundItem => foundItem.Id == id)
            .Select(foundItem => new GifDTO(foundItem.Id, foundItem.Name, foundItem.User.UserName, foundItem.Path, foundItem.Hash, 
                foundItem.Tags.Select(tag => new TagDTO(tag.Id, tag.Name)).ToList()))
            .FirstOrDefaultAsync();

        if (foundItem == null) return Result.Failure<GifDTO>(StorageServiceErrors.ItemNotFound);
        
        return Result.Success(foundItem);
    }
    
    public async Task<Result<MainPageGifResponseDTO>> RetrieveMainPageGifsAsync()
    {
        var gifs = await _context.StorageItems.Take(_mainPageGifNumber).Select(x =>
            new GifDTO(x.Id, x.Name, x.User.UserName, x.Path, x.Hash.Substring(0, _folderNameLength), 
                x.Tags.Select(tag => new TagDTO(tag.Id, tag.Name)).ToList()
            )).ToListAsync();
        
        return Result.Success(new MainPageGifResponseDTO(){gifItems = gifs});
    }
    
    public async Task<Result> UploadGIFAsync(StorageItemRequestDTO storageItemDTO)
    {
        string hash = string.Empty;
        using (var stream = storageItemDTO.File.OpenReadStream())
        {
            using (var sha256 = SHA256.Create())
            {
                hash = string.Join("", await sha256.ComputeHashAsync(stream));
            }
        }

        if (await isGifExistsOnStorage(hash)) return Result.Failure(StorageServiceErrors.GifAlreadyExists);
        
        var isGif = MetadataExtract.IsGIF(storageItemDTO.File);
        if(!isGif.IsSuccess) return Result.Failure(isGif.Error);

        var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        var email = identity.FindFirst(ClaimTypes.Email);
        
        var user = await _userManager.FindByEmailAsync(email.Value);
        if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);
        
        // Check if tags already exist
        List<Tag> tags = new List<Tag>();
        foreach (var tagName in storageItemDTO.serializedTags)
        {
            var foundTag = _context.Tags.FirstOrDefault(x => x.Name == tagName);
            if (foundTag == null)
            {
                tags.Add(new Tag() { Name = tagName }); 
                continue;
            }
            // Already existing objects will not be added to db
            tags.Add(foundTag);
        }
        
        var created = DateTime.UtcNow;
        
        var dimensions = MetadataExtract.ExtractGIFDimensions(storageItemDTO.File);
        var path = Path.Combine(hash.Substring(0, _folderNameLength), storageItemDTO.Name + ".gif").Replace('\\', '/');

        var storageItem = new StorageItem()
        {
            User = user, Hash = hash, Name = storageItemDTO.Name,
            Tags = tags, Path = path, Created = created, Width = dimensions.width, Height = dimensions.height 
        };

        await _context.StorageItems.AddAsync(storageItem);
        await _context.SaveChangesAsync();
        
        var saveResult = await SaveInStorageAsync(storageItem, storageItemDTO.File);
        if (!saveResult.IsSuccess) return Result.Failure(StorageServiceErrors.FailedSaveOnStorage);
        
        return Result.Success();
    }

    public string GetDiskPath()
    {
        return PATHDEVLOCAL == null ? PATH : PATHDEVLOCAL;
    }

    private async Task<Result> SaveInStorageAsync(StorageItem storageItem, IFormFile file)
    {
        if(await this.isGifExistsOnStorage(storageItem.Hash))
            return Result.Failure(StorageServiceErrors.GifAlreadyExists);
        
        var gifDiskPath = Path.Combine(this.GetDiskPath(), storageItem.Path.Replace('\\', '/'));
        
        Directory.CreateDirectory(Path.GetDirectoryName(gifDiskPath));
        
        using (var fileStream = File.Create(gifDiskPath))
        {
            await file.CopyToAsync(fileStream);
        }
        
        return Result.Success();
    }

    public async Task<bool> isGifExistsOnStorage(string hash)
    {
        var directoryPath = Path.Combine(this.GetDiskPath(), hash.Substring(0, this._folderNameLength));
        
        if (!Directory.Exists(directoryPath)) return false;
        
        var file = Directory.GetFiles(directoryPath).FirstOrDefault();
        if (file == null) return false;
        
        string foundFileHash = string.Empty;
        using (var stream = File.OpenRead(Path.Combine(directoryPath, file)))
        {
            using (var sha256 = SHA256.Create())
            {
                foundFileHash = string.Join("", await sha256.ComputeHashAsync(stream));
            }
        }

        return foundFileHash == hash;
    }
    
    public async Task<Result> DeleteGIFAsync(int id)
    {
        var storageItem = await _context.StorageItems.FirstOrDefaultAsync(item => item.Id == id);
        if(storageItem == null) Result.Failure(StorageServiceErrors.ItemNotFound);

        if(!await isGifExistsOnStorage(storageItem.Hash)) 
            return Result.Failure(StorageServiceErrors.ItemNotFound);

        _context.StorageItems.Remove(storageItem);
        await _context.SaveChangesAsync();

        var gifDiskPath = Path.Combine(this.GetDiskPath(), storageItem.Path.Replace('\\', '/'));
        var directoryPath = Path.Combine(this.GetDiskPath(), storageItem.Hash.Substring(0, this._folderNameLength));
        
        File.Delete(gifDiskPath);
        Directory.Delete(directoryPath);
        
        return Result.Success();
    }
}