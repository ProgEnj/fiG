using Backend.ErrorHandling;
using Backend.Model;

namespace Backend.Core;

public static class MetadataExtract
{
    private static string GIFHeader89a = "474946383961"; // GIF89a
    private static string GIFHeader87a = "474946383761"; // GIF87a

    public static Result IsGIF(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName);
        
        if (ext == null) return Result.Failure(FileHeaderCheckErrors.BadExtension); 
        if (ext == string.Empty) return Result.Failure(FileHeaderCheckErrors.NoExtension); 
        if (String.CompareOrdinal(ext, ".gif") != 0) return Result.Failure(FileHeaderCheckErrors.BadExtension);
        
        using (var stream = file.OpenReadStream())
        {
            var reader = new BinaryReader(stream).ReadBytes(6);
            var hex = BitConverter.ToString(reader).Replace("-", string.Empty);
            
            return (String.CompareOrdinal(hex, GIFHeader87a) == 0) || (String.CompareOrdinal(hex, GIFHeader89a) == 0) 
                ? Result.Success() : Result.Failure(FileHeaderCheckErrors.WrongExtenstion(".gif"));
        }
    }

    public static (int width, int height) ExtractGIFDimensions(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            stream.Seek(6, SeekOrigin.Current);
            var width = Convert.ToInt32(new BinaryReader(stream).ReadInt16());
            var height = Convert.ToInt32(new BinaryReader(stream).ReadInt16());

            return (width, height);
        }
    }
}