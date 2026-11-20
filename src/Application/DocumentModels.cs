namespace DocumentAPI.Application.Documents;

public sealed record UploadDocumentCommand(string Title, string Content);

public sealed record MutateDocumentCommand(string Content, string? Title);

public sealed record DocumentDto(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc);
