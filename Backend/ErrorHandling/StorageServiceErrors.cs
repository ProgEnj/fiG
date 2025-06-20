namespace Backend.ErrorHandling;

public static class StorageServiceErrors
{
    public static readonly Error TagAlreadyExists = new("TagAlreadyExists");
    public static readonly Error GifAlreadyExists = new("GifAlreadyExists");
}
