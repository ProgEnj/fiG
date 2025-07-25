namespace Backend.ErrorHandling;

public static class MetadataExtractErrors
{
    public static readonly Error BadExtension = new("File has bad extentsion");
    public static readonly Error NoExtension = new("File has no extension");

    public static Error WrongExtenstion(string extension)
    {
        return new Error($"File is not a {extension}");
    }
}