namespace DocumentAPI.Domain.Documents;

public sealed class Document
{
    internal Document(Guid id, string content, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Content = content;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static Document Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        var now = DateTimeOffset.UtcNow;
        return new Document(Guid.NewGuid(), content, now);
    }

    public void AddContent(string text)
    {
        Content += text;
    }
}
