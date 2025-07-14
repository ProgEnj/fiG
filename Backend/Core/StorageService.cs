using System.Security.Claims;
using System.Security.Cryptography;
using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Identity;
using Backend.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core;

public class StorageService : IStorageService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static string PATH = "";
    private static string PATHDEVLOCAL = "";

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
     * Retrieve gifs
     * Find file by name, tags
     * Check file header for gif[x]
     */
    
    public async Task<Result<MainPageGifResponseDTO>> RetrieveGIFAsync()
    {
        var gifs = await _context.StorageItems.Take(12).Select(x =>
            new MainPageGifDTO(x.Name, x.Path, x.Hash.Substring(0, 10), x.Tags)).ToListAsync();
        
        return Result.Success(new MainPageGifResponseDTO(){gifItems = gifs});
    }
    
    public async Task<Result> UploadGIFAsync(StorageItemRequestDTO storageItemDTO)
    {
        var check = MetadataExtract.IsGIF(storageItemDTO.File);
        if(!check.IsSuccess) return Result.Failure(check.Error);

        var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        var email = identity.FindFirst(ClaimTypes.Email);
        
        var user = await _userManager.FindByEmailAsync(email.Value);
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
        foreach (var tagName in storageItemDTO.serializedTags)
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
        var path = Path.Combine(hash.Substring(0, 10), storageItemDTO.Name + ".gif");

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
        string diskPATH = "";
        if (PATHDEVLOCAL == null)
        {
            diskPATH = Path.Combine(PATH, storageItem.Path);
        }
        else
        {
            diskPATH = Path.Combine(PATHDEVLOCAL, storageItem.Path);
        }
        // TODO: Check not on disk by filename, but in db with hash, or on disk with hash.
        // Because now same files with different name get added to the same folder
        var isExists = File.Exists(diskPATH);
        if (isExists) Result.Failure(StorageServiceErrors.FileAlreadyExistsOnStorage);
        Directory.CreateDirectory(Path.GetDirectoryName(diskPATH));
        
        using (var fileStream = File.Create(diskPATH))
        {
            await file.CopyToAsync(fileStream);
        }
        
        return Result.Success();
    }
}