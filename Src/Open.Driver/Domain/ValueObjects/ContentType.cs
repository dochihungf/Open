namespace Open.Driver.Domain.ValueObject;

public class ContentType : Core.SeedWork.ValueObject
{
    private ContentType(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static ContentType Article => new ContentType("Article", "A written work");
    public static ContentType Video => new ContentType("Video", "A recorded visual content");
    public static ContentType Image => new ContentType("Image", "A visual representation");
    public static ContentType Audio => new ContentType("Audio", "An auditory work");

    public static ContentType From(string name)
    {
        var contentType = new ContentType(name, string.Empty);

        if (!SupportedContentTypes.Contains(contentType))
        {
            throw new UnsupportedContentTypeException(name);
        }

        return contentType;
    }

    public static implicit operator string(ContentType contentType)
    {
        return contentType.ToString();
    }

    public static explicit operator ContentType(string name)
    {
        return From(name);
    }

    public override string ToString()
    {
        return Name;
    }

    protected static IEnumerable<ContentType> SupportedContentTypes
    {
        get
        {
            yield return Article;
            yield return Video;
            yield return Image;
            yield return Audio;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}

// Exception class for unsupported content types
public class UnsupportedContentTypeException : Exception
{
    public UnsupportedContentTypeException(string name) 
        : base($"The content type '{name}' is not supported.")
    {
    }
}
