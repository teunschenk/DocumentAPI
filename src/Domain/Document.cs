namespace DocumentAPI.Domain.Documents;

public sealed class Document
{
    private Document(Guid id, string title, string content, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static Document Create(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        var now = DateTimeOffset.UtcNow;
        return new Document(Guid.NewGuid(), title.Trim(), content, now);
    }

    public void Mutate(string content, string? title = null)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        if (title is not null && string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        }

        Content = content;
        if (title is not null)
        {
            Title = title.Trim();
        }

        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}
