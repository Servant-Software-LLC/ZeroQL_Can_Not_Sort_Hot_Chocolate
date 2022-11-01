namespace HotChocoServer;

public class Query
{
    [UseSorting]
    public Author[] GetAuthors() =>
        new Author[]
        {
            new() { Name = "Scott Hanselman" },
            new() { Name = "Jon Skeet" },
        };
}

