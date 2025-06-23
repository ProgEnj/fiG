namespace Backend.ErrorHandling;

public static class StorageServiceErrors
{
    public static readonly Error TagAlreadyExists = new("Tag already exists");
    public static readonly Error GifAlreadyExists = new("Gif already exists");
    public static readonly Error FileAlreadyExistsOnStorage = new("File already exists on storage");
    public static readonly Error FailedSaveOnStorage = new("Failed to save file on storage");
}
