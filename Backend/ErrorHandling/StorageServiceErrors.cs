namespace Backend.ErrorHandling;

public static class StorageServiceErrors
{
    public static readonly Error TagAlreadyExists = new("Tag already exists");
    public static readonly Error GifAlreadyExists = new("GIF already exists");
    public static readonly Error FailedSaveOnStorage = new("Failed to save file on storage");
    public static readonly Error ItemNotFound = new("Requested item not found");
}
