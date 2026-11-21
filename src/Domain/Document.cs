namespace DocumentAPI.Domain.Documents;

public sealed class Document
{
    internal Document(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public Guid Id { get; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; }

    public static Document Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        return new Document(Guid.NewGuid(), content);
    }

    public void AddContent(string text)
    {
        Content += text;
    }
}
