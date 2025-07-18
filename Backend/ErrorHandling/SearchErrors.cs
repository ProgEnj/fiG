namespace Backend.ErrorHandling;

public static class SearchErrors
{
    public static readonly Error NotFound = new("No gifs matching this name");
    public static readonly Error EmptyQuery = new("Search query is empty");
}